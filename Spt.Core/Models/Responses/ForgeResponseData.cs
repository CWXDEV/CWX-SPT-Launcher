using System.Text.Json.Serialization;

namespace Spt.Core.Models;

public class ForgeResponseData
{
    [JsonPropertyName("token")]
    public string? Token { get; set; }
}
