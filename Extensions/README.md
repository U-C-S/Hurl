# Hurl Extension

This browser extension uses Native Messaging API to communicate with the Hurl Application.

## Installation

To use it in Chrome, follow the steps below:

- Enable the Developer Mode for Extensions in Chrome
- Choose the folder `{installationDir}/Extensions/Chrome` after clicking `Load Unpacked` Button available after enabling Developer mode

Run the `install-nmh.ps1` script as admin located in `{installationDir}/Extensions` after installing the extension. Also Get the extension id from the extension page in Chrome.

```powershell
cd {installationDir}
.\Extensions\install-nmh.ps1 {EXTENSION_ID} {<optional> dir_where_Launcher.exe_is}
```

You can modify the script to use it for other chromium-based browsers by editing the REG command in it and pointing to appropriate browser's registry key for Native Messaging Hosts.

During **Development**, Use the `dir_where_Launcher.exe_is` to point to the directory where the `Launcher.exe` is located (usually to `Source/Launcher/target/debug`).
