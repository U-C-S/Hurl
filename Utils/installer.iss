#include "install_dotnet.iss"

#define MyAppName "Hurl"
#define NameSmall "hurl"
#define MyAppVersion "0.9.0.22"
#define MyAppPublisher "U-C-S"
#define MyAppURL "https://github.com/U-C-S/Hurl"
#define ExeLauncher "Launcher.exe"
#define ExeSelector "Hurl.exe"
#define ExeSettings "Hurl.Settings.exe"
#define AppDescription "Choose the browser on the click of a link"


[Setup]
AppId={{56C63D05-9D83-492A-ABDD-618FE36ACBFB}}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
VersionInfoVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL="https://github.com/U-C-S/Hurl"
AppSupportURL="https://github.com/U-C-S/Hurl/issues"
AppUpdatesURL="https://github.com/U-C-S/Hurl/releases"
AppReadmeFile="https://github.com/U-C-S/Hurl#README"
SetupIconFile=..\Source\Hurl.BrowserSelector\Assets\internet.ico
LicenseFile=..\LICENSE
InfoBeforeFile=README.md

UsePreviousAppDir=yes
DefaultDirName={autopf64}\{#MyAppName}
DisableProgramGroupPage=yes
PrivilegesRequired=admin
PrivilegesRequiredOverridesAllowed=dialog

OutputDir=.
OutputBaseFilename=Hurl_Installer

ArchitecturesAllowed=x64
Compression=lzma2
SolidCompression=yes

WizardStyle=modern
SetupLogging=yes

[Code]
function InitializeSetup: Boolean;
begin
  InstallDotNetDesktopRuntime;
  Result := True;
end;

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "protocol"; Description: "{cm:CreateProtocol}"; GroupDescription: "{cm:OtherOptions}"

[Files]
Source: "..\Source\Launcher\target\release\Launcher.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\_Publish\*.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\_Publish\*.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\LICENSE"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Extensions\*"; Excludes: "package-lock.json,node_modules"; DestDir: "{app}\Extensions"; Flags: ignoreversion recursesubdirs; Tasks: Protocol;
Source: "..\_Publish\*.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\_Publish\*.pri"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{autoprograms}\{#MyAppName}\{#MyAppName}"; Filename: "{app}\{#ExeLauncher}"; Comment: "{#AppDescription}"
Name: "{autoprograms}\{#MyAppName}\{#MyAppName} Settings"; Filename: "{app}\{#ExeSettings}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#ExeLauncher}"; Comment: "{#AppDescription}"; Tasks: desktopicon
Name: "{autodesktop}\{#MyAppName} Settings"; Filename: "{app}\{#ExeSettings}"; Tasks: desktopicon

[Registry]
#define RootKey "HKCU"
#define URLAssociate "HandleURL3721"

Root: {#RootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}"; ValueType: string; ValueName: ""; ValueData: "{#MyAppName}"; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}\Capabilities"; ValueType: string; ValueName: "ApplicationName"; ValueData: "{#MyAppName}"; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}\Capabilities"; ValueType: string; ValueName: "ApplicationDescription"; ValueData: "{#AppDescription}"; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}\Capabilities"; ValueType: string; ValueName: "ApplicationIcon"; ValueData: "{app}\{#ExeLauncher},0"; Flags: uninsdeletekey deletevalue

Root: {#RootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}\Capabilities\StartMenu"; ValueType: string; ValueName: "StartMenuInternet"; ValueData: "{#MyAppName}"; Flags: uninsdeletekey deletevalue
Root: {#RootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}\Capabilities\URLAssociations"; ValueType: string; ValueName: "http"; ValueData: "{#URLAssociate}"; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}\Capabilities\URLAssociations"; ValueType: string; ValueName: "https"; ValueData: "{#URLAssociate}"; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}\Capabilities\FileAssociations"; ValueType: string; ValueName: ".html"; ValueData: "{#URLAssociate}"; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}\Capabilities\FileAssociations"; ValueType: string; ValueName: ".htm"; ValueData: "{#URLAssociate}"; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}\Capabilities\FileAssociations"; ValueType: string; ValueName: ".pdf"; ValueData: "{#URLAssociate}"; Flags: uninsdeletekey

Root: {#RootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\{#ExeLauncher},0"; Flags: uninsdeletekey deletevalue
Root: {#RootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#ExeLauncher}"""; Flags: uninsdeletekey deletevalue

Root: {#RootKey}; Subkey: "Software\Classes\{#URLAssociate}"; ValueType: string; ValueName: ""; ValueData: "{#MyAppName} URL"; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\Classes\{#URLAssociate}\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\{#ExeLauncher},0"; Flags: deletevalue
Root: {#RootKey}; Subkey: "Software\Classes\{#URLAssociate}\shell\open"; ValueType: string; ValueName: "Icon"; ValueData: """{app}\{#ExeLauncher}"""; Flags: deletevalue
Root: {#RootKey}; Subkey: "Software\Classes\{#URLAssociate}\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#ExeLauncher}"" ""%1"""; Flags: uninsdeletekey deletevalue

Root: {#RootKey}; Subkey: "Software\Classes\.html\OpenWithProgids"; ValueType: string; ValueName: "{#URLAssociate}"; ValueData: ""; Flags: uninsdeletevalue;
Root: {#RootKey}; Subkey: "Software\Classes\.htm\OpenWithProgids"; ValueType: string; ValueName: "{#URLAssociate}"; ValueData: ""; Flags: uninsdeletevalue;
Root: {#RootKey}; Subkey: "Software\Classes\.pdf\OpenWithProgids"; ValueType: string; ValueName: "{#URLAssociate}"; ValueData: ""; Flags: uninsdeletevalue;

Root: {#RootKey}; Subkey: "Software\RegisteredApplications"; ValueType: string; ValueName: "{#MyAppName}"; ValueData: "Software\Clients\StartMenuInternet\{#MyAppName}\Capabilities"; Flags: deletevalue uninsdeletevalue

Root: HKCR ; Subkey: "{#NameSmall}"; ValueType: string; ValueName: ""; ValueData: "URL:{#NameSmall}"; Tasks: protocol; Flags: uninsdeletekey deletevalue
Root: HKCR ; Subkey: "{#NameSmall}"; ValueType: string; ValueName: "URL Protocol"; ValueData: ""; Tasks: protocol; Flags: uninsdeletekey
Root: HKCR ; Subkey: "{#NameSmall}\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#ExeLauncher}"" ""%1"""; Tasks: protocol; Flags: uninsdeletekey deletevalue


[CustomMessages]
OtherOptions=Other Options
CreateProtocol=Create Protocol (Required for Extension)
