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
    private int _refreshInterval = 60 * 10; // 10 mins
    private bool _internetAccess = false;
    private string? _forgeAddress = "https://forge.sp-tarkov.com/api";
    private string? _forgeToken;

    private LogHelper _logHelper;
    private ConfigHelper _configHelper;

    private HttpClient? _httpClient;
    private DateTime _lastRefreshTime;
    private Dictionary<string, ForgeMod> _modCache = new();

    public ForgeHelper
    (
        LogHelper logHelper,
        ConfigHelper configHelper
    )
    {
        _logHelper = logHelper;
        _configHelper = configHelper;

        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(_forgeAddress);

        _forgeToken = _configHelper.GetConfig().ApiKey;
    }

    public async Task<bool> GetModsFromPagination(string url, CancellationToken token, bool? includeFeatured = null)
    {
        _logHelper.LogInfo($"forge GetModsFromPagination: {url}");

        if (string.IsNullOrWhiteSpace(_configHelper.GetConfig().ApiKey))
        {
            _logHelper.LogInfo("GetMods - API Key is missing.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(url))
        {
            _logHelper.LogError($"forge GetModsFromPagination: url is null or empty");
            return false;
        }
        NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
        queryString.Add("include", "users,versions,license");
        queryString.Add("filter[name]", $"**");
        if (includeFeatured is not null)
        {
            queryString.Add("filter[featured]", includeFeatured.ToString());
        }
        queryString.Add("sort", "-featured,name");
        var message = new HttpRequestMessage(HttpMethod.Get, url)
        {
            Content = new StringContent("", Encoding.UTF8, "application/json")
        };

        message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _forgeToken);

        var task = await _httpClient?.SendAsync(message, token);
        var result = JsonSerializer.Deserialize<ForgeModsResponse>(await task.Content.ReadAsStringAsync(token));

        Console.WriteLine(result.ToString());

        return true;
    }

    public async Task<bool> GetModsFromForge(string url, string request, CancellationToken token, bool? includeFeatured = null)
    {
        _logHelper.LogInfo($"forge GetMods: {url}");

        if (string.IsNullOrWhiteSpace(_configHelper.GetConfig().ApiKey))
        {
            _logHelper.LogInfo("GetMods - API Key is missing.");
            return false;
        }

        var content = new StringContent("", Encoding.UTF8, "application/json");
        NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
        queryString.Add("include", "users,versions,license");
        queryString.Add("filter[name]", $"*{request}*");
        if (includeFeatured is not null)
        {
            queryString.Add("filter[featured]", includeFeatured.ToString());
        }
        queryString.Add("sort", "-featured,name");

        var message = new HttpRequestMessage(HttpMethod.Get, $"{url}?{queryString.ToString()}")
        {
            Content = content
        };

        message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _forgeToken);

        var task = await _httpClient?.SendAsync(message, token);
        var result = JsonSerializer.Deserialize<ForgeModsResponse>(await task.Content.ReadAsStringAsync(token));

        if (result == null || result.Data == null)
        {
            _logHelper.LogInfo($"forge GetMods result is null");
            return false;
        }

        if (!result.Data.Any())
        {
            _logHelper.LogInfo($"forge GetMods returned no results.");
            return false;
        }

        if (result.Data.Any())
        {
            _logHelper.LogInfo($"forge GetMods returned results.");
            foreach (var mod in result.Data)
            {
                _modCache.TryAdd(mod.Attributes.Name, mod);
            }

            _lastRefreshTime = DateTime.Now;

            return true;
        }

        return false;
    }

    public async Task<bool> LogoutOfForge(string url, CancellationToken token)
    {
        _logHelper.LogInfo($"Forge LogoutOfForge: {url}");
        var task = await _httpClient?.DeleteAsync(url, token);
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

    public async Task<bool> LoginToForge(string url, object request, CancellationToken token)
    {
        _logHelper.LogInfo($"Forge LoginToForge: {url}");
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var message = new HttpRequestMessage(HttpMethod.Post, url)
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
            _lastRefreshTime = DateTime.Now;
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

    public ForgeMod? GetMod(string modId)
    {
        return new ForgeMod();
    }

    public List<ForgeMod> GetMods()
    {
        // request for mods
        // newest
        // featured included

        return new List<ForgeMod>();
    }

    public List<ForgeMod> GetCachedMods()
    {
        // this should only clear and "refresh" the cache after 10 mins
        if (_lastRefreshTime < DateTime.Now - TimeSpan.FromSeconds(_refreshInterval))
        {
            _modCache.Clear();
        }

        return _modCache.Values.ToList();
    }
}
