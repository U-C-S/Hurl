# User settings

When Hurl is lauched for the first time, it automatically detects the installed browsers and creates a _UserSettings.json_ file at `C:\Users\{USER}\AppData\Roaming\Hurl` filling it with browsers it detected. A typical UserSettings.json file looks like this:

```json
{
  "LastUpdated": "22-Feb-22 2:22:22 AM",
  "Version": "0.6.2",
  // "AppSettings: {...},
  "Browsers": [
    {
      "Name": "Brave",
      "ExePath": "C:\\Program Files\\BraveSoftware\\Brave-Browser\\Application\\brave.exe",
      "Hidden": false
    },
    {
      "Name": "Google Chrome Dev",
      "ExePath": "C:\\Program Files\\Google\\Chrome Dev\\Application\\chrome.exe",
      "AlternateLaunches": [
        {
          "ItemName": "Profile 1",
          "LaunchArgs": "--profile-directory=\"Default\""
        },
        {
          "ItemName": "Incognito",
          "LaunchArgs": "-incognito"
        }
      ]
    }
  ]
}
```

## App settings

The following snippet shows the default options:

```json
"AppSettings": {
    "LaunchUnderMouse": false,
    "MinimizeOnFocusLoss": true,
    "NoWhiteBorder": false,
    "BackgroundType": "mica",
    "RuleMatching": false,
    "WindowSize": [460,210]
}
```

### Available options

- `LaunchUnderMouse` default is **false**, can be used to launch the Hurl window under the mouse when enabled
- `MinimizeOnFocusLoss` default is **true**
- `NoWhiteBorder` set **true** or **false** to enable or disable the white border around the window
- `BackgroundType` supports **mica**, **acrylic**, **none**
  - Windows 11 22H2 or above supports all options
  - Windows 11 build 22000 supports only mica
  - Windows 10 default is **none** irrespective of option
- `RuleMatching` default is **false**. On enabling, it supports the features from [Rulesets](https://github.com/U-C-S/Hurl/wiki/Rulesets)
- `WindowSize` is to store the size of Hurl BrowserSelect window. You dont need to set this, it will be saved automatically when the window is resized.

## Feature Documentation

- [Browser Configuration](./Features/BrowserConfiguration.md)
- [Rule Matching](./Features/RuleMatching.md)
