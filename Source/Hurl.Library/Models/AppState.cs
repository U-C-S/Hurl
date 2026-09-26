namespace Hurl.Library.Models;


/// <summary>
/// This defers from the Settings class by being non-user modifiable and a runtime internal state
/// </summary>
public class AppState
{
    public TransientDefaultConfig? TransientDefaultConfig { get; set; }

    // TODO: Move any relevant settings here, like WindowSize
}
