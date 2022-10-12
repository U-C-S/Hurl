using Hurl.BrowserSelector.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace Hurl.BrowserSelector.Helpers
{
    public class TimedBrowserSelect
    {
        public static bool CheckAndLaunch(string url)
        {
            var Path_TempDef = Path.Combine(Constants.APP_SETTINGS_DIR, "TempDefault.json");
            if (File.Exists(Path_TempDef))
            {
                var tempRead = File.ReadAllText(Path_TempDef);
                var obj = JsonSerializer.Deserialize<TemporaryDefaultBrowser>(tempRead);
                if (obj.ValidTill >= DateTime.Now)
                {
                    Process.Start(obj.TargetBrowser.ExePath, url);
                    Debug.WriteLine(obj.TargetBrowser.ExePath);
                    return true;
                }
                else
                {
                    File.Delete(Path_TempDef);
                }
            }
            return false;
        }

        public static void Create(int mins, Browser browser)
        {
            var dataObject = new TemporaryDefaultBrowser()
            {
                TargetBrowser = browser,
                SelectedAt = DateTime.Now,
                ValidTill = DateTime.Now.AddMinutes(mins)
            };
            string jsondata = JsonSerializer.Serialize(dataObject, new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true
            });
            File.WriteAllText(Path.Combine(Constants.APP_SETTINGS_DIR, "TempDefault.json"), jsondata);
        }
    }
}
