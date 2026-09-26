using Hurl.App.Helpers;
using Microsoft.Windows.AppLifecycle;
using System;
using System.Diagnostics;

namespace Hurl.App;

public class Program
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler", " 3.0.0.2408")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.STAThreadAttribute]
    static void Main(string[] args)
    {
        NativeMethods.XamlCheckProcessRequirements();
        global::WinRT.ComWrappersSupport.InitializeComWrappers();

        bool isRedirect = DecideRedirection();

        if (!isRedirect)
        {
            global::Microsoft.UI.Xaml.Application.Start((p) =>
            {
                var context = new global::Microsoft.UI.Dispatching.DispatcherQueueSynchronizationContext(global::Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread());
                global::System.Threading.SynchronizationContext.SetSynchronizationContext(context);
                var x = new App();
            });
        }
    }

    private static bool DecideRedirection()
    {
        bool isRedirect = false;
        AppActivationArguments args = AppInstance.GetCurrent().GetActivatedEventArgs();
        AppInstance keyInstance = AppInstance.FindOrRegisterForKey("Hurl_3721");

        if (!keyInstance.IsCurrent)
        {
            isRedirect = true;
            RedirectActivationTo(args, keyInstance);
        }

        return isRedirect;
    }

    public static void RedirectActivationTo(AppActivationArguments args,
                                            AppInstance keyInstance)
    {
        keyInstance.RedirectActivationToAsync(args).AsTask().Wait();

        Process process = Process.GetProcessById((int)keyInstance.ProcessId);
        NativeMethods.SetForegroundWindow(process.MainWindowHandle);
    }
}
