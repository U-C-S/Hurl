using Hurl.Selector.Models;
using System.Threading.Tasks;

namespace Hurl.Selector.Services;

public interface ISettingsService
{
    Task<Settings> LoadSettingsAsync();

    Task SaveSettingsAsync(Settings settings);
}
