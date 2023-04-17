using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;

namespace Hurl.Settings
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Wpf.Ui.Controls.UiWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = Globals.SettingsGlobal.Value.AppSettings;
        }

        public Dictionary<object, object> Pages = new();

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            object x = (sender as Button).Tag;
            if (Pages.ContainsKey(x))
            {
                ContentBox.Child = (System.Windows.UIElement)Pages[x];
                Debug.WriteLine("Used existing instance");
            }
            else
            {
                object instance = Activator.CreateInstance((System.Type)x);
                Pages.Add(x, instance);
                ContentBox.Child = (System.Windows.UIElement)instance;
                Debug.WriteLine($"New instance for the key: {x}");
            }
        }
    }
}
