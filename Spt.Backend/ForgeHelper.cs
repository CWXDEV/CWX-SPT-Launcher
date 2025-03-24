using System.Text.Json;
using Spt.Core.Enums;
using Spt.Core.Forge;
using Spt.Core.Responses;

namespace Spt.Backend;

public class ForgeHelper
{
    private HttpClient? _httpClient;
    private string? _apiToken;
    private DateTime _lastRefreshTime;
    private int _refreshInterval = 60 * 10; // 10 mins
    private bool _internetAccess = true; // implement checking for internet access
    private string? _forgeAddress;
    private LogHelper _logHelper;
    private Dictionary<string, ForgeMod> _modCache = new();

    public ForgeHelper
    (
        LogHelper logHelper
    )
    {
        _logHelper = logHelper;
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://forge.sp-tarkov.com/api/v0/");
    }

    // move headers being set depending on the method
    // public void SetupHttpClient(string apiToken)
    // {
    //
    //     _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiToken}");
    //     // _httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
    //     // _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    // }

    public async Task<bool> GetMods<T>(string url, CancellationToken token)
    {
        _logHelper.LogInfo($"forge GetMods: {url}");
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
        _logHelper.LogInfo($"Forge LoginToForge: {url}");
        var content = new StringContent(JsonSerializer.Serialize(request));
        var task = await _httpClient?.PostAsync(url, content, token);
        var result = JsonSerializer.Deserialize<ForgeLoginResponse>(await task.Content.ReadAsByteArrayAsync(token));

        if (result is null)
        {
            return false;
        }

        if (result.Message == ForgeResponseMessage.Unauthenticated)
        {
            _logHelper.AddLog($"Forge LoginToForge failed to authenticate");
            return false;
        }

        if (result.Message == ForgeResponseMessage.InvalidCredentials)
        {
            _logHelper.AddLog($"Forge LoginToForge failed to invalid credentials");
            return false;
        }

        if (result.Message == ForgeResponseMessage.Success)
        {
            // add api key
            // set lastRefreshTime
            return true;
        }

        return false;
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
