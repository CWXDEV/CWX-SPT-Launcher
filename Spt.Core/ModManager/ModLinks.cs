using System.Text.Json.Serialization;

namespace Spt.Core.ModManager;

public record ModLinks
{
    [JsonPropertyName("self")]
    public string? Self { get; set; }
}
