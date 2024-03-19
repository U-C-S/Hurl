using Hurl.Library;
using Hurl.Library.Models;
using System;
using System.Diagnostics;
using System.IO;

namespace Hurl.BrowserSelector.Helpers
{
    public class TimedBrowserSelect
    {
        public static bool CheckAndLaunch(string url)
        {
            var Path_TempDef = Path.Combine(Constants.APP_SETTINGS_DIR, "TempDefault.json");
            if (File.Exists(Path_TempDef))
            {
                var obj = JsonOperations.FromJsonToModel<TemporaryDefaultBrowser>(Path_TempDef);
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

            JsonOperations.FromModelToJson(dataObject, Path.Combine(Constants.APP_SETTINGS_DIR, "TempDefault.json"));
        }

        public static TemporaryDefaultBrowser? Get()
        {
            var Path_TempDef = Path.Combine(Constants.APP_SETTINGS_DIR, "TempDefault.json");
            if (File.Exists(Path_TempDef))
            {
                var obj = JsonOperations.FromJsonToModel<TemporaryDefaultBrowser>(Path_TempDef);
                if (obj.ValidTill >= DateTime.Now)
                {
                    return obj;
                }
                else
                {
                    File.Delete(Path_TempDef);
                }
            }
            return null;
        }

        internal static void DeleteCurrent()
        {
            var Path_TempDef = Path.Combine(Constants.APP_SETTINGS_DIR, "TempDefault.json");
            if (File.Exists(Path_TempDef))
            {
                File.Delete(Path_TempDef);
            }
        }
    }
}
