using Hurl.Library.Models;
using System.Threading.Tasks;

namespace Hurl.App.Services.Interfaces;

public interface ISettingsService
{
    Task<Settings> LoadSettingsAsync();

    Settings LoadSettings();

    Task SaveSettingsAsync(Settings settings);
}
