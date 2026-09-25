# Quick View

Quick View is enabled by default. Hold **Alt** while opening an HTTP or HTTPS link through Hurl to open it in
the built-in Edge WebView2 window. You can also use the **Quick View** button beside the URL text box in the
selector. A successful Quick View shortcut takes precedence over rule matching.

Configure it in **Hurl Settings > Quick View**, or add the following top-level property to `UserSettings.json`:

```json
{
  "QuickView": {
    "Enabled": true,
    "LaunchMode": "WebView",
    "ModifierKeys": "Alt",
    "BrowserId": null,
    "AlternateLaunchId": null,

    "AdditionalBrowserArguments": "",
    "BrowserExtensionsEnabled": false,
    "TrackingPrevention": "Balanced"
  }
}
```

- `Enabled` controls both the shortcut and the selector's Quick View button.
- `LaunchMode` is `WebView` for the built-in preview or `Browser` to launch a configured browser directly. The selector's Quick View button uses this setting too.
- `ModifierKeys` supports `Alt`, `CtrlAlt`, or `Ctrl`.
- `BrowserId` selects a browser by its `Id` when `LaunchMode` is `Browser`. 
- `AlternateLaunchId` selects one of that browser's alternate launches, `null` uses its default launch.

### WebView2 Configuration
- `AdditionalBrowserArguments` and `BrowserExtensionsEnabled` configure the WebView2 environment.
  Restart Hurl after changing them if you have already opened a Quick View window.
- `TrackingPrevention` supports `None`, `Basic`, `Balanced` (default), or `Strict` for the WebView2 profile.
