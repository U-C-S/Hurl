using Hurl.App.Helpers;
using Hurl.App.Services;
using Hurl.App.Services.Interfaces;
using Hurl.App.ViewModels;
using Hurl.App.Windows;
using Hurl.Library;
using Hurl.Library.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using Microsoft.Windows.AppLifecycle;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using WinUIEx;

namespace Hurl.App;

public partial class App : Microsoft.UI.Xaml.Application
{
    public static IServiceProvider? Services { get; private set; }

    private static SelectorWindow? _selectorWindow;
    private static SettingsWindow? _settingsWindow;
    private readonly DispatcherQueue dispatcherQueue;
    private AppActivationArguments? _pendingActivationArgs;
    private bool isLaunched;
    private TrayService? trayService;

    internal static bool IsExiting { get; private set; }

    public App()
    {
        dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        dispatcherQueue.ShutdownStarting += DispatcherQueue_ShutdownStarting;
        Services = ConfigureServices();
        InitializeComponent();
        Current.UnhandledException += Dispatcher_UnhandledException;
        DispatcherShutdownMode = Microsoft.UI.Xaml.DispatcherShutdownMode.OnLastWindowClose;
        AppInstance.GetCurrent().Activated += AppInstance_Activated;
    }

    private static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<ISettingsService, JsonFileService>();
        services.AddSingleton<IIconLoader, IconLoaderService>();
        // selector
        services.AddSingleton<IWebViewEnvironmentService, WebViewEnvironmentService>();
        services.AddSingleton<IQuickViewService, QuickViewService>();
        services.AddTransient<SelectorPageViewModel>();
        // settings
        services.AddTransient<SettingsPageViewModel>();
        services.AddTransient<BrowsersPageViewModel>();
        services.AddTransient<RulesetPageViewModel>();
        services.AddTransient<QuickViewPageViewModel>();
        services.AddTransient<StoreRulesetViewModel>();

        return services.BuildServiceProvider();
    }

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        isLaunched = true;
        HandleActivation(_pendingActivationArgs ?? AppInstance.GetCurrent().GetActivatedEventArgs(), false);
        _pendingActivationArgs = null;
    }

    private void AppInstance_Activated(object? sender, AppActivationArguments args)
    {
        if (!isLaunched)
        {
            _pendingActivationArgs = args;
            return;
        }

        _ = dispatcherQueue.TryEnqueue(() => HandleActivation(args, true));
    }

    private void HandleActivation(AppActivationArguments activationArgs, bool isSecondInstance)
    {
        var cliArgs = CliArgs.GatherInfo(activationArgs, isSecondInstance);
        IServiceProvider services = Services ?? throw new InvalidOperationException("Application services are not configured.");

        if (cliArgs.SettingsPage is string page)
        {
            ShowSettings(page);
            return;
        }

        if (services.GetRequiredService<IQuickViewService>().TryOpenIfModifierKeyActivated(cliArgs.Url))
        {
            return;
        }

        var settings = services.GetRequiredService<ISettingsService>().LoadSettings();
        if (cliArgs.Url is not null && RuleMatch.CheckRulesets(cliArgs.Url, settings.Rulesets) is Ruleset matchingRuleset)
        {
            var selectedBrowser = settings.Browsers.FirstOrDefault(b => b.Id == matchingRuleset.BrowserId);
            if (selectedBrowser is not null)
            {
                UriLauncher.ResolveAutomatically(cliArgs.Url, selectedBrowser, matchingRuleset.AlternateLaunchId);
            }
            return;
        }


        _selectorWindow ??= new SelectorWindow();
        trayService ??= new TrayService(ShowSelector, () => ShowSettings("settings"), ReloadApp, ExitApp);
        _selectorWindow.Init(cliArgs);
    }

    private static void ShowSelector()
    {
        _selectorWindow ??= new SelectorWindow();
        _selectorWindow.ShowWindow();
    }

    private void ReloadApp()
    {
        string appPath = Environment.ProcessPath ?? Path.Combine(AppContext.BaseDirectory, "Hurl.exe");
        Process.Start(new ProcessStartInfo(appPath)
        {
            UseShellExecute = true
        });
        ExitApp();
    }

    private void ExitApp()
    {
        IsExiting = true;
        trayService?.Dispose();
        Exit();
    }

    private void DispatcherQueue_ShutdownStarting(DispatcherQueue sender, DispatcherQueueShutdownStartingEventArgs args)
    {
        IsExiting = true;
        trayService?.Dispose();
        dispatcherQueue.ShutdownStarting -= DispatcherQueue_ShutdownStarting;
    }

    public static void ShowSettings(string page = "browsers")
    {
        _selectorWindow?.MinimizeWindow();
        if (_settingsWindow is null)
        {
            _settingsWindow = new SettingsWindow();
            _settingsWindow.Closed += (_, _) => _settingsWindow = null;
        }

        _settingsWindow.NavigateToPage(page);
        _settingsWindow.Restore();
        _settingsWindow.Activate();
        _settingsWindow.SetForegroundWindow();
    }

    private void Dispatcher_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        string ErrorMsgBuffer;
        string ErrorWndTitle;
        switch (e.Exception?.InnerException)
        {
            case JsonException:
                ErrorMsgBuffer = "The UserSettings.json file is in invalid JSON format. \n";
                ErrorWndTitle = "Hurl - Invalid JSON";
                break;
            default:
                ErrorMsgBuffer = "An unknown error has occurred. \n";
                ErrorWndTitle = "Hurl - Unknown Error";
                break;
        }

        // TODO: create the crashes directory if it doesn't exist
        long seconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var argsStoreFile = Path.Combine(Constants.ROAMING, "Hurl", "crashes", $"{seconds}.txt");
        var errorFileContents = string.Format("{0}\n\nStackTrace:\n{1}", e.Message, e.Exception.StackTrace);
        File.AppendAllText(argsStoreFile, errorFileContents);

        string errorMessage = string.Format("{0}\n{1}\n\n{2}", ErrorMsgBuffer, e.Exception?.InnerException?.Message, e.Exception?.Message);
        // ContentDialog dialog = new()
        // {
        //     Content = errorMessage,
        //     Title = ErrorWndTitle,
        // };
        // await dialog.ShowAsync();

        ExitApp();
    }
}
