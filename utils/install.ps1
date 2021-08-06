# REG ADD HKCU\Software\Hurl

# REG ADD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\Hurl" \
#                  "DisplayName" "Hurl -- select browser dynamically"
# REG ADD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\Hurl" \
#                  "UninstallString" "$\"$INSTDIR\uninstall.exe$\""


REG ADD HKCU\Software\Clients\StartMenuInternet\Hurl\Capabilities /v ApplicationName /d Hurl
REG ADD HKCU\Software\Clients\StartMenuInternet\Hurl\Capabilities /v ApplicationDescription /d "Hurl -- select browser dynamically"
REG ADD HKCU\Software\Clients\StartMenuInternet\Hurl\Capabilities  /v ApplicationIcon /d "D:\lolx\Hurl.exe,0"

REG ADD HKCU\Software\Clients\StartMenuInternet\Hurl\Capabilities\StartMenu  /v StartMenuInternet /d Hurl
REG ADD HKCU\Software\Clients\StartMenuInternet\Hurl\Capabilities\URLAssociations /v http /d HandleURL3721
REG ADD HKCU\Software\Clients\StartMenuInternet\Hurl\Capabilities\URLAssociations /v https /d HandleURL3721
REG ADD HKCU\Software\Clients\StartMenuInternet\Hurl\DefaultIcon /ve /d "D:\lolx\Hurl.exe,0"
REG ADD HKCU\Software\Clients\StartMenuInternet\Hurl\shell\open\command /ve /d "D:\lolx\Hurl.exe"

# ;register capablities
REG ADD HKCU\Software\RegisteredApplications /v Hurl /d "Software\Clients\StartMenuInternet\Hurl\Capabilities"

# ;register handler
REG ADD HKCU\Software\Classes\HandleURL3721 /ve /d Hurl Url
# REG ADD HKCU\Software\Classes\HandleURL3721\shell\open\command /ve /d "D:\lolx\Hurl.exe"

$value = "`"D:\lolx\Hurl.exe`" `"%1`""
New-ItemProperty -Path "HKCU\Software\Classes\HandleURL3721\shell\open\command" ` -Name " " ` -Value $value `