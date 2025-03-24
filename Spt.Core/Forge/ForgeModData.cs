using System.Text.Json.Serialization;

namespace Spt.Core.Forge;

public class ModData
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }
}
