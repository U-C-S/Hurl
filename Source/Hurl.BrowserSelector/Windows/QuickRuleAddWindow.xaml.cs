using Hurl.BrowserSelector.Globals;
using System.Collections.Generic;
using System.Windows;
using Wpf.Ui.Controls;

namespace Hurl.BrowserSelector.Windows
{
    /// <summary>
    /// Interaction logic for QuickRuleAddWindow.xaml
    /// </summary>
    public partial class QuickRuleAddWindow : FluentWindow
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
            var selectedBrowser = TargetBrowser.SelectedValue;
            var rules = RuleInput.Text;
            if (selectedBrowser == null || string.IsNullOrEmpty(rules))
            {
                WarnText.Visibility = Visibility.Visible;
                WarnText.Height = 20;
                return;
            };

            var rulesList = new List<string>();

            foreach (var rule in rules.Split('|'))
            {
                rulesList.Add(rule.Trim());
            };

            SettingsGlobal.AddBrowserRule(rulesList, TargetBrowser.SelectedValue.ToString());

            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) => this.Close();
    }
}
