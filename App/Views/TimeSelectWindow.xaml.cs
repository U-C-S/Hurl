using Hurl.BrowserSelector.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Hurl.BrowserSelector.Views
{
    /// <summary>
    /// Interaction logic for TimeSelectWindow.xaml
    /// </summary>
    public partial class TimeSelectWindow : Wpf.Ui.Controls.UiWindow
    {
        private List<Browser> browsers;

        public TimeSelectWindow(List<Browser> x)
        {
            InitializeComponent();
            browsers = x;
            DataContext = x.ConvertAll(y => y.Name);
        }

        private void StackPanel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => this.DragMove();

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var x = TimeBox.Value;
            var y = BrowserBox.SelectedIndex;

            if (y != -1)
            {
                var dataObject = new TemporaryDefaultBrowser()
                {
                    TargetBrowser = browsers[y],
                    SelectedAt = DateTime.Now,
                    ValidTill = DateTime.Now.AddMinutes(x)
                };
                string jsondata = JsonSerializer.Serialize(dataObject, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    IncludeFields = true
                });
                File.WriteAllText(Path.Combine(Constants.APP_SETTINGS_DIR, "TempDefault.json"), jsondata);

                this.Close();
            }
        }
    }
}
