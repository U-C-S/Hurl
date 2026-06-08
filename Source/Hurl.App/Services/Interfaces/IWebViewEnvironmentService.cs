using Microsoft.Web.WebView2.Core;
using System.Threading.Tasks;

namespace Hurl.App.Services.Interfaces;

public interface IWebViewEnvironmentService
{
    Task<CoreWebView2Environment> GetEnvironmentAsync();
}
