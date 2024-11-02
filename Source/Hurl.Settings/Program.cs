using Windows.ApplicationModel;
using Windows.Management.Deployment;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Hurl.Settings;

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
        uint majorMinorVersion = global::Microsoft.WindowsAppSDK.Release.MajorMinor;
        int hr = 0;
        if (!global::Microsoft.Windows.ApplicationModel.DynamicDependency.Bootstrap.TryInitialize(majorMinorVersion, out hr))
        {
            global::System.Environment.Exit(hr);
        }
        XamlCheckProcessRequirements();

        global::WinRT.ComWrappersSupport.InitializeComWrappers();
        global::Microsoft.UI.Xaml.Application.Start((p) =>
        {
            var context = new global::Microsoft.UI.Dispatching.DispatcherQueueSynchronizationContext(global::Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread());
            global::System.Threading.SynchronizationContext.SetSynchronizationContext(context);
            new App();
        });
    }

    public static bool IsPackageRegisteredForCurrentUser(string packageFamilyName, PackageVersion minVersion, Windows.System.ProcessorArchitecture architecture, PackageTypes packageType)
    {
        ulong minPackageVersion = ToVersion(minVersion);
        var pkgMgr = new PackageManager();
        var x = pkgMgr.FindPackagesForUserWithPackageTypes(string.Empty, packageFamilyName, packageType);

        foreach (var p in x)
        {
            // Is the package architecture compatible?
            if (p.Id.Architecture != architecture)
            {
                continue;
            }

            // Is the package version sufficient for our needs?
            ulong packageVersion = ToVersion(p.Id.Version);
            if (packageVersion < minPackageVersion)
            {
                continue;
            }

            // Success!
            return true;
        }

        // No qualifying package found
        return false;
    }

    private static ulong ToVersion(PackageVersion packageVersion)
    {
        return ((ulong)packageVersion.Major << 48) |
               ((ulong)packageVersion.Minor << 32) |
               ((ulong)packageVersion.Build << 16) |
               ((ulong)packageVersion.Revision);
    }
}
