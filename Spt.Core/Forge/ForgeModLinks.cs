using System.Text.Json.Serialization;

namespace Spt.Core.Forge;

public record ModLinks
{
    [JsonPropertyName("self")]
    public string? Self { get; set; }
}
