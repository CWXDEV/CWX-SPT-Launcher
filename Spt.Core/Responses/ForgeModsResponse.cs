using System.Text.Json.Serialization;
using Spt.Core.Forge;
using Spt.Core.Spt;

namespace Spt.Core.Responses;

public class ForgeModsResponse
{
    [JsonPropertyName("data")]
    public List<ForgeMod>? Data { get; set; }

    [JsonPropertyName("links")]
    public object? Links { get; set; }

    [JsonPropertyName("meta")]
    public object? Meta { get; set; }
}
