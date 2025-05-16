using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using Spt.Core.Models;

namespace Spt.Core.Helpers;

public class ForgeHelper
{
    private LogHelper _logHelper;
    private ConfigHelper _configHelper;
    private HttpHelper _httpHelper;

    public ForgeHelper
    (
        LogHelper logHelper,
        ConfigHelper configHelper,
        HttpHelper httpHelper
    )
    {
        _logHelper = logHelper;
        _configHelper = configHelper;
        _httpHelper = httpHelper;
    }

    public async Task<ForgeModsResponse> GetModsFromForge(CancellationToken token, string search = "", string sort = "-featured,name", int page = 1, string? includeFeatured = null)
    {
        return new ForgeModsResponse();
    }
}
