using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui.Controls;
using MessageBox = Wpf.Ui.Controls.MessageBox;
using TextBox = Wpf.Ui.Controls.TextBox;

namespace Hurl.BrowserSelector.Windows;

/**
 * Source : https://github.com/BartoszCichecki/LenovoLegionToolkit/blob/2.8.1/LenovoLegionToolkit.WPF/Utils/MesageBoxHelper.cs
 */
public static class URLEditor
{
    async public static Task<string> ShowInputAsync(Window window, string text)
    {
        var textBox = new TextBox
        {
            MaxLines = 1,
            PlaceholderText = "Enter the URL you want to open",
            Text = text,
        };

        var messageBox = new MessageBox
        {
            Owner = window,
            Title = "Edit URL to open",
            Content = textBox,
            PrimaryButtonAppearance = ControlAppearance.Primary,
            PrimaryButtonText = "OK",
            ShowInTaskbar = false,
            Topmost = false,
            SizeToContent = SizeToContent.Height
        };

        textBox.Focus();
        var result = await messageBox.ShowDialogAsync();

        if (result == Wpf.Ui.Controls.MessageBoxResult.Primary)
        {
            var content = textBox.Text?.Trim();
            return content ?? string.Empty;
        }
        else return text;
    }
}
