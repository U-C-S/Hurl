<img width="150" align="left" src="Source/Hurl.BrowserSelector/Assets/internet.ico">

<h1 align="center">Hurl</h1>

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

> This software is currently in pre-v1.0 version, which means it can frequently introduce breaking changes with new versions.

## Why and What?

Sometimes you might want to open a link in a browser of your choice, instead of the default one. Hurl can let you choose the browser eachtime you click a link (links outside of browser). So naturally, It acts as default browser to do that. And hurl can be powered up with a _browser extension_ to let you open a browser tab in different browser.

- Modern Windows UI with Multiple Customization Options
- Supports adding own browser config with Launch Arguments
- Rules to automatically open a browser without prompting
- Settings application to manage all the features (beta)
- Web Extension to open browser tabs in Hurl (experimental)

<p align="center">
  <img width="720" src="https://github.com/user-attachments/assets/dc4cc718-abf8-4032-9ef9-df832a1d059b" />
  <!--<img width="720" src="" />-->
  <!--https://github.com/user-attachments/assets/7b3418fb-38e1-4259-85c6-11603c6eec7d-->
  <!--<img width="720" src="https://user-images.githubusercontent.com/50218121/230982396-152a2342-f02a-47c0-9349-3d1a4920554f.png" />-->
  <!--<img width="720" src="https://user-images.githubusercontent.com/50218121/198988257-7f89288c-7fd4-4bf3-8d7f-b5501d81ac61.png" />-->
  <!--<img width="640" src="https://user-images.githubusercontent.com/50218121/158625754-78026dbe-cd99-4078-8407-313b9c548ca1.png" />-->
  <!--<img width="640" src="https://user-images.githubusercontent.com/50218121/157494232-a134a412-9dd7-4706-8be7-6e3800484082.png" />-->
</p>

> As a Web-Developer, Web-Surfer and someone who uses 3 browsers, Hurl is a bliss - Me probably

## Installation & Usage

Download and Install the latest versions of
- [.NET 8 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)
- Windows App Runtime v1.5 [Direct Download Link](https://aka.ms/windowsappsdk/1.5/latest/windowsappruntimeinstall-x64.exe)

Get the Hurl_Installer from [Releases](https://github.com/U-C-S/Hurl/releases/latest) and Install it.

Lastly, After installing, Make sure to set Hurl as the default `http/https` protocol handler in the Windows Settings ( For Windows 11, `Settings > Apps > Defualt apps > Hurl`), just like how you change the default browser.

> Check out [Hurl Wiki](https://github.com/U-C-S/Hurl/wiki/) for more details on usage and configuration. And [Extensions/README.md](./Extensions/README.md) for installing the Browser Extension.

## Building from Source / Local Development

- Install Visual Studio 2022 with following workloads
  - .NET desktop development
  - Windows application development
  - Desktop development with C++ (required for building Launcher)
- After Cloning the Repo, Open the solution file `./Hurl.sln` in Visual Studio. You can change the projects between Hurl.BrowserSelector and Hurl.Settings
- Install [Rustup / Setup Rust complier](https://www.rust-lang.org/tools/install) locally to debug Launcher app
- Install Inno Setup, to create the Hurl Installer

Use the Build Script from `Utils/build.ps1` to build the Application in release mode and successively build the installer. Make sure you have all the tools installed mentioned in the above description.

To check out older versions source code, Use the [Github Tags](https://github.com/U-C-S/Hurl/tags).

## Contributing

The Project is open to Pull-Requests and Feedback. MIT License.

## Credits

- Icon used is from [FlatIcons](https://www.flaticon.com/free-icon/internet_4861937)
- Inspiration from Repository [zumoshi/BrowserSelect](https://github.com/zumoshi/BrowserSelect) and other [similar projects](https://github.com/U-C-S/Hurl/issues/5)
