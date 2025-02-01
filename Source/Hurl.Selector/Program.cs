using Microsoft.Windows.ApplicationModel.DynamicDependency;
using Microsoft.Windows.AppLifecycle;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Hurl.Selector;

public partial class Program
{
    [LibraryImport("Microsoft.ui.xaml.dll")]
    [DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
    private static partial void XamlCheckProcessRequirements();

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler", " 3.0.0.2408")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.STAThreadAttribute]
    static void Main(string[] args)
    {
        InitializeWASDK();
        XamlCheckProcessRequirements();
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

    public static bool InitializeWASDK()
    {
        uint minSupportedMinorVersion = global::Microsoft.WindowsAppSDK.Release.MajorMinor; // 0x00010005
        uint maxSupportedMinorVersion = 0x00010008;
        for (uint version = minSupportedMinorVersion; version <= maxSupportedMinorVersion; version++)
        {
            if (global::Microsoft.Windows.ApplicationModel.DynamicDependency.Bootstrap.TryInitialize(version, out _))
                return true;
        }

        // Failure in trying to initialize supported versions.
        // Try one last time in the WINDOWSAPPSDK_BOOTSTRAP_AUTO_INITIALIZE way
        // that <WindowsPackageType>None</WindowsPackageType> property does.
        try
        {
            string versionTag = global::Microsoft.WindowsAppSDK.Release.VersionTag;
            var minVersion = new PackageVersion(global::Microsoft.WindowsAppSDK.Runtime.Version.UInt64);
            var options = global::Microsoft.Windows.ApplicationModel.DynamicDependency.Bootstrap.InitializeOptions.OnNoMatch_ShowUI;
            int hr = 0;
            if (!global::Microsoft.Windows.ApplicationModel.DynamicDependency.Bootstrap.TryInitialize(minSupportedMinorVersion, versionTag, minVersion, options, out hr))
            {
                Environment.Exit(hr);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error in initializing the default version of Windows App Runtime", ex);
        }

        return false;
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

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetForegroundWindow(IntPtr hWnd);

    public static void RedirectActivationTo(AppActivationArguments args,
                                            AppInstance keyInstance)
    {
        keyInstance.RedirectActivationToAsync(args).AsTask().Wait();

        Process process = Process.GetProcessById((int)keyInstance.ProcessId);
        SetForegroundWindow(process.MainWindowHandle);
    }
}
