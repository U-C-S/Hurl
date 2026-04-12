using Hurl.Selector.Helpers;
using Hurl.Selector.Pages;
using Hurl.Selector.Services;
using Hurl.Selector.Services.Interfaces;
using Hurl.Selector.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Windows.AppLifecycle;
using System;
using System.Text.Json;
using System.Windows;

namespace Hurl.Selector;

public partial class App : Microsoft.UI.Xaml.Application
{
    public static IServiceProvider? Services { get; private set; }

    private static SelectorWindow? _mainWindow;
    private AppActivationArguments? _pendingActivationArgs;

    public App()
    {
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
        services.AddTransient<SettingsViewModel>();
        services.AddTransient<SelectorPageViewModel>();

        return services.BuildServiceProvider();
    }

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        _mainWindow ??= new SelectorWindow();
        HandleActivation(_pendingActivationArgs ?? AppInstance.GetCurrent().GetActivatedEventArgs(), false);
        _pendingActivationArgs = null;
    }

    private void AppInstance_Activated(object? sender, AppActivationArguments args)
    {
        if (_mainWindow is null)
        {
            _pendingActivationArgs = args;
            return;
        }

        _ = _mainWindow.DispatcherQueue.TryEnqueue(() => HandleActivation(args, true));
    }

    private static void HandleActivation(AppActivationArguments activationArgs, bool isSecondInstance)
    {
        _mainWindow ??= new SelectorWindow();
        var cliArgs = CliArgs.GatherInfo(activationArgs, isSecondInstance);
        _mainWindow.Init(cliArgs);
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
        string errorMessage = string.Format("{0}\n{1}\n\n{2}", ErrorMsgBuffer, e.Exception?.InnerException?.Message, e.Exception?.Message);
        MessageBox.Show(errorMessage, ErrorWndTitle, MessageBoxButton.OK, MessageBoxImage.Error);

        Exit();
    }
}
