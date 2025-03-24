using System.Text.Json.Serialization;

namespace Spt.Core.Spt;

public class ForgeResponseData
{
    [JsonPropertyName("token")]
    public string? Token { get; set; }
}
