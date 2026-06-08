namespace Hurl.App.Services.Interfaces;

public interface IQuickViewService
{
    bool IsQuickViewEnabled { get; }

    bool TryOpen(string? url);

    bool TryOpenIfModifierKeyActivated(string? url);
}
