using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Hurl.SettingsApp.Controls
{
    public sealed partial class NewRuleCard : UserControl
    {
        public NewRuleCard()
        {
            this.InitializeComponent();
        }

        public string ConstructRule()
        {
            var ruleType = RuleTypeControl.SelectedValue;
            var ruleValue = RuleValueControl.Text;

            if (ruleType == null || string.IsNullOrEmpty(ruleValue))
            {
                return string.Empty;
            }

            return ruleType switch
            {
                "regex" => $"r${ruleValue}",
                "glob" => $"g${ruleValue}",
                _ => $"s${ruleValue}",
            };
        }
    }
}
