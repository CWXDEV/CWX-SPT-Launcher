using System.Text.Json.Serialization;

namespace Spt.Core.Forge;

public class ModVersions
{
    [JsonPropertyName("data")]
    public ModData Data { get; set; }

    [JsonPropertyName("links")]
    public ModLinks Links { get; set; }
}
