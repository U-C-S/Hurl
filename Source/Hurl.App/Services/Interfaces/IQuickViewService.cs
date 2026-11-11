using System.Threading.Tasks;

namespace Hurl.App.Services.Interfaces;

public interface IQuickViewService
{
    bool IsQuickViewEnabled { get; }

    Task<bool> TryOpenAsync(string? url);

    Task<bool> TryOpenIfModifierKeyActivatedAsync(string? url);
}
