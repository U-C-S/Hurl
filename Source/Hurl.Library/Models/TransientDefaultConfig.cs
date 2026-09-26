namespace Hurl.Library.Models;

public record TransientBrowser(
    Guid BrowserId,
    Guid? AltLaunchId,
    string Name,
    string ExePath,
    string Arguments);

public class TransientDefaultConfig
{
    public required TransientBrowser Browser { get; set; }
    
    public required DateTime ValidTill { get; set; }

    public DateTime SelectedAt { get; set; }
}
