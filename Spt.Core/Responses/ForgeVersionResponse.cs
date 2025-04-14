using System.Text.Json.Serialization;
using Spt.Core.Forge;
using Spt.Core.Forge.Links;
using Spt.Core.Forge.Meta;

namespace Spt.Core.Responses;

public class ForgeVersionResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("data")]
    public List<ForgeModVersion>? Data { get; set; }

    [JsonPropertyName("links")]
    public ForgeLinks? Links { get; set; }

    [JsonPropertyName("meta")]
    public ForgeMeta? Meta { get; set; }
}
