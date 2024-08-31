# Hurl browser extension

This browser extension uses Native Messaging API to communicate with the Hurl Application.

## Installation

To use it in Chrome, follow the steps below:

- Enable the Developer Mode for Extensions in Chrome ([instructions on Chromium blog](https://blog.chromium.org/2009/06/developer-tools-for-google-chrome.html))
- Select **Load Unpacked**
- Choose the folder `{installationDir}/Extensions/Chrome`

Run the `install-nmh.ps1` script as admin located in `{installationDir}/Extensions` after installing the extension. Also get the extension id from the extension page in Chrome.

```powershell
cd {installationDir}
.\Extensions\install-nmh.ps1 {EXTENSION_ID} {<optional> dir_where_Launcher.exe_is}
```

You can modify the script to use it for other chromium-based browsers by editing the REG command in it and pointing to appropriate browser's registry key for Native Messaging Hosts.

During **Development**, use the `dir_where_Launcher.exe_is` to point to the directory where the _Launcher.exe_ is located (usually to `Source/Launcher/target/debug`).
