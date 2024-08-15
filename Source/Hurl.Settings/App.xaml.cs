using Microsoft.UI.Xaml;
using System;
using System.Diagnostics;

namespace Hurl.Settings;

public partial class App : Application
{
    public App()
    {
        this.InitializeComponent();
    }

    private MainWindow m_window;

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var cmdArgs = Environment.GetCommandLineArgs();

        m_window = new MainWindow();

        if (cmdArgs.Length > 1)
        {
            ProcessArgs(cmdArgs);
        }
        else
        {
            m_window.Activate();
        }
    }


    void ProcessArgs(string[] args)
    {
        Debug.WriteLine(args[0]);
        var primaryArg = args[1];

        if (primaryArg.Equals("--page"))
        {
            if (args.Length > 2)
            {
                m_window.NavigateToPage(args[2]);
                m_window.Activate();
            }
        }
    }
}
