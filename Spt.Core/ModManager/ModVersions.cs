using System.Text.Json.Serialization;

namespace SPT_Core.ModManager;

public class ModVersions
{
    [JsonPropertyName("data")]
    public ModData Data { get; set; }

    [JsonPropertyName("links")]
    public ModLinks Links { get; set; }
}
