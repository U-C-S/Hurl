using Hurl.Library;
using Hurl.App.Helpers;
using Hurl.App.Views;
using Hurl.App.Services;
using Hurl.App.Services.Interfaces;
using Hurl.App.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using Microsoft.Windows.AppLifecycle;
using System;
using System.IO;
using System.Text.Json;

namespace Hurl.App;

public partial class App : Microsoft.UI.Xaml.Application
{
    public static IServiceProvider? Services { get; private set; }

    private static SelectorWindow? _selectorWindow;
    private readonly DispatcherQueue dispatcherQueue;
    private AppActivationArguments? _pendingActivationArgs;
    private bool isLaunched;

    public App()
    {
        dispatcherQueue = DispatcherQueue.GetForCurrentThread();
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
        services.AddSingleton<IWebViewEnvironmentService, WebViewEnvironmentService>();
        services.AddSingleton<IQuickViewService, QuickViewService>();
        services.AddTransient<SelectorPageViewModel>();

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

    private static void HandleActivation(AppActivationArguments activationArgs, bool isSecondInstance)
    {
        var cliArgs = CliArgs.GatherInfo(activationArgs, isSecondInstance);
        IServiceProvider services = Services ?? throw new InvalidOperationException("Application services are not configured.");

        if (services.GetRequiredService<IQuickViewService>().TryOpenIfModifierKeyActivated(cliArgs.Url))
        {
            return;
        }

        _selectorWindow ??= new SelectorWindow();
        _selectorWindow.Init(cliArgs);
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

        Exit();
    }
}
