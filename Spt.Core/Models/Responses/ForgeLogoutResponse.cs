using System.Text.Json.Serialization;

namespace Spt.Core.Models;

public class ForgeLogoutResponse
{
    [JsonPropertyName("message")] public string? Message { get; set; }

    [JsonPropertyName("code")] public string? Code { get; set; }

    [JsonPropertyName("success")] public bool Success { get; set; }

    [JsonPropertyName("data")] public ForgeResponseData? Data { get; set; }
}
