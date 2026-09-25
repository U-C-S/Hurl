# User settings

When Hurl is lauched for the first time, it automatically detects the installed browsers and creates a _UserSettings.json_ file at `C:\Users\{USER}\AppData\Roaming\Hurl` filling it with browsers it detected. A typical UserSettings.json file looks like this:

```json
{
  "AppSettings": {
    "LaunchUnderMouse": false,
    "MinimizeOnFocusLoss": true,
    "BackgroundType": "mica",
    "RuleMatching": false,
    "WindowSize": [500, 260]
  },
  "Browsers": [
    {
      "Id": "2b36a1fe-97f7-4509-ae6e-5c2c61602af4",
      "Name": "Brave",
      "ExePath": "C:\\Program Files\\BraveSoftware\\Brave-Browser\\Application\\brave.exe",
      "Hidden": false
    },
    {
      "Id": "e48b823f-c4b0-4218-a7e2-a8c80231228a",
      "Name": "Google Chrome Dev",
      "ExePath": "C:\\Program Files\\Google\\Chrome Dev\\Application\\chrome.exe",
      "AlternateLaunches": [
        {
          "Id": "f81a1698-1488-461d-9294-4f4f8313178e",
          "ItemName": "Profile 1",
          "LaunchArgs": "--profile-directory=\"Default\""
        },
        {
          "Id": "4bd80686-0a0b-4668-8ed0-063989e99c14",
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
    "BackgroundType": "mica",
    "RuleMatching": false,
    "WindowSize": [460,230]
}
```

### Available options

- `LaunchUnderMouse` default is **false**, can be used to launch the Hurl window under the mouse when enabled
- `MinimizeOnFocusLoss` default is **true**
- `BackgroundType` supports **mica** (default) and **acrylic** for the selector window.
- `RuleMatching` defaults to **false**. Enable it in **Hurl Settings > Rulesets** to automatically open links using [rule matching](./Features/RuleMatching.md).
- `WindowSize` stores the selector's width and height, defaults to **[500, 260]**, and is saved when you finish resizing the window. The minimum size is also 500 by 260.


## Feature Documentation

- [Browser Configuration](./Features/BrowserConfiguration.md)
- [Rule Matching](./Features/RuleMatching.md)
