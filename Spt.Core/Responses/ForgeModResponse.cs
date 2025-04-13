using System.Text.Json.Serialization;
using Spt.Core.Forge;

namespace Spt.Core.Responses;

public class ForgeModResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("data")]
    public ForgeBase? Data { get; set; }
}
