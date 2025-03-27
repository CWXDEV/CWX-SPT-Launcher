using System.Text.Json.Serialization;
using Spt.Core.Forge;
using Spt.Core.Forge.Links;
using Spt.Core.Forge.Meta;
using Spt.Core.Spt;

namespace Spt.Core.Responses;

public class ForgeModsResponse
{
    [JsonPropertyName("data")]
    public List<ForgeBase>? Data { get; set; }

    [JsonPropertyName("links")]
    public ForgeLinks? Links { get; set; }

    [JsonPropertyName("meta")]
    public ForgeMeta? Meta { get; set; }
}
