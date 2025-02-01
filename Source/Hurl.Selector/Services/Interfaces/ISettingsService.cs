using Hurl.Selector.Models;
using System.Threading.Tasks;

namespace Hurl.Selector.Services.Interfaces;

public interface ISettingsService
{
    Task<Settings> LoadSettingsAsync();

    Task SaveSettingsAsync(Settings settings);
}
