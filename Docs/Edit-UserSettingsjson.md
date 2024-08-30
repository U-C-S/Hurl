When Hurl is lauched for the first time, it automatically detects the installed browsers in the computer and Creates a UserSettings.json file at `C:\Users\{USER}\AppData\Roaming\Hurl` filling it with browsers it detected. A typical UserSettings.json file looks like this.

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

## App Settings

The following snippet does this shows the default options
```json
"AppSettings": {
  "LaunchUnderMouse": false,
  "RuleMatching": false,
  "NoWhiteBorder": false,
  "BackgroundType": "mica",
  "WindowSize": [420,210]
}
```

### Available Options
- `LaunchUnderMouse` defaulted to false, can be used to launch the hurl window under the mouse when enabled
- `NoWhiteBorder` set true or false to enable or disable the white border around the window.
- `BackgroundType` supports `mica`, `acrylic`, `none`
  - win11 22H2 or above supports all options
  - win11 build 22000 only mica
  - win10 is defaulted to `none` irrespective of option
- `WindowSize` is to set the size of Hurl BrowserSelect window. You dont need to set this, Just resize the window accordingly, It get filled automatically.
- `RuleMatching` defaulted to false, on enabling, it supports the features from [Rulesets](https://github.com/U-C-S/Hurl/wiki/Rulesets)

## Browsers Field

- `Name` - you know (Req.)
- `ExePath` - The path of browser main exe file (Req.)
- `CustomIconPath` - Use absolute path of the image. Also, Supports URLs (OPTIONAL)
- `LaunchArgs` - Add the default exe launch arguments here. use param `%URL%` injecting the Url at runtime here (OPTIONAL)
- `Hidden` - set it to `true` to hide the current icon in the selection screen (OPTIONAL)
- `AlternateLaunches` - This is an array .... see below (OPTIONAL)

### AlternateLaunches
This is a way to launch the browser when you have multiple launch methods or launch targets... like incognito, browser profiles...

Suppose you have multiple chrome profiles like this

![image](https://user-images.githubusercontent.com/50218121/158058450-21eda36a-9794-4f34-97c7-ae5c5f105412.png)

Then you might wanna use this feature, instead of totally adding a new browser entity for each profile in settings file. The following snippet demonstrates this feature. Adding the `AlternateLaunches` field to browser entity will you simply right-click on the browser icon in the selection window and choose the req. option.

```json
"AlternateLaunches": [
  {
    "ItemName": "Main Profile",
    "LaunchArgs": "--profile-directory=\"Default\""
  },
  {
    "ItemName": "Profile 2",
    "LaunchArgs": "--profile-directory=\"Profile 1\""
  },
  {
    "ItemName": "Incognito",
    "LaunchArgs": "-incognito"
  }
]
```

Right-Clicking on the browser that has `AlternateLauches` brings up the context menu with options specified as in the settings file. On selecting the the URL will also automatically included while launching the browser.

![image](https://user-images.githubusercontent.com/50218121/172186598-9414b860-ecaf-4bb4-bc69-a49c6cfedfdb.png)

- `ItemName` - The name that shows up in the context menu for this launch
- `LaunchArgs` - Launch args. You can keep launch the browser in incognito, other browser profiles...
