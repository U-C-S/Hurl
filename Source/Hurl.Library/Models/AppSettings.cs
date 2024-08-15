namespace Hurl.Library.Models;

public class AppSettings
{
    public bool LaunchUnderMouse { get; set; } = false;

    public bool NoWhiteBorder { get; set; } = false;

    public string BackgroundType { get; set; } = "mica";

    public bool RuleMatching { get; set; } = false;

    public int[] WindowSize { get; set; } = [420, 210];
}


public enum BackgroundMaterial
{
    Acrylic,
    Mica,
    Solid
}
