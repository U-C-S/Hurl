using Hurl.Library.Models;
using System.IO;

namespace Hurl.Library;

public class SettingsFile
{
    public Settings SettingsObject;

    private SettingsFile(Settings settings)
    {
        this.SettingsObject = settings;
    }

    public static SettingsFile TryLoading()
    {
        return new SettingsFile(GetSettings());
    }

    public static Settings GetSettings()
    {
        try
        {
            return JsonOperations.FromJsonToModel<Settings>(Constants.APP_SETTINGS_MAIN);
        }
        catch (Exception e)
        {
            switch (e)
            {
                case FileNotFoundException _:
                case DirectoryNotFoundException _:
                    return New(GetBrowsers.FromRegistry()).SettingsObject;
                default:
                    throw;
            }
        }
    }

    public static SettingsFile New(List<Browser> browsers)
    {
        Directory.CreateDirectory(Constants.APP_SETTINGS_DIR);

        var _settings = new Settings()
        {
            Browsers = browsers,
        };

        JsonOperations.FromModelToJson(_settings, Constants.APP_SETTINGS_MAIN);
        return new SettingsFile(_settings);
    }

    public void Update()
    {
        SettingsObject.LastUpdated = DateTime.Now.ToString();
        JsonOperations.FromModelToJson(SettingsObject, Constants.APP_SETTINGS_MAIN);
    }
}
