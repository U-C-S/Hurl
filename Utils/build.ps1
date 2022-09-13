$OUTPUT_PATH = "./App/bin/publish"
$INNO_SETUP_COMPILER = "C:/Program Files (x86)/Inno Setup 6/ISCC.exe"

dotnet publish -c Release -r win-x64 --no-self-contained --output $OUTPUT_PATH ./Hurl.sln

Write-Output "Building Installer...."

& $INNO_SETUP_COMPILER ./Utils/installer.iss