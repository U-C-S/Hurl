using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Hurl.BrowserSelector.Services;

/// <summary>
/// A robust Named Pipe server that maintains overlapping server instances
/// to eliminate race conditions. Always has at least one pipe ready to accept connections.
/// </summary>
internal sealed class NamedPipeUrlReceiver : IAsyncDisposable
{
    private const string PipeName = "HurlNamedPipe";
    private const int MaxServerInstances = 4;
    private const int TargetListenerCount = 2;

    private readonly CancellationTokenSource _cts = new();
    private readonly Action<string[]> _onUrlReceived;
    private readonly PipeSecurity _pipeSecurity;

    private int _activeListeners = 0;

    public NamedPipeUrlReceiver(Action<string[]> onUrlReceived)
    {
        _onUrlReceived = onUrlReceived ?? throw new ArgumentNullException(nameof(onUrlReceived));

        _pipeSecurity = new PipeSecurity();
        _pipeSecurity.AddAccessRule(new PipeAccessRule(
            new SecurityIdentifier(WellKnownSidType.WorldSid, null),
            PipeAccessRights.ReadWrite,
            AccessControlType.Allow));
    }

    public void Start()
    {
        for (int i = 0; i < TargetListenerCount; i++)
        {
            _ = StartListenerAsync();
        }
    }

    private async Task StartListenerAsync()
    {
        Interlocked.Increment(ref _activeListeners);

        try
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                NamedPipeServerStream? pipeServer = null;

                try
                {
                    pipeServer = NamedPipeServerStreamAcl.Create(
                        PipeName,
                        PipeDirection.InOut,
                        MaxServerInstances,
                        PipeTransmissionMode.Byte,
                        PipeOptions.Asynchronous | PipeOptions.WriteThrough,
                        inBufferSize: 4096,
                        outBufferSize: 4096,
                        _pipeSecurity);

                    try
                    {
                        await pipeServer.WaitForConnectionAsync(_cts.Token).ConfigureAwait(false);
                    }
                    catch (IOException ex) when (ex.HResult == -2147024664) // ERROR_PIPE_CONNECTED
                    {
                        // Client connected before WaitForConnectionAsync was called - connection is already established
                    }

                    // Spawn replacement listener IMMEDIATELY before processing
                    EnsureMinimumListeners();

                    await ProcessConnectionAsync(pipeServer).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Pipe listener error: {ex.Message}");
                    try
                    {
                        await Task.Delay(100, _cts.Token).ConfigureAwait(false);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                }
                finally
                {
                    pipeServer?.Dispose();
                }
            }
        }
        finally
        {
            Interlocked.Decrement(ref _activeListeners);
        }
    }

    private void EnsureMinimumListeners()
    {
        int current = Volatile.Read(ref _activeListeners);
        // Spawn a replacement if we're at or below target, ensuring always have listeners ready
        if (current <= TargetListenerCount && !_cts.Token.IsCancellationRequested)
        {
            _ = StartListenerAsync();
        }
    }

    private async Task ProcessConnectionAsync(NamedPipeServerStream pipeServer)
    {
        try
        {
            using var reader = new StreamReader(pipeServer, leaveOpen: true);

            using var readCts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token);
            readCts.CancelAfter(TimeSpan.FromSeconds(5));

            string data = await reader.ReadToEndAsync(readCts.Token).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(data))
            {
                string[]? args = JsonSerializer.Deserialize<string[]>(data);
                if (args != null && args.Length > 0)
                {
                    _onUrlReceived(args);
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Timeout or shutdown
        }
        catch (JsonException ex)
        {
            Debug.WriteLine($"Failed to parse pipe data: {ex.Message}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error processing pipe connection: {ex.Message}");
        }
    }

    public async ValueTask DisposeAsync()
    {
        _cts.Cancel();

        int maxWaitIterations = 50;
        while (Volatile.Read(ref _activeListeners) > 0 && maxWaitIterations-- > 0)
        {
            await Task.Delay(100).ConfigureAwait(false);
        }

        _cts.Dispose();
    }
}
