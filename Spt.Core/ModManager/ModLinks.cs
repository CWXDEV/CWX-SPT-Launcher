using System.Text.Json.Serialization;

namespace SPT_Core.ModManager;

public record ModLinks
{
    [JsonPropertyName("self")]
    public string? Self { get; set; }
}
