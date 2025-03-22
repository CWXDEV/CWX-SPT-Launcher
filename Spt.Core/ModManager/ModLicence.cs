using System.Text.Json.Serialization;

namespace SPT_Core.ModManager;

public class ModLicence
{
    [JsonPropertyName("data")]
    public ModData Data { get; set; }
}
