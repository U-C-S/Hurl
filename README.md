<p align="center">
  <img width="128" align="center" src="App/Common/internet.ico">
</p>
<h1 align="center">
  Hurl
</h1>
<p align="center">
  A windows utility that lets you choose the browser on the click of a link
</p>
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

## Why and What?

The Default Browser War in Windows is one of the reasons for creation of Hurl and Sometimes you might want to open a link in a browser of your choice, instead of the default one. Hurl is used to let you choose the browser everytime you click a link (links outside of browser). So naturally, It acts as default browser to do that. And hurl can be powered up with a _browser extension_ to let you open a browser tab in another browser.

- Fast startup and lightweight
- Supports adding your own browsers with Launch Args
- Web Extension to open browser tabs in Hurl

## Installation

Get the Installer from [Releases](https://github.com/U-C-S/Hurl/releases/latest). Your PC's Anti-Virus might warn that it's not secure to download or install it, You can ignore it as the executables are Raw files and are Un-signed. After installing, You need to set Hurl as the default `http/https` protocol handler in the Windows Settings. Use `Hurl Settings` App (from Start Menu App list) to customize the Browsers.

## Dependencies and Tools

- [Visual Studio](https://visualstudio.microsoft.com)
- [Inno Setup](https://jrsoftware.org/isinfo.php)
- [Windows Presentation Foundation](https://docs.microsoft.com/en-us/visualstudio/designers/getting-started-with-wpf) - .NET Framework 4.8
- [C#](https://dotnet.microsoft.com/languages/csharp)

## Building from Source

- Install Visual Studio 2019 or 2022 in your Windows with .NET Desktop Development workload
- After Cloning the Repo, Open the solution file `App/Hurl.sln` in Visual Studio
- In Debug / Release mode, Run the `Hurl.Settings` Project which builds all the projects in the solution and Opens Settings Window
- `App/_bin` contains resulting executables
- Install Inno Setup, Open the `Utils/installer.iss` script and Compile it to create the Installer (output in the `Utils` directory)

### Using the Extension

- Enable the Developer Mode for Extensions in Edge or Chrome
- Choose the folder `Extensions/Chrome` after clicking `Load Unpacked` Button available after enabling Developer mode

## Contributing

The Project welcomes Pull-Requests. So, If you like the Project or Using it, also consider:

- Giving a Star ⭐
- Contributing code....
