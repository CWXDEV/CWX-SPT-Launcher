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
    private bool _internetAccess = true; // implement checking for internet access
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

    public async Task<bool> GetMods<T>(string url, CancellationToken token)
    {
        _logHelper.LogInfo($"forge GetMods: {url}");

        if (string.IsNullOrWhiteSpace(_configHelper.GetConfig().ApiKey))
        {
            _logHelper.LogInfo("GetMods - API Key is missing.");
            return false;
        }

        var task = await _httpClient?.GetAsync(url, token);
        var result = JsonSerializer.Deserialize<T>(await task.Content.ReadAsByteArrayAsync(token));

        switch (result)
        {
            case List<ForgeMod> modList:
                return true;
                break;
            case ForgeMod mod:
                return true;
                break;
            default:
                return false;
        }
    }

    public async Task<bool> LogoutOfForge(string url, CancellationToken token)
    {
        _logHelper.LogInfo($"Forge LogoutOfForge: {url}");
        var task = await _httpClient?.DeleteAsync(url, token);
        var result = JsonSerializer.Deserialize<ForgeLoginResponse>(await task.Content.ReadAsByteArrayAsync(token));

        if (result is null)
        {
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
            // remove api key
            // remove lastRefreshTime
            return true;
        }

        return false;
    }

    public async Task<bool> LoginToForge(string url, object request, CancellationToken token)
    {
        try
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
                _forgeToken = $"Bearer {result.Data.Token}";
                _configHelper.SetApiKey(_forgeToken);
                _lastRefreshTime = DateTime.Now;
                return true;
            }

            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
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

    public List<ForgeMod> GetFeaturedMods()
    {
        return new List<ForgeMod>();
    }

    public List<ForgeMod> GetNewMods()
    {
        return new List<ForgeMod>();
    }

    public List<ForgeMod> GetRecentlyUpdatedMods()
    {
        return new List<ForgeMod>();
    }

    public ForgeMod? GetMod(string modId)
    {
        return new ForgeMod();
    }

    public List<ForgeMod> GetCachedMods()
    {
        return new List<ForgeMod>();
    }
}
