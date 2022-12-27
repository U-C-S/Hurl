using Hurl.Library;
using Hurl.Library.Models;
using System;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace Hurl.BrowserSelector.Views.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public Settings settings = SettingsFile.GetSettings();

        public AppAutoSettings RuntimeSettings
        {
            get
            {
                var path = Path.Combine(Constants.APP_SETTINGS_DIR, "runtime.json");
                try
                {
                    return JsonOperations.FromJsonToModel<AppAutoSettings>(path);

                }
                catch (Exception ex)
                {
                    if (ex is DirectoryNotFoundException)
                    {
                        Directory.CreateDirectory(Constants.APP_SETTINGS_DIR);
                    }
                    if (ex is FileNotFoundException or DirectoryNotFoundException)
                    {
                        var obj = new AppAutoSettings()
                        {
                            WindowSize = new int[] { 350, 200 }
                        };

                        File.WriteAllText(path, JsonSerializer.Serialize(obj));
                        return obj;
                    }
                    else
                    {
                        MessageBox.Show(ex.Message);
                        return null;
                    }
                }
            }
        }

        public BaseViewModel viewModel
        {
            get
            {
                return new BrowserListViewModel(settings);
            }
        }
    }
}
