using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Appearance;

namespace Hurl.RulesetManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            //Loaded += (sender, args) => SystemThemeWatcher.Watch(this, Wpf.Ui.Controls.WindowBackdropType.Mica, true);
        }

        //public QuickRuleAddWindow()
        //{
        //    InitializeComponent();

        //    var x = SettingsGlobal.GetBrowserNameList();
        //    x.Add("_Hurl");
        //    DataContext = x;
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedBrowser = TargetBrowser.SelectedValue;
            var rules = "RuleInput.Text";
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

            //SettingsGlobal.AddBrowserRule(rulesList, TargetBrowser.SelectedValue.ToString());

            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) => this.Close();
    }



}