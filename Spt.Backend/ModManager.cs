using Spt.Core.ModManager;

namespace Spt.Backend;

public class ModManager
{
    private HttpClient? _httpClient;
    private string? _apiToken = "Bearer FCfzYkguSm8mt3GryR9wBt4n3yedfT4MQjrb0zSo9b05792c";
    private string? _forgeAddress;
    private Logger _logger;

    public ModManager
    (
        Logger logger
    )
    {
        _logger = logger;
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://forge.sp-tarkov.com/api/v0/");
        _httpClient.DefaultRequestHeaders.Add("Authorization", _apiToken);
        _httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public List<Mod> GetFeaturedMods()
    {
        return new List<Mod>();
    }

    public List<Mod> GetNewMods()
    {
        return new List<Mod>();
    }

    public List<Mod> GetRecentlyUpdatedMods()
    {
        return new List<Mod>();
    }
}
