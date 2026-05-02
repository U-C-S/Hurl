using Hurl.Library.Models;
using Hurl.Selector.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Windows.Input;

namespace Hurl.Selector.Controls;

public sealed partial class BrowserBarButton : UserControl
{
    private readonly BrowserTileInteractionState interactionState = new();
    private readonly bool isInitialized;

    #region Lifecycle
    public BrowserBarButton()
    {
        InitializeComponent();
        isInitialized = true;
    }

    private static void OnBrowserItemChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
    {
        if (dependencyObject is BrowserBarButton button)
        {
            button.RefreshBrowserItem();
        }
    }

    private static void OnLaunchCommandChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
    {
        if (dependencyObject is BrowserBarButton button && button.isInitialized)
        {
            button.Bindings.Update();
        }
    }

    private void RefreshBrowserItem()
    {
        if (!isInitialized)
        {
            return;
        }

        Bindings.Update();
        ConfigureAlternateLaunchFlyouts();
    }
    #endregion

    #region Public API
    public BrowserItemViewModel? BrowserItem
    {
        get => (BrowserItemViewModel?)GetValue(BrowserItemProperty);
        set => SetValue(BrowserItemProperty, value);
    }

    public static readonly DependencyProperty BrowserItemProperty =
        DependencyProperty.Register(
            nameof(BrowserItem),
            typeof(BrowserItemViewModel),
            typeof(BrowserBarButton),
            new PropertyMetadata(null, OnBrowserItemChanged));

    public ICommand? LaunchCommand
    {
        get => (ICommand?)GetValue(LaunchCommandProperty);
        set => SetValue(LaunchCommandProperty, value);
    }

    public static readonly DependencyProperty LaunchCommandProperty =
        DependencyProperty.Register(
            nameof(LaunchCommand),
            typeof(ICommand),
            typeof(BrowserBarButton),
            new PropertyMetadata(null, OnLaunchCommandChanged));

    public event EventHandler<AlternateLaunchRequestedEventArgs>? AlternateLaunchRequested;
    #endregion

    #region AltLaunch Flyout
    public Visibility AlternateLaunchVisibility => BrowserItem?.AlternateLaunches is { Count: > 0 }
        ? Visibility.Visible
        : Visibility.Collapsed;

    private void ConfigureAlternateLaunchFlyouts()
    {
        var contextFlyout = BrowserItem is null ? null : CreateAlternateLaunchFlyout(BrowserItem.Model);
        var additionalFlyout = BrowserItem is null ? null : CreateAlternateLaunchFlyout(BrowserItem.Model);

        LaunchButton.ContextFlyout = contextFlyout;
        AdditionalBtn.Flyout = additionalFlyout;
        FlyoutBase.SetAttachedFlyout(AdditionalBtn, additionalFlyout);
    }

    private MenuFlyout? CreateAlternateLaunchFlyout(Browser browser)
    {
        if (browser.AlternateLaunches is not { Count: > 0 } alternateLaunches)
        {
            return null;
        }

        MenuFlyout flyout = new();
        flyout.Opening += (_, _) => SetFlyoutActive(true);
        flyout.Closed += (_, _) => SetFlyoutActive(false);

        foreach (var alternateLaunch in alternateLaunches)
        {
            MenuFlyoutItem item = new()
            {
                Text = alternateLaunch.ItemName,
                Tag = new AlternateLaunchContext(browser, alternateLaunch)
            };
            item.Click += AlternateLaunch_Click;
            flyout.Items.Add(item);
        }

        return flyout;
    }

    private void AdditionalBtn_Click(object sender, RoutedEventArgs e)
    {
        SetFlyoutActive(true);
        FlyoutBase.ShowAttachedFlyout(AdditionalBtn);
    }

    private void AlternateLaunch_Click(object sender, RoutedEventArgs e)
    {
        if (sender is MenuFlyoutItem { Tag: AlternateLaunchContext context })
        {
            AlternateLaunchRequested?.Invoke(
                this,
                new AlternateLaunchRequestedEventArgs(context.Browser, context.AlternateLaunch));
        }
    }

    private void SetFlyoutActive(bool isOpen)
    {
        interactionState.IsFlyoutOpen = isOpen;
        UpdateInteractionState();
    }
    #endregion

    #region Internal UI Events
    private void BrowserTile_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        interactionState.IsPointerOver = true;
        UpdateInteractionState();
    }

    private void BrowserTile_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        interactionState.IsPointerOver = false;
        UpdateInteractionState();
    }

    private void BrowserTile_GotFocus(object sender, RoutedEventArgs e)
    {
        interactionState.IsKeyboardFocusWithin = true;
        UpdateInteractionState();
    }

    private void BrowserTile_LostFocus(object sender, RoutedEventArgs e)
    {
        DispatcherQueue.TryEnqueue(() =>
        {
            var focusedElement = XamlRoot is null ? null : FocusManager.GetFocusedElement(XamlRoot);
            interactionState.IsKeyboardFocusWithin = focusedElement is DependencyObject focusedObject
                && IsDescendantOf(focusedObject, BrowserTileRoot);
            UpdateInteractionState();
        });
    }

    private void UpdateInteractionState()
    {
        bool isActive = interactionState.IsPointerOver
            || interactionState.IsKeyboardFocusWithin
            || interactionState.IsFlyoutOpen;

        if (interactionState.IsActive == isActive)
        {
            return;
        }

        interactionState.IsActive = isActive;

        AnimateDouble(BrowserTileBackground, nameof(Opacity), isActive ? 1 : 0, 160);

        if (BrowserTileRoot.RenderTransform is CompositeTransform tileTransform)
        {
            AnimateDouble(tileTransform, nameof(CompositeTransform.ScaleX), isActive ? 1 : 0.985, 180);
            AnimateDouble(tileTransform, nameof(CompositeTransform.ScaleY), isActive ? 1 : 0.985, 180);
        }

        AdditionalBtn.IsHitTestVisible = isActive;
        AdditionalBtn.IsTabStop = isActive;
        AnimateDouble(AdditionalBtn, nameof(Opacity), isActive ? 1 : 0, 140);

        if (AdditionalBtn.RenderTransform is CompositeTransform transform)
        {
            AnimateDouble(transform, nameof(CompositeTransform.TranslateY), isActive ? 0 : -4, 180);
            AnimateDouble(transform, nameof(CompositeTransform.ScaleX), isActive ? 1 : 0.88, 180);
            AnimateDouble(transform, nameof(CompositeTransform.ScaleY), isActive ? 1 : 0.88, 180);
        }
    }

    private static void AnimateDouble(DependencyObject target, string property, double to, double milliseconds)
    {
        DoubleAnimation animation = new()
        {
            To = to,
            Duration = TimeSpan.FromMilliseconds(milliseconds),
            EnableDependentAnimation = true,
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };

        Storyboard.SetTarget(animation, target);
        Storyboard.SetTargetProperty(animation, property);

        Storyboard storyboard = new();
        storyboard.Children.Add(animation);
        storyboard.Begin();
    }
    #endregion

    #region Helpers
    private static bool IsDescendantOf(DependencyObject source, DependencyObject ancestor)
    {
        DependencyObject? current = source;
        while (current is not null)
        {
            if (current == ancestor)
            {
                return true;
            }

            current = VisualTreeHelper.GetParent(current);
        }

        return false;
    }
    #endregion

    #region Nested Types
    private sealed record AlternateLaunchContext(Browser Browser, AlternateLaunch AlternateLaunch);

    private sealed class BrowserTileInteractionState
    {
        public bool IsPointerOver { get; set; }

        public bool IsKeyboardFocusWithin { get; set; }

        public bool IsFlyoutOpen { get; set; }

        public bool IsActive { get; set; }
    }
    #endregion
}

public sealed class AlternateLaunchRequestedEventArgs(Browser browser, AlternateLaunch alternateLaunch) : EventArgs
{
    public Browser Browser { get; } = browser;

    public AlternateLaunch AlternateLaunch { get; } = alternateLaunch;
}
