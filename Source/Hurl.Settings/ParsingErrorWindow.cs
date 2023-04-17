using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf.Ui.Common;
using MessageBox = Wpf.Ui.Controls.MessageBox;
using TextBox = Wpf.Ui.Controls.TextBox;

namespace Hurl.Settings;

/**
 * Source : https://github.com/BartoszCichecki/LenovoLegionToolkit/blob/2.8.1/LenovoLegionToolkit.WPF/Utils/MesageBoxHelper.cs
 */
public static class ParsingErrorWindow
{
    public static Task<string[]> ShowInputAsync(
        Window window,
        bool isEditRule,
        string[] text
    )
    {
        // give 3 options in case of parsing error
        // - edit the json manually
        // - generate a new file while backing up prev file with timestamp
        // - exit the settings window

        var tcs = new TaskCompletionSource<string[]>();

        var stack = new StackPanel();

        var textBox = new TextBox
        {
            MaxLines = 1,
            PlaceholderText = "Enter the URL you want to open",
            Text = text[0],
            SelectionStart = text?.Length ?? 0,
            SelectionLength = 0
        };
        stack.Children.Add(textBox);

        TextBox textBox2 = null;

        if (isEditRule)
        {
            textBox2 = new TextBox
            {
                MaxLines = 1,
                PlaceholderText = "Enter the URL Rule you want the selected browser to store",
                Text = text[1],
                SelectionStart = text?.Length ?? 0,
                SelectionLength = 0
            };
            stack.Children.Add(textBox2);
        }


        var messageBox = new MessageBox
        {
            Owner = window,
            Title = isEditRule ? "Edit Rule and URL to open" : "Edit URL to open",
            Content = stack,
            ButtonLeftAppearance = ControlAppearance.Transparent,
            ButtonLeftName = "OK",
            ButtonRightName = "Cancel",
            ShowInTaskbar = false,
            Topmost = false,
            MinHeight = 240,
            MaxHeight = 240,
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
            tcs.SetResult(new string[] { newText });
            messageBox.Close();
        };
        messageBox.ButtonRightClick += (s, e) =>
        {
            tcs.SetResult(new string[] { "" });
            messageBox.Close();
        };
        messageBox.Closing += (s, e) =>
        {
            tcs.TrySetResult(new string[] { " " });
        };
        messageBox.Show();

        FocusManager.SetFocusedElement(window, textBox);

        return tcs.Task;
    }
}
