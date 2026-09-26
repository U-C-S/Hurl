using Hurl.Library.Models;

namespace Hurl.App.Services.Interfaces;

public interface IAppStateService
{
    AppState LoadState();
    void UpdateTransientDefault(TransientDefaultConfig? config);
}
