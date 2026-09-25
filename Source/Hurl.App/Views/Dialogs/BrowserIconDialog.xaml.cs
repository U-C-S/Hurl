using Hurl.App.Services.Interfaces;
using Hurl.App.ViewModels;
using Hurl.Library.Models;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Linq;
using Windows.Storage.Pickers;
using Windows.System;

namespace Hurl.App.Views.Dialogs;

public sealed partial class BrowserIconDialog : ContentDialog
{
    private bool isOpened;
    public BrowserIconDialogViewModel ViewModel { get; }

    public BrowserIconDialog(IIconLoader iconLoader, string exePath, BrowserIcon? icon)
    {
        ViewModel = new(iconLoader, exePath, icon);
        InitializeComponent();
        // Populate the enum tags before selecting a tab; x:Bind normally initializes later.
        Bindings.Update();
        SourceSelector.SelectedItem = SourceSelector.Items.First(item =>
            item.Tag is BrowserIconSource source && source == ViewModel.SelectedSource);
    }

    private async void Dialog_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args)
    {
        isOpened = true;
        await ViewModel.SelectSourceAsync(ViewModel.SelectedSource);
    }

    private void Dialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
    {
        isOpened = false;
        ViewModel.Close();
    }

    private async void SourceSelector_SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs args)
    {
        if (isOpened && sender.SelectedItem?.Tag is BrowserIconSource source)
            await ViewModel.SelectSourceAsync(source);
    }

    public Visibility SourceVisibility(BrowserIconSource selected, BrowserIconSource expected) =>
        selected == expected ? Visibility.Visible : Visibility.Collapsed;

    public Visibility ImageSourceVisibility(BrowserIconSource selected) =>
        selected != BrowserIconSource.Executable ? Visibility.Visible : Visibility.Collapsed;

    private async void SelectFile_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var picker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                ViewMode = PickerViewMode.Thumbnail
            };
            foreach (string extension in new[] { ".ico", ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".tif", ".tiff" })
                picker.FileTypeFilter.Add(extension);

            var hwnd = Win32Interop.GetWindowFromWindowId(XamlRoot.ContentIslandEnvironment.AppWindowId);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
            var file = await picker.PickSingleFileAsync();
            if (file is not null && isOpened)
                await ViewModel.LoadLocalImageAsync(file.Path);
        }
        catch (Exception)
        {
            if (isOpened) ViewModel.ErrorMessage = "Couldn't open the file picker. Please try again.";
        }
    }

    private async void LoadUrl_Click(object sender, RoutedEventArgs e) => await ViewModel.LoadUrlAsync();

    private async void UrlInput_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.Enter)
        {
            e.Handled = true;
            await ViewModel.LoadUrlAsync();
        }
    }
}
