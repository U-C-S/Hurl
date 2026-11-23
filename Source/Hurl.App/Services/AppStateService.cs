using Hurl.App.Services.Interfaces;
using Hurl.Library;
using Hurl.Library.Models;
using Hurl.Library.Serialization;
using Hurl.Library.Storage;
using System.Diagnostics;
using System.Text.Json;

namespace Hurl.App.Services;

public sealed class AppStateService : IAppStateService
{
    private readonly JsonFileStore<AppState> store;
    private AppState? state;

    public AppStateService(string? statePath = null)
    {
        store = new JsonFileStore<AppState>(
            statePath ?? Constants.APP_STATE_MAIN,
            SelectorJsonSerializerContext.Default.AppState);
    }

    public AppState LoadState()
    {
        if (state is not null)
        {
            return state;
        }

        try
        {
            state = store.Read() ?? new AppState();
        }
        catch (JsonException ex)
        {
            Debug.WriteLine($"Could not read AppState.json; using empty state: {ex.Message}");
            state = new AppState();
        }

        return state;
    }

    public void UpdateTransientDefault(TransientDefaultConfig? config)
    {
        AppState currentState = LoadState();
        TransientDefaultConfig? previous = currentState.TransientDefaultConfig;
        currentState.TransientDefaultConfig = config;

        try
        {
            store.Write(currentState);
        }
        catch
        {
            // Keep the shared state consistent with the last successful write.
            currentState.TransientDefaultConfig = previous;
            throw;
        }
    }
}
