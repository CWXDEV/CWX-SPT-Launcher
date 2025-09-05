using System.Text.Json.Serialization;

namespace Spt.Core.Models;

public class SPTData
{
    [JsonPropertyName("version")] public string Version { get; set; } = "";
}
