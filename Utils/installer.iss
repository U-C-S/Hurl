#define MyAppName "Hurl"
#define NameSmall "hurl"
#define MyAppVersion "0.8.1.18"
#define MyAppPublisher "The 3721 Tools"
#define MyAppURL "https://github.com/U-C-S/Hurl"
#define MyAppExeName "Hurl.exe"

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
DefaultDirName={autopf}\{#MyAppName}
DisableProgramGroupPage=yes
PrivilegesRequired=admin

OutputDir=.
OutputBaseFilename=Hurl_Installer

ArchitecturesAllowed=x64
Compression=lzma
SolidCompression=yes
WizardStyle=modern

SetupLogging=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "protocol"; Description: "{cm:CreateProtocol}"; GroupDescription: "{cm:OtherOptions}"; Flags: unchecked

[Files]
Source: "..\_Publish\*.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\_Publish\*.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\_Publish\*.txt"; DestDir: "{app}/License"; Flags: ignoreversion
Source: "..\LICENSE"; DestDir: "{app}/License"; Flags: ignoreversion
Source: "..\Extensions\*"; Excludes: "package-lock.json,node_modules"; DestDir: "{app}\Extensions"; Flags: ignoreversion recursesubdirs; Tasks: Protocol;
Source: "..\_Publish\Hurl.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{autoprograms}\{#MyAppName}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Registry]
#define RootKey "HKCU"
#define URLAssociate "HandleURL3721"

Root: {#RootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}\Capabilities"; ValueType: string; ValueName: "ApplicationName"; ValueData: "{#MyAppName}"; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}\Capabilities"; ValueType: string; ValueName: "ApplicationDescription"; ValueData: "Hurl -- select browser dynamically"; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}\Capabilities"; ValueType: string; ValueName: "ApplicationIcon"; ValueData: "{app}\{#MyAppExeName},0"; Flags: uninsdeletekey deletevalue

Root: {#RootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}\Capabilities\StartMenu"; ValueType: string; ValueName: "StartMenuInternet"; ValueData: "{#MyAppName}"; Flags: uninsdeletekey deletevalue
Root: {#RootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}\Capabilities\URLAssociations"; ValueType: string; ValueName: "http"; ValueData: "{#URLAssociate}"; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}\Capabilities\URLAssociations"; ValueType: string; ValueName: "https"; ValueData: "{#URLAssociate}"; Flags: uninsdeletekey

Root: {#RootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\{#MyAppExeName},0"; Flags: uninsdeletekey deletevalue
Root: {#RootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppExeName}"""; Flags: uninsdeletekey deletevalue

Root: {#RootKey}; Subkey: "Software\Classes\{#URLAssociate}"; ValueType: string; ValueName: ""; ValueData: "{#MyAppName} URL"; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\Classes\{#URLAssociate}\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\{#MyAppExeName},0"; Flags: deletevalue
Root: {#RootKey}; Subkey: "Software\Classes\{#URLAssociate}\shell\open"; ValueType: string; ValueName: "Icon"; ValueData: """{app}\{#MyAppExeName}"""; Flags: deletevalue
Root: {#RootKey}; Subkey: "Software\Classes\{#URLAssociate}\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppExeName}"" ""%1"""; Flags: uninsdeletekey deletevalue

Root: {#RootKey}; Subkey: "Software\Classes\.html\OpenWithProgids"; ValueType: string; ValueName: "{#URLAssociate}"; ValueData: ""; Flags: uninsdeletevalue;
Root: {#RootKey}; Subkey: "Software\Classes\.htm\OpenWithProgids"; ValueType: string; ValueName: "{#URLAssociate}"; ValueData: ""; Flags: uninsdeletevalue;
Root: {#RootKey}; Subkey: "Software\Classes\.pdf\OpenWithProgids"; ValueType: string; ValueName: "{#URLAssociate}"; ValueData: ""; Flags: uninsdeletevalue;


Root: {#RootKey}; Subkey: "Software\RegisteredApplications"; ValueType: string; ValueName: "{#MyAppName}"; ValueData: "Software\Clients\StartMenuInternet\{#MyAppName}\Capabilities"; Flags: deletevalue uninsdeletevalue

Root: HKCR ; Subkey: "{#NameSmall}"; ValueType: string; ValueName: ""; ValueData: "URL:{#NameSmall}"; Tasks: protocol; Flags: uninsdeletekey deletevalue
Root: HKCR ; Subkey: "{#NameSmall}"; ValueType: string; ValueName: "URL Protocol"; ValueData: ""; Tasks: protocol; Flags: uninsdeletekey
Root: HKCR ; Subkey: "{#NameSmall}\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppExeName}"" ""%1"""; Tasks: protocol; Flags: uninsdeletekey deletevalue


[CustomMessages]
OtherOptions=Other Options
CreateProtocol=Create Protocol (Required for Extension)