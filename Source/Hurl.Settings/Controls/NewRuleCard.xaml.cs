using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Hurl.Settings.Controls
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
