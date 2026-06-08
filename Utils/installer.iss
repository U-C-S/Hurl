#define MyAppName "Hurl"
#define NameSmall "hurl"
#define MyAppVersion "0.10.0.002"
#define MyAppPublisher "U-C-S"
#define MyAppURL "https://github.com/U-C-S/Hurl"
#define ExeNativeHost "NativeMessagingHost.exe"
#define ExeSelector "Hurl Selector.exe"
#define ExeSettings "Hurl Settings.exe"
#define AppDescription "Choose the browser on the click of a link"


[Setup]
AppId={{56C63D05-9D83-492A-ABDD-618FE36ACBFB}}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
VersionInfoVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}/issues
AppUpdatesURL={#MyAppURL}/releases
AppReadmeFile={#MyAppURL}#README
SetupIconFile=..\Source\Hurl.App\Assets\internet.ico
LicenseFile=..\LICENSE
InfoBeforeFile=README.md
UninstallDisplayIcon={app}\{#ExeSettings}

UsePreviousAppDir=yes
DefaultDirName={autopf64}\{#MyAppName}
DisableProgramGroupPage=yes
PrivilegesRequired=admin
PrivilegesRequiredOverridesAllowed=dialog

OutputDir=.
OutputBaseFilename=Hurl_Installer

ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
Compression=lzma2
SolidCompression=yes

WizardStyle=modern
SetupLogging=yes
ChangesAssociations=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "protocol"; Description: "{cm:CreateProtocol}"; GroupDescription: "{cm:OtherOptions}"; Flags: unchecked

[Files]
Source: "..\target\release\{#ExeNativeHost}"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\_Publish\*"; DestDir: "{app}"; Excludes: "*.pdb,Hurl.exe,Hurl.dll,Hurl.BrowserSelector.dll,Hurl.Library.dll,*.deps.json,*.runtimeconfig.json"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\LICENSE"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Extensions\*"; Excludes: "package-lock.json,node_modules"; DestDir: "{app}\Extensions"; Flags: ignoreversion recursesubdirs

[InstallDelete]
Type: files; Name: "{app}\Hurl.exe"
Type: files; Name: "{app}\Hurl.dll"
Type: files; Name: "{app}\Hurl.BrowserSelector.dll"
Type: files; Name: "{app}\Hurl.Library.dll"
Type: files; Name: "{app}\Launcher.exe"
Type: files; Name: "{app}\*.deps.json"
Type: files; Name: "{app}\*.runtimeconfig.json"
Type: files; Name: "{app}\*.pdb"

[UninstallDelete]
Type: files; Name: "{app}\nmh-manifest.json"

[Icons]
Name: "{autoprograms}\{#MyAppName}\{#MyAppName}"; Filename: "{app}\{#ExeSelector}"; Comment: "{#AppDescription}"
Name: "{autoprograms}\{#MyAppName}\{#MyAppName} Settings"; Filename: "{app}\{#ExeSettings}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#ExeSelector}"; Comment: "{#AppDescription}"; Tasks: desktopicon
Name: "{autodesktop}\{#MyAppName} Settings"; Filename: "{app}\{#ExeSettings}"; Tasks: desktopicon

[Registry]
#define RootKey "HKA"
#define AppRegistrationName "Hurl"
#define AppCapabilitiesPath "Software\Hurl\Capabilities"
#define UrlProgId "Hurl.Url"
#define HtmlProgId "Hurl.Html"
#define PdfProgId "Hurl.Pdf"
#define HurlProtocolProgId "Hurl.Protocol"
#define LegacyProgId "HandleURL3721"

; Remove the old combined browser/file ProgID and legacy browser-registration path.
Root: {#RootKey}; Subkey: "Software\Clients\StartMenuInternet\{#MyAppName}"; Flags: deletekey
Root: {#RootKey}; Subkey: "Software\Classes\{#LegacyProgId}"; Flags: deletekey
Root: {#RootKey}; Subkey: "Software\Classes\.html\OpenWithProgids"; ValueType: none; ValueName: "{#LegacyProgId}"; Flags: deletevalue
Root: {#RootKey}; Subkey: "Software\Classes\.htm\OpenWithProgids"; ValueType: none; ValueName: "{#LegacyProgId}"; Flags: deletevalue
Root: {#RootKey}; Subkey: "Software\Classes\.pdf\OpenWithProgids"; ValueType: none; ValueName: "{#LegacyProgId}"; Flags: deletevalue

; Register Hurl as a Default Apps candidate for web protocols and supported files.
Root: {#RootKey}; Subkey: "{#AppCapabilitiesPath}"; ValueType: string; ValueName: "ApplicationName"; ValueData: "{#AppRegistrationName}"; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "{#AppCapabilitiesPath}"; ValueType: string; ValueName: "ApplicationDescription"; ValueData: "{#AppDescription}"; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "{#AppCapabilitiesPath}"; ValueType: string; ValueName: "ApplicationIcon"; ValueData: "{app}\{#ExeSelector},0"; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "{#AppCapabilitiesPath}\URLAssociations"; ValueType: string; ValueName: "http"; ValueData: "{#UrlProgId}"; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "{#AppCapabilitiesPath}\URLAssociations"; ValueType: string; ValueName: "https"; ValueData: "{#UrlProgId}"; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "{#AppCapabilitiesPath}\URLAssociations"; ValueType: string; ValueName: "{#NameSmall}"; ValueData: "{#HurlProtocolProgId}"; Tasks: protocol; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "{#AppCapabilitiesPath}\FileAssociations"; ValueType: string; ValueName: ".html"; ValueData: "{#HtmlProgId}"; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "{#AppCapabilitiesPath}\FileAssociations"; ValueType: string; ValueName: ".htm"; ValueData: "{#HtmlProgId}"; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "{#AppCapabilitiesPath}\FileAssociations"; ValueType: string; ValueName: ".pdf"; ValueData: "{#PdfProgId}"; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\RegisteredApplications"; ValueType: string; ValueName: "{#AppRegistrationName}"; ValueData: "{#AppCapabilitiesPath}"; Flags: uninsdeletevalue

; URL handler ProgID for http/https.
Root: {#RootKey}; Subkey: "Software\Classes\{#UrlProgId}"; ValueType: string; ValueName: ""; ValueData: "Hurl URL"; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\Classes\{#UrlProgId}"; ValueType: string; ValueName: "URL Protocol"; ValueData: ""; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\Classes\{#UrlProgId}\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\{#ExeSelector},0"; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\Classes\{#UrlProgId}\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#ExeSelector}"" ""%1"""; Flags: uninsdeletekey

; File handler ProgIDs.
Root: {#RootKey}; Subkey: "Software\Classes\{#HtmlProgId}"; ValueType: string; ValueName: ""; ValueData: "Hurl HTML Document"; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\Classes\{#HtmlProgId}\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\{#ExeSelector},0"; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\Classes\{#HtmlProgId}\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#ExeSelector}"" ""%1"""; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\Classes\{#PdfProgId}"; ValueType: string; ValueName: ""; ValueData: "Hurl PDF Document"; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\Classes\{#PdfProgId}\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\{#ExeSelector},0"; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\Classes\{#PdfProgId}\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#ExeSelector}"" ""%1"""; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\Classes\.html\OpenWithProgids"; ValueType: none; ValueName: "{#HtmlProgId}"; Flags: uninsdeletevalue
Root: {#RootKey}; Subkey: "Software\Classes\.htm\OpenWithProgids"; ValueType: none; ValueName: "{#HtmlProgId}"; Flags: uninsdeletevalue
Root: {#RootKey}; Subkey: "Software\Classes\.pdf\OpenWithProgids"; ValueType: none; ValueName: "{#PdfProgId}"; Flags: uninsdeletevalue

; Optional hurl:// protocol. This is not required for default browser registration.
Root: {#RootKey}; Subkey: "Software\Classes\{#HurlProtocolProgId}"; ValueType: string; ValueName: ""; ValueData: "Hurl Protocol"; Tasks: protocol; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\Classes\{#HurlProtocolProgId}"; ValueType: string; ValueName: "URL Protocol"; ValueData: ""; Tasks: protocol; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\Classes\{#HurlProtocolProgId}\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\{#ExeSelector},0"; Tasks: protocol; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\Classes\{#HurlProtocolProgId}\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#ExeSelector}"" --uri ""%1"""; Tasks: protocol; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\Classes\{#NameSmall}"; ValueType: string; ValueName: ""; ValueData: "URL:Hurl Protocol"; Tasks: protocol; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\Classes\{#NameSmall}"; ValueType: string; ValueName: "URL Protocol"; ValueData: ""; Tasks: protocol; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\Classes\{#NameSmall}\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\{#ExeSelector},0"; Tasks: protocol; Flags: uninsdeletekey
Root: {#RootKey}; Subkey: "Software\Classes\{#NameSmall}\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#ExeSelector}"" --uri ""%1"""; Tasks: protocol; Flags: uninsdeletekey


[CustomMessages]
OtherOptions=Other Options
CreateProtocol=Register hurl:// URL protocol (optional)
