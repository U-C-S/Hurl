using Hurl.Library;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;

namespace Hurl.Settings.Controls;

public sealed partial class ComingSoon : UserControl
{
    public ComingSoon()
    {
        this.InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Process.Start("explorer", "\"" + Constants.APP_SETTINGS_MAIN + "\"");
    }
}
