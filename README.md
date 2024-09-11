<div align="center">
  <img width="130" src="Source/Hurl.BrowserSelector/Assets/internet.ico">
  
  <h1>Hurl</h1>
  
  <p align="center">A windows utility that lets you choose a browser on the click of a link</p>
  
  <p align="center">
    <a style="text-decoration:none" href="https://github.com/U-C-S/Hurl/releases">
      <img src="https://img.shields.io/github/v/release/u-c-s/hurl?color=red&label=latest%20version&style=flat-square" alt="Releases" />
    </a>
    <a style="text-decoration:none">
      <img src="https://img.shields.io/badge/platform-Windows%2010%20%26%2011-blue.svg?style=flat-square" alt="Platform" />
    </a>
    <a style="text-decoration:none">
      <img src="https://img.shields.io/github/license/u-c-s/hurl?style=flat-square" alt="License" />
    </a>
    <a style="text-decoration:none" href="https://github.com/U-C-S/Hurl/commits">
      <img src="https://img.shields.io/github/last-commit/u-c-s/hurl?color=orange&style=flat-square" alt="Commits" />
    </a>
  </p>
</div>

> [!NOTE]
> This software is currently in pre-v1.0 version, which means it can frequently introduce breaking changes with new versions.

## Why and what?

Sometimes you might want to open a link in a browser of your choice, instead of the default one. Hurl lets you choose the browser each time you click a link (links outside of browsers). So naturally, it acts as default browser to do that.
Hurl can also be powered up with a _browser extension_ to let you open a browser tab in a different browser.

- Modern Windows UI with multiple customization options
- Supports adding custom browser configuration with Launch Arguments
- Rules to automatically open a browser without prompting
- Settings application to manage all the features (beta)
- Web Extension to open browser tabs in Hurl (experimental)
- ...more to come

<p align="center">
  <img width="720" src="/Docs/Images/HurlMainWindow.png" />
</p>


## Installation and usage

Download and install the latest versions of:

- [.NET 8 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)
- Windows App Runtime v1.5 [Direct Download Link](https://aka.ms/windowsappsdk/1.5/latest/windowsappruntimeinstall-x64.exe)
- [Hurl_Installer](https://github.com/U-C-S/Hurl/releases/latest)

> [!TIP]
> It is recommended to uninstall your current version before installing a new version.

After installing, make sure to set Hurl as the default `http/https` protocol handler in the Windows Settings. In Windows 11: **Settings** > **Apps** > **Default apps** > **Hurl** > **Set as standard browser**.

Go to [Hurl Wiki](https://github.com/U-C-S/Hurl/wiki/) for more details on usage and configuration. See [Extensions readme](./Extensions/README.md) for installing the Browser Extension.

## Building from source / local development

- Install [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/) with following workloads:
  - .NET desktop development
  - Windows application development
  - Desktop development with C++ (required for building Launcher)
- After forking and cloning the repository, open the solution file `./Hurl.sln` in Visual Studio. You can change the projects between _Hurl.BrowserSelector_ and _Hurl.Settings_
- Install [Rustup / Setup Rust complier](https://www.rust-lang.org/tools/install) locally to debug Launcher app
- Install [Inno Setup](https://jrsoftware.org/isdl.php) to create the Hurl Installer

Use the Build Script from `Utils/build.ps1` to build the application in _Release_ mode and build the installer. Make sure you have all the tools installed mentioned in the above description.

To check out older versions source code, go to [Github Tags](https://github.com/U-C-S/Hurl/tags).

## Contributing

The project is open to Pull-Requests and Feedback. MIT License.

## Credits

- Icon used is from [FlatIcons](https://www.flaticon.com/free-icon/internet_4861937)
- Inspiration from Repository [zumoshi/BrowserSelect](https://github.com/zumoshi/BrowserSelect) and other [similar projects](https://github.com/U-C-S/Hurl/issues/5)
