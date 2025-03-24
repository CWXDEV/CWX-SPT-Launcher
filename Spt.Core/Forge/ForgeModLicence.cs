using System.Text.Json.Serialization;

namespace Spt.Core.Forge;

public class ModLicence
{
    [JsonPropertyName("data")]
    public ModData Data { get; set; }
}
