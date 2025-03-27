using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using ComponentAce.Compression.Libs.zlib;
using Spt.Core.Enums;
using Spt.Core.Responses;

namespace Spt.Backend;

public class HttpHelper
{
    private readonly HttpClient _httpClient;
    private readonly ConfigHelper _configHelper;
    private readonly LogHelper _logHelper;
    private readonly StateHelper _stateHelper;
    private string _token;
    private bool _internetAccess = false;

    public HttpHelper(
        ConfigHelper configHelper,
        LogHelper logHelper,
        StateHelper stateHelper
    )
    {
        _configHelper = configHelper;
        _logHelper = logHelper;
        _stateHelper = stateHelper;

        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = CertificateValidationCallback;
        _httpClient = new HttpClient(handler);
        _httpClient.DefaultRequestVersion = new Version(3, 0);
        _httpClient.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;

        _token = configHelper.GetConfig().ApiKey;
    }

    private bool CertificateValidationCallback(
        HttpRequestMessage httpRequestMessage,
        X509Certificate2? x509Certificate2,
        X509Chain? x509Chain,
        SslPolicyErrors sslPolicyErrors
    )
    {
        return true;
    }

    private string BuildGameUrl(string url)
    {
        return "https://" + _stateHelper.SelectedServer.Ip + url;
    }

    public async Task<T> GameServerGet<T>(string url, CancellationToken token)
    {
        _logHelper.LogInfo($"GET: {url}");
        var task = await _httpClient?.GetAsync(BuildGameUrl(url), token);

        return JsonSerializer.Deserialize<T>(
            SimpleZlib.Decompress(
                await task.Content.ReadAsByteArrayAsync(token)
            )
        );
    }

    public async Task<T> GameServerPut<T>(string url, object request, CancellationToken token)
    {
        _logHelper.LogInfo($"Put: {url}");

        var content = new ByteArrayContent(
            SimpleZlib.CompressToBytes(
                JsonSerializer.Serialize(request)
                , zlibConst.Z_BEST_COMPRESSION
            )
        );

        var task = await _httpClient?.PutAsync(BuildGameUrl(url), content, token);

        return JsonSerializer.Deserialize<T>(
            SimpleZlib.Decompress(
                await task.Content.ReadAsByteArrayAsync(token)
            )
        );
    }

    public async Task<ForgeModsResponse> ForgeMods(
        CancellationToken token,
        string search = "",
        string sort = "-featured,name",
        int page = 1,
        string? includeFeatured = null
    )
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
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

        var task = await _httpClient?.SendAsync(message, token);
        return JsonSerializer.Deserialize<ForgeModsResponse>(await task.Content.ReadAsStringAsync(token));
    }

    public async Task<ForgeLogoutResponse> ForgeLogout(CancellationToken token)
    {
        _logHelper.LogInfo($"Forge ForgeLogout");

        if (string.IsNullOrWhiteSpace(_configHelper.GetConfig().ApiKey))
        {
            _logHelper.LogInfo("GetMods - API Key is missing.");
            return null;
        }

        var message = new HttpRequestMessage(HttpMethod.Delete, "https://forge.sp-tarkov.com/api/logout")
        {
            Content = new StringContent("", Encoding.UTF8, "application/json")
        };

        message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

        var task = await _httpClient?.SendAsync(message, token);
        return JsonSerializer.Deserialize<ForgeLogoutResponse>(await task.Content.ReadAsStringAsync(token));
    }

    public async Task<ForgeLoginResponse> ForgeLogin(object request, CancellationToken token)
    {
        _logHelper.LogInfo($"Forge ForgeLogin");

        var message = new HttpRequestMessage(HttpMethod.Post, "https://forge.sp-tarkov.com/api/login")
        {
            Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
        };

        message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var task = await _httpClient?.SendAsync(message, token);
        return JsonSerializer.Deserialize<ForgeLoginResponse>(await task.Content.ReadAsStringAsync(token));
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

    public string GetApiKey()
    {
        return _token;
    }

    public void ClearApiKey()
    {
        _token = "";
        _configHelper.SetApiKey(_token);
    }

    public void SetApiKey(string apiKey)
    {
        _token = apiKey;
        _configHelper.SetApiKey(_token);
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
}
