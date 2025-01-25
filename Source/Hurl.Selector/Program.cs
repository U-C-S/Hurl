using Microsoft.Windows.ApplicationModel.DynamicDependency;
using System;

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
        global::Microsoft.UI.Xaml.Application.Start((p) =>
        {
            var context = new global::Microsoft.UI.Dispatching.DispatcherQueueSynchronizationContext(global::Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread());
            global::System.Threading.SynchronizationContext.SetSynchronizationContext(context);
            new App();
        });
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
}
