using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using Spt.Core.Enums;
using Spt.Core.Forge;
using Spt.Core.Responses;

namespace Spt.Backend;

public class ForgeHelper
{
    private bool _internetAccess = false;
    private string? _forgeToken;

    private LogHelper _logHelper;
    private ConfigHelper _configHelper;

    private HttpClient? _httpClient;

    public ForgeHelper
    (
        LogHelper logHelper,
        ConfigHelper configHelper
    )
    {
        _logHelper = logHelper;
        _configHelper = configHelper;

        _httpClient = new HttpClient();
        _forgeToken = _configHelper.GetConfig().ApiKey;
    }

    private NameValueCollection GetParamsCollection(string search, string sort, bool? featured)
    {
        NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
        queryString.Add("include", "users,versions,license");
        queryString.Add("filter[name]", $"*{search}*");
        if (featured is not null)
        {
            queryString.Add("filter[featured]", featured.ToString());
        }
        queryString.Add("sort", sort);
        return queryString;
    }

    public async Task<ForgeModsResponse> GetModsFromForge(CancellationToken token, string search = "", string sort = "-featured,name", int page = 1, string? includeFeatured = null)
    {
        _logHelper.LogInfo($"forge GetModsFromForge");

        if (string.IsNullOrWhiteSpace(_configHelper.GetConfig().ApiKey))
        {
            _logHelper.LogInfo("GetMods - API Key is missing.");
            return null;
        }

        var paramsToUse = GetParamsCollection(search, sort, ConvertFeaturedToBool(includeFeatured));
        var message = new HttpRequestMessage(HttpMethod.Get, $"https://forge.sp-tarkov.com/api/v0/mods?page={page}&{paramsToUse.ToString()}")
        {
            Content = new StringContent("", Encoding.UTF8, "application/json")
        };

        message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _forgeToken);

        var task = await _httpClient?.SendAsync(message, token);
        return JsonSerializer.Deserialize<ForgeModsResponse>(await task.Content.ReadAsStringAsync(token));
    }

    public async Task<bool> LogoutOfForge(CancellationToken token)
    {
        _logHelper.LogInfo($"Forge LogoutOfForge");
        var task = await _httpClient?.DeleteAsync("https://forge.sp-tarkov.com/api/logout", token);
        var result = JsonSerializer.Deserialize<ForgeLoginResponse>(await task.Content.ReadAsByteArrayAsync(token));

        if (result is null)
        {
            _logHelper.LogInfo($"Forge LogoutOfForge result is null");
            return false;
        }

        if (result.Message == ForgeResponseMessage.Unauthenticated)
        {
            _logHelper.AddLog($"Forge LogoutOfForge failed to authenticate");
            return false;
        }

        if (result.Message == ForgeResponseMessage.InvalidCredentials)
        {
            _logHelper.AddLog($"Forge LogoutOfForge failed to invalid-credentials");
            return false;
        }

        if (result.Message == ForgeResponseMessage.Success)
        {
            _logHelper.AddLog($"Forge LogoutOfForge Successful");
            // remove api key
            // remove lastRefreshTime
            return true;
        }

        return false;
    }

    public async Task<bool> LoginToForge(object request, CancellationToken token)
    {
        _logHelper.LogInfo($"Forge LoginToForge");
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var message = new HttpRequestMessage(HttpMethod.Post, "https://forge.sp-tarkov.com/api/login")
        {
            Content = content
        };

        message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var task = await _httpClient?.SendAsync(message, token);
        var result = JsonSerializer.Deserialize<ForgeLoginResponse>(await task.Content.ReadAsStringAsync(token));

        if (result is null)
        {
            _logHelper.AddLog($"Forge LoginToForge result is null");
            return false;
        }

        if (result.Message == ForgeResponseMessage.Unauthenticated)
        {
            _logHelper.AddLog($"Forge LoginToForge failed to authenticate");
            return false;
        }

        if (result.Message == ForgeResponseMessage.InvalidEmail)
        {
            _logHelper.AddLog($"Forge LoginToForge failed due to invalid email");
            return false;
        }

        if (result.Message == ForgeResponseMessage.InvalidCredentials)
        {
            _logHelper.AddLog($"Forge LoginToForge failed to invalid credentials");
            return false;
        }

        if (result.Message == ForgeResponseMessage.Authenticated)
        {
            _logHelper.AddLog($"Forge LoginToForge Authenticated");
            _forgeToken = result.Data.Token;
            _configHelper.SetApiKey(_forgeToken);
            return true;
        }

        return false;
    }

    public bool IsInternetAccessAvailable()
    {
        try
        {
            using (var ping = new Ping())
            {
                var result = ping.Send("8.8.8.8", 1000); // Google's DNS server
                _internetAccess = (result.Status == IPStatus.Success);
            }
        }
        catch
        {
            _internetAccess = false;
        }

        _logHelper.LogInfo($"IsInternetAccessAvailable: {_internetAccess}");
        return _internetAccess;
    }

    public bool GetApiKey()
    {
        return !string.IsNullOrEmpty(_forgeToken);
    }

    public void ClearApiKey()
    {
        _forgeToken = "";
        _configHelper.SetApiKey(_forgeToken);
    }

    private bool? ConvertFeaturedToBool(string selected)
    {
        switch (selected.ToLower())
        {
            case "include":
                return null;
            case "exclude":
                return false;
            case "only":
                return true;
            default:
                return null;
        }
    }
}
