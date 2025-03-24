using System.Text.Json.Serialization;

namespace Spt.Core.Forge;

public record ForgeMod
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("attributes")]
    public ModAttributes? Attributes { get; set; }

    [JsonPropertyName("relationships")]
    public Dictionary<string, List<ModRelationship>>? Relationships { get; set; }

    [JsonPropertyName("includes")]
    public List<ForgeMod>? Includes { get; set; }

    [JsonPropertyName("links")]
    public ModLinks? Links { get; set; }
}
