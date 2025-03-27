using System.Text.Json.Serialization;
using Spt.Core.Forge.Meta;

namespace Spt.Core.Forge;

public record ForgeBase
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("attributes")]
    public ModAttributes? Attributes { get; set; }

    [JsonPropertyName("relationships")]
    public Dictionary<string, object>? Relationships { get; set; }

    [JsonPropertyName("includes")]
    public ForgeModIncludes? Includes { get; set; }

    [JsonPropertyName("links")]
    public ModLinks? Links { get; set; }
}
