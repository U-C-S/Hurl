using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Controls;
using MessageBox = Wpf.Ui.Controls.MessageBox;
using TextBox = Wpf.Ui.Controls.TextBox;

namespace Hurl.BrowserSelector.Windows;

/**
 * Source : https://github.com/BartoszCichecki/LenovoLegionToolkit/blob/2.8.1/LenovoLegionToolkit.WPF/Utils/MesageBoxHelper.cs
 */
public static class URLEditor
{
    public static Task<string> ShowInputAsync(
        Window window,
        string text
    )
    {
        var tcs = new TaskCompletionSource<string>();

        var textBox = new TextBox
        {
            MaxLines = 1,
            PlaceholderText = "Enter the URL you want to open",
            Text = text,
            SelectionStart = text?.Length ?? 0,
            SelectionLength = 0
        };

        var messageBox = new MessageBox
        {
            Owner = window,
            Title = "Edit URL to open",
            Content = textBox,
            ButtonLeftAppearance = ControlAppearance.Transparent,
            ButtonLeftName = "OK",
            ButtonRightName = "Cancel",
            ShowInTaskbar = false,
            Topmost = false,
            SizeToContent = SizeToContent.Height,
            ResizeMode = ResizeMode.NoResize,
        };

        textBox.TextChanged += (s, e) =>
        {
            var isEmpty = string.IsNullOrWhiteSpace(textBox.Text);
            messageBox.ButtonLeftAppearance = isEmpty ? ControlAppearance.Transparent : ControlAppearance.Primary;
        };
        messageBox.ButtonLeftClick += (s, e) =>
        {
            var content = textBox.Text?.Trim();
            var newText = string.IsNullOrWhiteSpace(content) ? null : content;
            if (newText is null)
                return;
            tcs.SetResult(newText);
            messageBox.Close();
        };
        messageBox.ButtonRightClick += (s, e) =>
        {
            tcs.SetResult(null);
            messageBox.Close();
        };
        messageBox.Closing += (s, e) =>
        {
            tcs.TrySetResult(null);
        };
        messageBox.ShowDialog();

        FocusManager.SetFocusedElement(window, textBox);

        return tcs.Task;
    }
}
