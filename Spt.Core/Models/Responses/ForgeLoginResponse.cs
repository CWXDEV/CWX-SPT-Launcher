using System.Text.Json.Serialization;

namespace Spt.Core.Models;

public class ForgeLoginResponse
{
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("status")]
    public int? Status { get; set; }

    [JsonPropertyName("data")]
    public ForgeResponseData? Data { get; set; }
}
