using Microsoft.UI.Xaml;
using System;
using System.Diagnostics;

namespace Hurl.Settings
{
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            var cmdArgs = Environment.GetCommandLineArgs();

            m_window = new MainWindow();

            if (cmdArgs.Length > 1)
            {
                ProcessArgs(cmdArgs);
                m_window.Activate();
            }
            else
            {
                m_window.Activate();
            }

        }

        private Window m_window;

        void ProcessArgs(string[] args)
        {
            Debug.WriteLine(args[0]);
            var primaryArg = args[1];

            if (primaryArg.StartsWith("--newrule"))
            {
                if (args.Length >= 2)
                {
                    var ruleURL = args[2];
                    Debug.WriteLine($"------------------------> Creating new rule for {ruleURL}");
                }
            }

        }
    }
}
