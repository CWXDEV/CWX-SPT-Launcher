using System.Text.Json.Serialization;

namespace Spt.Core.Models;

public class ForgeModIncludes
{
    [JsonPropertyName("users")]
    public List<ForgeBase> Users { get; set; }

    [JsonPropertyName("license")]
    public ForgeBase License { get; set; }

    [JsonPropertyName("versions")]
    public List<ForgeBase> Versions { get; set; }
}
