using System.Text.Json.Serialization;

namespace Spt.Core.ModManager;

public class ModData
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }
}
