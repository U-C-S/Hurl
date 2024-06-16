$EXTENSION_ID = $args[0]

# Custom path to the directory. Absolute path is required.
$CURRENT_DIR = $args[1]
if ($CURRENT_DIR -eq $null) {
  $CURRENT_DIR = Get-Location
}
$CURRENT_DIR = "$CURRENT_DIR".Replace('\', '/')

$PATH_TO_NMHOST = "$CURRENT_DIR/nmh-manifest.json"
Write-Output "Creating Native Messaging Host manifest file at $PATH_TO_NMHOST"


$NMH_MANIFEST = @"
{
  "name": "com.3721tools.hurl",
  "description": "Hurl Proxy Native Messaging Host",
  "path": "$CURRENT_DIR/Launcher.exe",
  "type": "stdio",
  "allowed_origins": ["chrome-extension://$EXTENSION_ID/"]
}
"@

# $FIREFOX_NMH_MANIFEST = @"
# {
#   "name": "com.3721tools.hurl",
#   "description": "Hurl Proxy Native Messaging Host",
#   "path": "$CURRENT_DIR/Launcher.exe",
#   "type": "stdio",
#   "allowed_extensions": ["chrome-extension://$EXTENSION_ID/"]
# }
# "@

$NMH_MANIFEST | Out-File -FilePath $PATH_TO_NMHOST

$REG_PATH = "HKCU\Software\Google\Chrome\NativeMessagingHosts"
# $FIREFOX_REG_PATH = "HKCU\Software\Mozilla\NativeMessagingHosts"

REG ADD "$REG_PATH\com.3721tools.hurl" /ve /t REG_SZ /d "$PATH_TO_NMHOST" /f
