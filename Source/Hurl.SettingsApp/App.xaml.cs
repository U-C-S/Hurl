using Microsoft.UI.Xaml;
using System.Diagnostics;

namespace Hurl.SettingsApp
{
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            processArgs(args.Arguments.Split(' '));
            m_window.Activate();
        }

        private Window m_window;

        void processArgs(string[] args)
        {
            if(args.Length > 0)
            {
                var primaryArg = args[0];

                if (primaryArg.StartsWith("--newrule"))
                {
                    if(args.Length >= 2)
                    {
                        var ruleURL = args[1];
                        Debug.WriteLine($"Creating new rule for {ruleURL}");
                        m_window.Title = $"Hurl Settings - New Rule for {ruleURL}";
                    }
                }
            }
        }
    }
}
