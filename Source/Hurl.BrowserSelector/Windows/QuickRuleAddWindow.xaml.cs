using Hurl.BrowserSelector.Globals;
using System.Collections.Generic;
using System.Windows;
using Wpf.Ui.Controls;

namespace Hurl.BrowserSelector.Windows
{
    /// <summary>
    /// Interaction logic for QuickRuleAddWindow.xaml
    /// </summary>
    public partial class QuickRuleAddWindow: UiWindow
    {
        public QuickRuleAddWindow()
        {
            InitializeComponent();

            var x = SettingsGlobal.GetBrowserNameList();
            x.Add("_Hurl");
            DataContext = x;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var rules = new List<string> { RuleInput.Text };
            var ruleMode = RuleModeInput.SelectedValue.ToString();

            switch (ruleMode)
            {
                case "System.Windows.Controls.ComboBoxItem: Regex":
                    rules[0] = "r$" + rules[0];
                    break;
                case "System.Windows.Controls.ComboBoxItem: Glob":
                    rules[0] = "g$" + rules[0];
                    break;
                default:
                    break;
            }

            SettingsGlobal.AddBrowserRule(rules, TargetBrowser.SelectedValue.ToString());

            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) => this.Close();
    }
}
