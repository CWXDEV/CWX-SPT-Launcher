using System.Text.Json.Serialization;
using Spt.Core.Spt;

namespace Spt.Core.Responses;

public class ForgeLogoutResponse
{
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("status")]
    public int? Status { get; set; }

    [JsonPropertyName("data")]
    public ForgeResponseData? Data { get; set; }
}
