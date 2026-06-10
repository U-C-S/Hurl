using Hurl.App.ViewModels;
using Hurl.Library.Models;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Numerics;
using System.Windows.Input;

namespace Hurl.App.Controls;

public sealed partial class BrowserBarButton : UserControl
{
    private const float InactiveTileScale = 0.985f;
    private const float InactiveAdditionalScale = 0.88f;
    private const float InactiveAdditionalOffsetY = -4f;
    private static readonly TimeSpan BackgroundAnimationDuration = TimeSpan.FromMilliseconds(160);
    private static readonly TimeSpan TileAnimationDuration = TimeSpan.FromMilliseconds(180);
    private static readonly TimeSpan AdditionalAnimationDuration = TimeSpan.FromMilliseconds(140);

    private readonly BrowserTileInteractionState interactionState = new();
    private readonly bool isInitialized;

    #region Lifecycle
    public BrowserBarButton()
    {
        InitializeComponent();
        InitializeInteractionVisualState();
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

        AdditionalBtn.IsHitTestVisible = isActive;
        AdditionalBtn.IsTabStop = isActive;
        AnimateInteractionVisualState(isActive);
    }

    private void InitializeInteractionVisualState()
    {
        BrowserTileRoot.SizeChanged += (_, _) => UpdateVisualCenterPoints();
        AdditionalBtn.SizeChanged += (_, _) => UpdateVisualCenterPoints();
        SetInteractionVisualState(isActive: false);
    }

    private void AnimateInteractionVisualState(bool isActive)
    {
        var backgroundVisual = ElementCompositionPreview.GetElementVisual(BrowserTileBackground);
        var tileVisual = ElementCompositionPreview.GetElementVisual(BrowserTileRoot);
        var additionalVisual = ElementCompositionPreview.GetElementVisual(AdditionalBtn);

        StartScalarAnimation(backgroundVisual, "Opacity", isActive ? 1f : 0f, BackgroundAnimationDuration);
        StartVector3Animation(
            tileVisual,
            "Scale",
            CreateUniformScale(isActive ? 1f : InactiveTileScale),
            TileAnimationDuration);
        StartScalarAnimation(additionalVisual, "Opacity", isActive ? 1f : 0f, AdditionalAnimationDuration);
        StartVector3Animation(
            additionalVisual,
            "Scale",
            CreateUniformScale(isActive ? 1f : InactiveAdditionalScale),
            TileAnimationDuration);
        StartVector3Animation(
            additionalVisual,
            "Offset",
            new Vector3(0, isActive ? 0 : InactiveAdditionalOffsetY, 0),
            TileAnimationDuration);
    }

    private void SetInteractionVisualState(bool isActive)
    {
        var backgroundVisual = ElementCompositionPreview.GetElementVisual(BrowserTileBackground);
        var tileVisual = ElementCompositionPreview.GetElementVisual(BrowserTileRoot);
        var additionalVisual = ElementCompositionPreview.GetElementVisual(AdditionalBtn);

        backgroundVisual.Opacity = isActive ? 1f : 0f;
        tileVisual.Scale = CreateUniformScale(isActive ? 1f : InactiveTileScale);
        additionalVisual.Opacity = isActive ? 1f : 0f;
        additionalVisual.Scale = CreateUniformScale(isActive ? 1f : InactiveAdditionalScale);
        additionalVisual.Offset = new Vector3(0, isActive ? 0 : InactiveAdditionalOffsetY, 0);
        UpdateVisualCenterPoints();
    }

    private void UpdateVisualCenterPoints()
    {
        ElementCompositionPreview.GetElementVisual(BrowserTileRoot).CenterPoint = new Vector3(
            (float)BrowserTileRoot.ActualWidth / 2,
            (float)BrowserTileRoot.ActualHeight / 2,
            0);

        ElementCompositionPreview.GetElementVisual(AdditionalBtn).CenterPoint = new Vector3(
            (float)AdditionalBtn.ActualWidth / 2,
            (float)AdditionalBtn.ActualHeight / 2,
            0);
    }

    private static void StartScalarAnimation(Visual visual, string property, float to, TimeSpan duration)
    {
        var animation = visual.Compositor.CreateScalarKeyFrameAnimation();
        animation.InsertKeyFrame(1f, to, CreateEaseOut(visual));
        animation.Duration = duration;
        visual.StartAnimation(property, animation);
    }

    private static void StartVector3Animation(Visual visual, string property, Vector3 to, TimeSpan duration)
    {
        var animation = visual.Compositor.CreateVector3KeyFrameAnimation();
        animation.InsertKeyFrame(1f, to, CreateEaseOut(visual));
        animation.Duration = duration;
        visual.StartAnimation(property, animation);
    }

    private static CompositionEasingFunction CreateEaseOut(Visual visual)
    {
        return visual.Compositor.CreateCubicBezierEasingFunction(
            new Vector2(0.16f, 1f),
            new Vector2(0.3f, 1f));
    }

    private static Vector3 CreateUniformScale(float scale)
    {
        return new Vector3(scale, scale, 1);
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
