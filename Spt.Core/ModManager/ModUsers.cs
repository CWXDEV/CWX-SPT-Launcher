using System.Text.Json.Serialization;

namespace Spt.Core.ModManager;

public class ModUsers
{
    [JsonPropertyName("data")]
    public ModData Data { get; set; }

    [JsonPropertyName("links")]
    public ModLinks Links { get; set; }
}
