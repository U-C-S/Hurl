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
    async public static Task<string> ShowInputAsync(
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
            PrimaryButtonAppearance = ControlAppearance.Primary,
            PrimaryButtonText = "OK",
            ShowInTaskbar = false,
            Topmost = false,
            SizeToContent = SizeToContent.Height
        };

        textBox.TextChanged += (s, e) =>
        {
            var isEmpty = string.IsNullOrWhiteSpace(textBox.Text);
            messageBox.IsPrimaryButtonEnabled = !isEmpty;
        };

        //messageBox.ButtonLeftClick += (s, e) =>
        //{
        //    var content = textBox.Text?.Trim();
        //    var newText = string.IsNullOrWhiteSpace(content) ? null : content;
        //    if (newText is null)
        //        return;
        //    tcs.SetResult(newText);
        //    messageBox.Close();
        //};
        //messageBox.ButtonRightClick += (s, e) =>
        //{
        //    tcs.SetResult(null);
        //    messageBox.Close();
        //};
        messageBox.Closing += (s, e) =>
        {
            tcs.TrySetResult(null);
        };

        var result = await messageBox.ShowDialogAsync();

        FocusManager.SetFocusedElement(window, textBox);

        if (result == Wpf.Ui.Controls.MessageBoxResult.Primary)
        {
            var content = textBox.Text?.Trim();
            var newText = string.IsNullOrWhiteSpace(content) ? null : content;
            if (newText is null)
                return text;
            else return newText;
            //messageBox.Close();
        }
        else return text;
    }
}
