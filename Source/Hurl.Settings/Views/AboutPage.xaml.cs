using Hurl.Library;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

namespace Hurl.Settings.Views;

public sealed partial class AboutPage : Page
{
    public AboutPage()
    {
        this.InitializeComponent();
    }

    public string Version => Constants.VERSION;

    public string SourceCodeLink => Constants.SOURCE_CODE_LINK;

    public List<OSSLibrary> OpenSourceLibraries => [
        new ("https://github.com/CommunityToolkit/dotnet", "CommunityToolkit.Mvvm"),
        new ("https://github.com/CommunityToolkit/Windows","CommunityToolkit.WinUI.Controls.SettingsControls"),
        new ("https://github.com/dazinator/DotNet.Glob","DotNet.Glob"),
        new ("https://github.com/dotnet/runtime","Microsoft.Win32.Registry"),
        new ("https://github.com/microsoft/cswinrt","Microsoft.Windows.CsWinRT"),
        new ("https://github.com/dotnet/winforms","System.Drawing.Common"),
        new ("https://github.com/microsoft/WindowsAppSDK", "WindowsAppSDK"),
        new ("https://github.com/microsoft/microsoft-ui-xaml", "WinUI 3"),
        new ("https://github.com/lepoco/wpfui","WPF-UI"),
        new ("https://github.com/lepoco/wpfui","WPF-UI.Tray"),
        ];
}

public class OSSLibrary(string Url, string Name)
{
    public string Name { get; set; } = Name;

    public string Url { get; set; } = Url;
}