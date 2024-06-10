$CURRENT_DIR = Get-Location
$PATH_TO_NMHOST = "$CURRENT_DIR\nmh-manifest.json"

REG ADD "HKCU\Software\Google\Chrome\NativeMessagingHosts\com.3721tools.hurl" /ve /t REG_SZ /d "$PATH_TO_NMHOST" /f
