using Microsoft.Windows.ApplicationModel.DynamicDependency;
using Microsoft.Windows.AppLifecycle;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Hurl.Selector;

public class Program
{
    [global::System.Runtime.InteropServices.DllImport("Microsoft.ui.xaml.dll")]
    [global::System.Runtime.InteropServices.DefaultDllImportSearchPaths(global::System.Runtime.InteropServices.DllImportSearchPath.SafeDirectories)]
    private static extern void XamlCheckProcessRequirements();

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
                new App();
            });
        }

        //return 0;


    }

    public static bool InitializeWASDK()
    {
        uint minSupportedMinorVersion = global::Microsoft.WindowsAppSDK.Release.MajorMinor; // 0x00010005
        uint maxSupportedMinorVersion = 0x00010007;
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
        ExtendedActivationKind kind = args.Kind;
        AppInstance keyInstance = AppInstance.FindOrRegisterForKey("Hurl_3721");

        if (keyInstance.IsCurrent)
        {
            keyInstance.Activated += OnActivated;
        }
        else
        {
            isRedirect = true;
            RedirectActivationTo(args, keyInstance);
        }

        return isRedirect;
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr CreateEvent(
    IntPtr lpEventAttributes, bool bManualReset,
    bool bInitialState, string lpName);

    [DllImport("kernel32.dll")]
    private static extern bool SetEvent(IntPtr hEvent);

    [DllImport("ole32.dll")]
    private static extern uint CoWaitForMultipleObjects(
        uint dwFlags, uint dwMilliseconds, ulong nHandles,
        IntPtr[] pHandles, out uint dwIndex);

    [DllImport("user32.dll")]
    static extern bool SetForegroundWindow(IntPtr hWnd);

    private static IntPtr redirectEventHandle = IntPtr.Zero;

    // Do the redirection on another thread, and use a non-blocking
    // wait method to wait for the redirection to complete.
    public static void RedirectActivationTo(AppActivationArguments args,
                                            AppInstance keyInstance)
    {
        redirectEventHandle = CreateEvent(IntPtr.Zero, true, false, null);
        Task.Run(() =>
        {
            keyInstance.RedirectActivationToAsync(args).AsTask().Wait();
            SetEvent(redirectEventHandle);
        });

        uint CWMO_DEFAULT = 0;
        uint INFINITE = 0xFFFFFFFF;
        _ = CoWaitForMultipleObjects(
           CWMO_DEFAULT, INFINITE, 1,
           [redirectEventHandle], out uint handleIndex);

        // Bring the window to the foreground
        Process process = Process.GetProcessById((int)keyInstance.ProcessId);
        SetForegroundWindow(process.MainWindowHandle);
    }

    private static void OnActivated(object sender, AppActivationArguments args)
    {
        ExtendedActivationKind kind = args.Kind;
    }
}
