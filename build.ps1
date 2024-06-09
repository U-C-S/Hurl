$OUTPUT_PATH = "./_Publish"
$INNO_SETUP_COMPILER = "C:/Program Files (x86)/Inno Setup 6/ISCC.exe"

Write-Output "Building Launcher...."
cd ./Source/Launcher
cargo build --release
cd ../..

Write-Output "Building Hurl...."
dotnet publish -c Release -r win-x64 --no-self-contained --output $OUTPUT_PATH ./Hurl.sln

Write-Output "Building Installer...."
& $INNO_SETUP_COMPILER ./Utils/installer.iss