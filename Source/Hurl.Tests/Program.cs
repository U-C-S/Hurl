using Hurl.App.Helpers;
using Hurl.Library.Models;
using System.Xml.Linq;

const string family = "TheBrowserCompany.Dia_ttt1ap7aakyb4";
const string url = "https://example.test/a?one=1&two=2#fragment";
var package = new PackagedBrowser(family, null, ["http", "https"]);
var browser = new Browser("Dia", "Dia.exe");
var processes = new List<(string Path, string Args)>();
var activations = new List<(Uri Uri, string Family)>();
int resolutions = 0;
var completion = new TaskCompletionSource<bool>();
var launcher = new BrowserLauncher(
    _ => { resolutions++; return package; },
    (uri, target) => { activations.Add((uri, target)); return completion.Task; },
    (path, args) => processes.Add((path, args)));

Task pending = launcher.LaunchAsync(url, browser, null);
Check(!pending.IsCompleted, "Launch must await protocol activation");
Check(activations.Single() == (new Uri(url), family), "Dia must receive the full URL and package family");
Check(processes.Count == 0, "Dia alias must not be started as a process");
completion.SetResult(true);
await pending;
browser.LaunchArgs = "";
await launcher.LaunchAsync(url, browser, null);
Check(activations.Count == 2, "Empty default arguments must also use package activation");

browser.LaunchArgs = "--profile=work %URL%";
await launcher.LaunchAsync(url, browser, null);
Check(processes.Last() == ("Dia.exe", "--profile=work " + url), "Custom URL templates must stay unchanged");
browser.LaunchArgs = "--private";
await launcher.LaunchAsync(url, browser, null);
Check(processes.Last().Args == url + " --private", "Custom args without a template must stay unchanged");
Check(resolutions == 2, "Custom arguments must bypass package resolution");
browser.LaunchArgs = " ";
await launcher.LaunchAsync(url, browser, null);
Check(processes.Last().Args == url + "  ", "Whitespace arguments must retain executable launch behavior");
Check(resolutions == 2, "Whitespace arguments must bypass package resolution");

var alternate = new AlternateLaunch("Private", "");
browser.AlternateLaunches = [alternate];
await launcher.LaunchAsync(url, browser, alternate.Id);
Check(processes.Last().Args == url + " ", "Even empty alternate arguments must use the executable");
alternate.LaunchArgs = "--incognito %URL%";
await launcher.LaunchAsync(url, browser, alternate.Id);
Check(processes.Last().Args == "--incognito " + url, "Alternate URL templates must stay unchanged");
Check(resolutions == 2, "Alternate launches must bypass package resolution");
launcher.LaunchAlternate(url, browser, alternate);
Check(processes.Last().Args == "--incognito " + url, "Selector alternate launches must retain URL templates");
Check(resolutions == 2, "Selector alternate launches must bypass package resolution");
await ThrowsAsync(() => launcher.LaunchAsync(url, browser, Guid.NewGuid()), "Missing alternate must fail");

browser.LaunchArgs = null;
var ordinary = new BrowserLauncher(_ => null, (_, _) => throw new Exception("Unexpected activation"),
    (path, args) => processes.Add((path, args)));
await ordinary.LaunchAsync(url, browser, null);
Check(processes.Last() == ("Dia.exe", url + " "), "Ordinary browsers must retain process launch behavior");
await launcher.LaunchAsync("file:///C:/test.html", browser, null);
Check(processes.Last().Args == "file:///C:/test.html ", "Undeclared protocols must use the executable");

var failed = new BrowserLauncher(_ => package, (_, _) => Task.FromResult(false),
    (_, _) => throw new Exception("Must not fall back to the broken alias"));
await ThrowsAsync(() => failed.LaunchAsync(url, browser, null), "Rejected protocol activation must fail");

string directory = Path.Combine(Path.GetTempPath(), "Hurl.Tests-" + Guid.NewGuid().ToString("N"));
Directory.CreateDirectory(Path.Combine(directory, "assets"));
try
{
    string logo = Path.Combine(directory, "assets", "AppList.targetsize-256.png");
    File.WriteAllBytes(logo, [1]);
    var manifest = XDocument.Parse("""
        <Package xmlns="http://schemas.microsoft.com/appx/manifest/foundation/windows10"
                 xmlns:uap="http://schemas.microsoft.com/appx/manifest/uap/windows10"
                 xmlns:uap3="http://schemas.microsoft.com/appx/manifest/uap/windows10/3"
                 xmlns:uap5="http://schemas.microsoft.com/appx/manifest/uap/windows10/5">
          <Applications>
            <Application Id="Other">
              <uap:VisualElements Square44x44Logo="assets\Wrong.png" />
              <Extensions><uap:Extension Category="windows.protocol"><uap:Protocol Name="file" /></uap:Extension></Extensions>
            </Application>
            <Application Id="Dia">
              <uap:VisualElements Square44x44Logo="assets\AppList.png" />
              <Extensions>
                <uap3:Extension Category="windows.appExecutionAlias">
                  <uap3:AppExecutionAlias><uap5:ExecutionAlias Alias="Dia.exe" /></uap3:AppExecutionAlias>
                </uap3:Extension>
                <uap:Extension Category="windows.protocol"><uap:Protocol Name="https" /></uap:Extension>
                <uap:Extension Category="windows.protocol"><uap:Protocol Name="http" /></uap:Extension>
              </Extensions>
            </Application>
          </Applications>
        </Package>
        """);
    var resolved = PackagedBrowser.FromManifest(manifest, directory, family, "dia.EXE");
    Check(resolved?.FamilyName == family, "Alias matching must be case insensitive");
    Check(resolved?.LogoPath == logo, "Zero-icon alias must resolve the qualified package logo");
    Check(resolved!.Protocols.SetEquals(["http", "https"]), "Protocols must belong to the alias's application");
    Check(PackagedBrowser.FromManifest(manifest, directory, family, "Chrome.exe") is null,
        "Unrelated aliases must not match");
    File.Delete(logo);
    Check(PackagedBrowser.FromManifest(manifest, directory, family, "Dia.exe")?.LogoPath is null,
        "Missing package assets must not create invalid icon paths");
    string scaled = Path.Combine(directory, "assets", "AppList.scale-200.png");
    File.WriteAllBytes(scaled, [1]);
    Check(PackagedBrowser.FromManifest(manifest, directory, family, "Dia.exe")?.LogoPath == scaled,
        "Scale-qualified logos must work when target-size assets are absent");
    string unplated = Path.Combine(directory, "assets", "AppList.targetsize-256_altform-unplated.png");
    File.WriteAllBytes(unplated, [1]);
    Check(PackagedBrowser.FromManifest(manifest, directory, family, "Dia.exe")?.LogoPath == unplated,
        "Unplated target-size logos must take priority");
    manifest.Descendants().Single(x => x.Name.LocalName == "VisualElements"
        && x.Attribute("Square44x44Logo")?.Value == @"assets\AppList.png")
        .SetAttributeValue("Square44x44Logo", @"..\outside.png");
    Check(PackagedBrowser.FromManifest(manifest, directory, family, "Dia.exe")?.LogoPath is null,
        "Logo paths must stay inside the package");
}
finally
{
    Directory.Delete(directory, recursive: true);
}

Console.WriteLine("All packaged-browser regression checks passed.");

static void Check(bool condition, string message)
{
    if (!condition) throw new Exception(message);
}

static async Task ThrowsAsync(Func<Task> action, string message)
{
    try { await action(); }
    catch (InvalidOperationException) { return; }
    throw new Exception(message);
}
