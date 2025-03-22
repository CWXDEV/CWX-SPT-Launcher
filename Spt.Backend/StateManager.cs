using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using ComponentAce.Compression.Libs.zlib;
using Spt.Core.App;
using Spt.Core.Spt;
using Spt.Core.Responses;

namespace Spt.Backend;

public class StateManager
{
    private HttpClient? _netClient;
    public Servers? ConnectedServer;
    public MiniProfile? SelectedProfile;
    public Dictionary<string, SPTMod> ModList = [];
    public List<MiniProfile> ProfileList = [];
    public Dictionary<string, string> ProfileTypes = new();
    private readonly Logger _logger;

    public StateManager(
        Logger logger
    )
    {
        _logger = logger;
    }

    public async Task<bool> GetAsync<T>(string url, CancellationToken token)
    {
        _logger.LogInfo($"GET: {url}");
        var task = await _netClient?.GetAsync(url, token);
        var result = JsonSerializer.Deserialize<T>(SimpleZlib.Decompress(await task.Content.ReadAsByteArrayAsync(token)));

        switch (result)
        {
            case ProfilesResponse casting1:
                ProfileList = casting1.Response;
                return true;
            case TypesResponse casting2:
                ProfileTypes = casting2.Response;
                return true;
            case ModsResponse casting3:
                ModList = casting3.Response;
                return true;
            case PingResponse casting4:
                return casting4.Response == "Pong!";
            default:
                return false;
        }
    }

    public async Task<bool> PutAsync<T>(string url, object request, CancellationToken token)
    {
        _logger.LogInfo($"POST: {url}");
        var content = new ByteArrayContent(SimpleZlib.CompressToBytes(JsonSerializer.Serialize(request), zlibConst.Z_BEST_COMPRESSION));
        var task = await _netClient?.PutAsync(url, content, token);
        var result = JsonSerializer.Deserialize<T>(SimpleZlib.Decompress(await task.Content.ReadAsByteArrayAsync(token)));

        switch (result)
        {
            case RegisterResponse registerResponse:
                ProfileList = registerResponse.Profiles;
                return registerResponse.Response;
            case RemoveResponse removeResponse:
                ProfileList = removeResponse.Profiles;
                return removeResponse.Response;
            case PasswordChangeResponse passwordChangeResponse:
                ProfileList = passwordChangeResponse.Profiles;
                return passwordChangeResponse.Response;
            case LoginResponse loginResponse:
                return loginResponse.Response;
            default:
                return false;
        }
    }

    public void SetupHttpClient(Servers server)
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = HandlerServerCertificateCustomValidationCallback;

        _netClient = new HttpClient(handler);
        _netClient.DefaultRequestVersion = new Version(3, 0);
        _netClient.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
        _netClient.BaseAddress = new Uri("https://" + server.Ip);
    }

    private bool HandlerServerCertificateCustomValidationCallback(
        HttpRequestMessage httpRequestMessage,
        X509Certificate2? x509Certificate2,
        X509Chain? x509Chain,
        SslPolicyErrors sslPolicyErrors
    )
    {
        return true;
    }

    public void LogoutAndDispose()
    {
        _logger.LogInfo($"Logged out of server {(ConnectedServer?.Ip ?? "Unknown")} and disposed");
        ProfileList = [];
        ModList = [];
        SelectedProfile = null;
        ProfileTypes = new Dictionary<string, string>();
        ConnectedServer = null;
        _netClient = null;
    }

    public void ServerLogin(Servers server)
    {
        ConnectedServer = server;
    }

    public void ProfileLogin(MiniProfile miniProfile)
    {
        SelectedProfile = miniProfile;
    }
}
