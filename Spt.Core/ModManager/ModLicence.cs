using System.Text.Json.Serialization;

namespace Spt.Core.ModManager;

public class ModLicence
{
    [JsonPropertyName("data")]
    public ModData Data { get; set; }
}
