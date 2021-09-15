#define MyAppName "Hurl"
#define MyAppVersion "0.2.0"
#define MyAppPublisher "The 3721 Tools"
#define MyAppURL "https://github.com/U-C-S/Hurl"
#define MyAppExeName "Hurl.exe"

[Setup]
AppId={{56C63D05-9D83-492A-ABDD-618FE36ACBFB}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}

DefaultDirName={autopf}\{#MyAppName}
DisableProgramGroupPage=yes
PrivilegesRequiredOverridesAllowed=commandline

OutputDir=.
OutputBaseFilename=Hurl_Installer

Compression=lzma
SolidCompression=yes
WizardStyle=classic

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "protocol"; Description: "{cm:CreateProtocol}"; GroupDescription: "{cm:OtherOptions}"; Flags: unchecked

[Files]
Source: "..\_bin\Release\Hurl.exe"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Registry]
#define SoftwareClassesRootKey "HKCU"
#define URLAssociate "HandleURL3721"

Root: {#SoftwareClassesRootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}\Capabilities"; ValueType: string; ValueName: "ApplicationName"; ValueData: "{#MyAppName}"; Flags: uninsdeletekey
Root: {#SoftwareClassesRootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}\Capabilities"; ValueType: string; ValueName: "ApplicationDescription"; ValueData: "Hurl -- select browser dynamically"; Flags: uninsdeletekey
Root: {#SoftwareClassesRootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}\Capabilities"; ValueType: string; ValueName: "ApplicationIcon"; ValueData: "{app}\{#MyAppExeName},0"; Flags: uninsdeletekey

Root: {#SoftwareClassesRootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}\Capabilities\StartMenu"; ValueType: string; ValueName: "StartMenuInternet"; ValueData: "{#MyAppName}"; Flags: uninsdeletekey
Root: {#SoftwareClassesRootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}\Capabilities\URLAssociations"; ValueType: string; ValueName: "http"; ValueData: "{#URLAssociate}"; Flags: uninsdeletekey
Root: {#SoftwareClassesRootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}\Capabilities\URLAssociations"; ValueType: string; ValueName: "https"; ValueData: "{#URLAssociate}"; Flags: uninsdeletekey

Root: {#SoftwareClassesRootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\{#MyAppExeName},0"; Flags: uninsdeletekey
Root: {#SoftwareClassesRootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppExeName}"""; Flags: uninsdeletekey


Root: {#SoftwareClassesRootKey}; Subkey: "Software\Classes\{#URLAssociate}"; ValueType: string; ValueName: ""; ValueData: "{#MyAppName} URL"; Flags: uninsdeletekey
Root: {#SoftwareClassesRootKey}; Subkey: "Software\Classes\{#URLAssociate}\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppExeName}"" ""%1"""; Flags: uninsdeletekey
;Root: {#SoftwareClassesRootKey}; Subkey: "Software\Classes\{#URLAssociate}\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\{#MyAppExeName},0"; Flags: uninsdeletekey

Root: {#SoftwareClassesRootKey}; Subkey: "Software\RegisteredApplications"; ValueType: string; ValueName: "{#MyAppName}"; ValueData: "Software\Clients\StartMenuInternet\{#MyAppName}\Capabilities"; Flags: deletevalue

[CustomMessages]
OtherOptions=Other Options
CreateProtocol=Create Protocol (which is used for Extension)