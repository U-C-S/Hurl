using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using System.IO;
using WinUIEx;

namespace Hurl.App.Services;

public sealed class TrayService : IDisposable
{
    private const uint TrayIconId = 3721;
    private readonly TrayIcon trayIcon;
    private readonly MenuFlyout trayMenuFlyout;
    private readonly Action showSelector;
    private readonly Action showSettings;
    private readonly Action reload;
    private readonly Action exit;
    private bool disposed;

    public TrayService(Action showSelector, Action showSettings, Action reload, Action exit)
    {
        this.showSelector = showSelector;
        this.showSettings = showSettings;
        this.reload = reload;
        this.exit = exit;

        trayMenuFlyout = new MenuFlyout();
        trayMenuFlyout.Items.Add(CreateMenuItem("Settings", "settings", "\uE713"));
        trayMenuFlyout.Items.Add(CreateMenuItem("Reload", "reload", "\uE777"));
        trayMenuFlyout.Items.Add(CreateMenuItem("Exit", "exit", "\uE8BB"));

        string iconPath = Path.Combine(AppContext.BaseDirectory, "Assets", "internet.ico");
        trayIcon = new TrayIcon(TrayIconId, iconPath, "Hurl is running in background for faster access");
        trayIcon.Selected += TrayIcon_Selected;
        trayIcon.LeftDoubleClick += TrayIcon_Selected;
        trayIcon.ContextMenu += TrayIcon_ContextMenu;
        trayIcon.IsVisible = true;
    }

    private MenuFlyoutItem CreateMenuItem(string text, string tag, string glyph)
    {
        MenuFlyoutItem item = new()
        {
            Text = text,
            Tag = tag,
            Icon = new FontIcon { Glyph = glyph }
        };
        item.Click += MenuItem_Click;
        return item;
    }

    private void TrayIcon_Selected(object? sender, TrayIconEventArgs e)
    {
        e.Handled = true;
        showSelector();
    }

    private void TrayIcon_ContextMenu(object? sender, TrayIconEventArgs e)
    {
        e.Handled = true;
        e.Flyout = trayMenuFlyout;
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            switch ((sender as MenuFlyoutItem)?.Tag as string)
            {
                case "settings":
                    showSettings();
                    break;
                case "reload":
                    reload();
                    break;
                case "exit":
                    exit();
                    break;
            }
        }
        catch (Exception err)
        {
            Debug.WriteLine(err);
        }
    }

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;
        trayIcon.CloseFlyout();
        trayIcon.IsVisible = false;
        trayIcon.Selected -= TrayIcon_Selected;
        trayIcon.LeftDoubleClick -= TrayIcon_Selected;
        trayIcon.ContextMenu -= TrayIcon_ContextMenu;
        foreach (MenuFlyoutItem item in trayMenuFlyout.Items)
        {
            item.Click -= MenuItem_Click;
        }

        trayIcon.Dispose();
    }
}
