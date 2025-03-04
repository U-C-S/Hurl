using Hurl.Library.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Hurl.Settings.Services.Interfaces;

public interface ISettingsService
{
    //Task<Settings> LoadSettingsAsync();

    Task SaveSettingsAsync(Library.Models.Settings settings);

    void UpdateAppSettings(AppSettings appSettings);

    void UpdateBrowsers(ObservableCollection<Browser> browsers);
}
