$OUTPUT_PATH = "./_Publish"
$INNO_SETUP_COMPILER = "C:/Program Files (x86)/Inno Setup 6/ISCC.exe"

Write-Output "Building NativeMessagingHost...."
cargo build --release --workspace

Write-Output "Building Hurl...."
dotnet restore

dotnet publish .\Source\Hurl.Settings\Hurl.Settings.csproj -c Release -r win-x64 -o .\_Publish
dotnet publish .\Source\Hurl.Selector\Hurl.Selector.csproj -c Release -r win-x64 -o .\_Publish

Write-Output "Building Installer...."
& $INNO_SETUP_COMPILER ./Utils/installer.iss
