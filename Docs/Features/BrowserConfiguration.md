# Browsers

Manage browsers in **Hurl Settings > Browsers**, or edit the top-level `Browsers` array in [UserSettings.json](../README.md).
The settings UI supports creating, editing, deleting, and dragging browser entries to reorder them.

- `Id` - Stable UUID for this browser. Various other settings refer to this browser by this Id. Required.
- `Name` - Display name for browser. Required.
- `ExePath` - Path to the browser's main executable, or its package family name when `IsUwp` is **true**. Required.
- `Icon` - Optional icon configuration containing `Source`, `Path`, and `Index`. Omit it or use `null` to use the executable's default icon. See below.
- `LaunchArgs` - Default executable launch arguments. Use `%URL%` to insert the URL at a specific position. If `%URL%` is absent, the URL is placed before the arguments. Optional.
- `Hidden` - Set to **true** to hide the browser from the selector and Quick View's browser targets. Rules can still launch it. Defaults to **false**.
- `AlternateLaunches` - This is an array; See below. Optional.
- `IsUwp` - Set to **true** to launch a packaged browser using its package family name. Defaults to **false**. Configure this property in JSON; the browser editor does not currently expose it.

## Browser icons

Click the icon preview beside **Name** and **Executable Path** to open **Select Browser Icon**:

- **Exe Icons** shows the icons embedded in the executable, labeled with their zero-based index. Selecting `icon 0` restores the default.
- **Local Image** opens a local file picker for ICO, PNG, JPEG, BMP, GIF, or TIFF images. The source file should be available all times.
- **From URL** loads a direct HTTP(S) image URL. Press Enter or the arrow button to preview it.

Hurl falls back to the executable's default icon if the override cannot be loaded.

### Caching

All images are cached in the `%APPDATA%/Roaming/Hurl/cache/icons`. As of now, the cache is not automatically
cleared.

The cache will be update automatically if the original file changes versions or the config changes.

### Or in the UserSettings.json

| Source      | Example                                                                |
| ----------- | ---------------------------------------------------------------------- |
| Executable  | `"Icon": { "Source": "Executable", "Index": 2 }`                       |
| Local image | `"Icon": { "Source": "LocalImage", "Path": "C:\\Icons\\browser.png" }` |
| URL         | `"Icon": { "Source": "Url", "Path": "https://example.com/icon.png" }`  |

## Launch Profiles (AlternateLaunches)

This is a way to launch the browser when you have multiple launch methods or launch targets, like incognito, browser profiles...

Suppose you have multiple chrome profiles like this:

![Example of Chrome profile in .lnk shortcut](../Images/ChromeProfiles.png)

Then you might want to use this feature, instead of totally adding a new browser entity for each profile in
the settings file. The following snippet demonstrates this feature.
Adding the `AlternateLaunches` field to the browser entry lets you right-click its icon or use its dropdown
button in the selector to choose an alternate launch. Add this property to the browser object:

```json
{
  "AlternateLaunches": [
    {
      "Id": "f81a1698-1488-461d-9294-4f4f8313178e",
      "ItemName": "Main Profile",
      "LaunchArgs": "--profile-directory=\"Default\""
    },
    {
      "Id": "e9e5df5a-1bc6-4bce-8f8d-e66950daa495",
      "ItemName": "Profile 2",
      "LaunchArgs": "--profile-directory=\"Profile 1\""
    },
    {
      "Id": "4bd80686-0a0b-4668-8ed0-063989e99c14",
      "ItemName": "Incognito",
      "LaunchArgs": "-incognito"
    }
  ]
}
```

Selecting an alternate launch includes the URL automatically. Its arguments replace the browser's default `LaunchArgs`; they are not combined.

![Alternate-launch menu in an older Hurl version](../Images/BrowserProfiles.png)

- `ItemName` - The name that shows up in the context menu for this launch
- `LaunchArgs` - Arguments for this alternate launch, such as an incognito flag or profile directory. Supports `%URL%` with the same behavior as the browser's default arguments.
- `Id` - Stable UUID for this alternate launch. Hurl generates one when absent. Preserve it when rulesets or Quick View target this profile.

## UWP Browsers

Packaged browsers can be launched through Windows using `IsUwp: true`. In this mode, `ExePath` contains the **package family name**, not an executable path or an application ID.

Find the package family name for an installed browser in PowerShell:

```powershell
Get-AppxPackage | Select-Object Name, PackageFamilyName
```

For example, a `Browsers` array containing packaged Firefox and Arc entries looks like this. Use the package family names reported on your machine:

```json
{
  "Browsers": [
    {
      "Id": "2b36a1fe-97f7-4509-ae6e-5c2c61602af4",
      "Name": "Firefox",
      "ExePath": "Mozilla.Firefox_n80bbvh6b1yt2",
      "IsUwp": true
    },
    {
      "Id": "e48b823f-c4b0-4218-a7e2-a8c80231228a",
      "Name": "Arc",
      "ExePath": "TheBrowserCompany.Arc_ttt1ap7aakyb4",
      "IsUwp": true
    }
  ]
}
```

### Limitations

- Icons cannot be loaded from a package family name. Choose a local image or URL in the icon chooser instead.
- The URL must be an absolute URI with a scheme, such as `https://github.com`.
- Default launches and automatic launches from rules or Quick View use Windows URI activation, which does not pass `LaunchArgs` or alternate-launch arguments. Alternate launches from the selector attempt to run `ExePath` as an executable and are not supported for package family names.
- Registry detection does not discover every packaged browser. Add missing browsers manually.

## Refreshing Browsers list

Select **Refresh** on the **Browsers** page in Hurl Settings to choose between two modes:

- **Preserve existing** compares detected browsers by `ExePath` and appends only those without an existing
  match. Existing settings and IDs are kept.
- **Add all detected** appends every detected browser as a new entry with a new ID, irrespective of whether an
  entry with the same executable path already exists.

### Limitations

- Refresh does not hide or remove uninstalled browsers, update existing entries, or repair rules and Quick View targets that reference deleted entries.
