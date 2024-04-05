using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hurl.App.Utils;

internal class SingleInstance
{
    // TAKEN from https://github.com/BartoszCichecki/LenovoLegionToolkit/blob/master/LenovoLegionToolkit.WPF/App.xaml.cs
    // TODO: Support sharing the Cmd Args between the instances
    private const string MUTEX_NAME = "hurl_app_mutex";
    private const string EVENT_NAME = "hurl_app_mutex";

    private Mutex? _singleInstanceMutex;
    private EventWaitHandle? _singleInstanceWaitHandle;

    private void EnsureSingleInstance()
    {
        _singleInstanceMutex = new Mutex(true, MUTEX_NAME, out var isOwned);
        _singleInstanceWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, EVENT_NAME);

        if (!isOwned)
        {
            _singleInstanceWaitHandle.Set();
            Shutdown();
            return;
        }

        new Thread(() =>
        {
            while (_singleInstanceWaitHandle.WaitOne())
            {
                Current.Dispatcher.BeginInvoke(async () =>
                {
                    if (Current.MainWindow is { } window)
                    {
                        window.BringToForeground();
                    }
                    else
                    {
                        await ShutdownAsync();
                    }
                });
            }
        })
        {
            IsBackground = true
        }.Start();
    }
}

