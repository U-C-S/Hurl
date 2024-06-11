$EXTENSION_ID = $args[0]

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
  "path": "$CURRENT_DIR/Laucher.exe",
  "type": "stdio",
  "allowed_origins": ["chrome-extension://$EXTENSION_ID/"]
}
"@

$NMH_MANIFEST | Out-File -FilePath $PATH_TO_NMHOST

REG ADD "HKCU\Software\Google\Chrome\NativeMessagingHosts\com.3721tools.hurl" /ve /t REG_SZ /d "$PATH_TO_NMHOST" /f
