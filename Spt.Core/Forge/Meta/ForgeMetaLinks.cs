using System.Text.Json.Serialization;

namespace Spt.Core.Forge.Meta;

public class ForgeMetaLinks
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("label")]
    public string? Label { get; set; }

    [JsonPropertyName("active")]
    public bool Active { get; set; }
}
