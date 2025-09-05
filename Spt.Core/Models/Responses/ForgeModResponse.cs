using System.Text.Json.Serialization;

namespace Spt.Core.Models;

public class ForgeModResponse
{
    [JsonPropertyName("success")] public bool Success { get; set; }

    [JsonPropertyName("data")] public ForgeBase? Data { get; set; }
}
