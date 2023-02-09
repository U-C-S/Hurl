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

## Why and What?

Sometimes you might want to open a link in a browser of your choice, instead of the default one. Hurl can let you choose the browser eachtime you click a link (links outside of browser). So naturally, It acts as default browser to do that. And hurl can be powered up with a _browser extension_ to let you open a browser tab in different browser.

- Modern Windows UI with Multiple Customization Options
- Supports adding own browser config with Launch Arguments
- Rules to automatically open a browser without prompting (coming soon!)
- Settings application to manage all the features (coming soon!)
- Web Extension to open browser tabs in Hurl (experimental)

<p align="center">
  <img width="720" src="https://user-images.githubusercontent.com/50218121/198988257-7f89288c-7fd4-4bf3-8d7f-b5501d81ac61.png" />
  <!--<img width="640" src="https://user-images.githubusercontent.com/50218121/158625754-78026dbe-cd99-4078-8407-313b9c548ca1.png" />-->
  <!--<img width="640" src="https://user-images.githubusercontent.com/50218121/157494232-a134a412-9dd7-4706-8be7-6e3800484082.png" />-->
</p>

> As a Web-Developer, Web-Surfer and someone who uses 3 browsers, Hurl is a bliss - Me probably

## Installation & Usage

Download Install the latest [.NET 6 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0/runtime) and Get the Hurl_Installer from [Releases](https://github.com/U-C-S/Hurl/releases/latest). Your PC's Anti-Virus might warn that it's not secure to download or install it, You can ignore it as the executables are Un-signed. Lastly, After installing, You might need to set Hurl as the default `http/https` protocol handler in the Windows Settings, just like how you change the default browser.

### Check out wiki for configuring - [Editing UserSettings.json](https://github.com/U-C-S/Hurl/wiki/Editing-UserSettings.json)

## Dependencies and Tools used

- [Visual Studio](https://visualstudio.microsoft.com)
- [Inno Setup](https://jrsoftware.org/isinfo.php)
- [.NET 6.0](https://dot.net) , [C#](https://dotnet.microsoft.com/languages/csharp) and [WPF](https://docs.microsoft.com/en-us/visualstudio/designers/getting-started-with-wpf)
- [WPF UI](https://github.com/lepoco/wpfui)

## Building from Source

- Install Inno Setup and Visual Studio 2022 in your Windows with .NET Desktop Development workload
- After Cloning the Repo, Open the solution file `./Hurl.sln` in Visual Studio
- In Debug / Release mode, Run the `Hurl.BrowserSelector` Project (Publish the executables to `./_Publish/*` for the installer script usage)
- Install Inno Setup, Open the `Utils/installer.iss` script in it and Compile it to create the Installer (output in the same directory)

or Simply use the Build Script from `Utils/build.ps1` to build the Application in release mode and successively build the installer.

For checking out older versions source code, Check the [Tags](https://github.com/U-C-S/Hurl/tags).

### Using the Extension

This is a experimental feature. Is implemented as hacky way. Use at ur own risk!

- Enable the Developer Mode for Extensions in Edge or Chrome (Not tested in Firefox)
- Choose the folder `{installationDir}/Extensions/Chrome` after clicking `Load Unpacked` Button available after enabling Developer mode

## Contributing

The Project is open to Pull-Requests and Feedback. MIT License.

## Credits

- Icon used is from [FlatIcons](https://www.flaticon.com/free-icon/internet_4861937)
- Inspiration from Repository [zumoshi/BrowserSelect](https://github.com/zumoshi/BrowserSelect) and other [similar projects](https://github.com/U-C-S/Hurl/issues/5)
