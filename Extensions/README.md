# Hurl Extension

This browser extension uses Native Messaging API to communicate with the Hurl Application.

## Installation

To use it in Chrome or Chromium based browsers, follow the steps below:

- Enable the Developer Mode for Extensions in Edge or Chrome
- Choose the folder `{installationDir}/Extensions/Chrome` after clicking `Load Unpacked` Button available after enabling Developer mode

Run the `install-nmh.ps1` script located in `{installationDir}/Extensions` after installing the extension. Also Get the extension id from the extension page in Chrome.

```powershell
.\install-nmh.ps1 {EXTENSION_ID} {<optional> dir_where_Launcher.exe_is}
```
