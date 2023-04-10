using Hurl.BrowserSelector.Globals;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;

namespace Hurl.BrowserSelector.Windows
{
    /// <summary>
    /// Interaction logic for QuickRuleAddWindow.xaml
    /// </summary>
    public partial class QuickRuleAddWindow
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
            var rules = new List<string>();
            rules.Add(RuleInput.Text);

            SettingsGlobal.AddBrowserRule(rules, TargetBrowser.SelectedValue.ToString());
        }
    }
}
