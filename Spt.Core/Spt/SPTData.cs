using System.Text.Json.Serialization;

namespace Spt.Core.Spt;

public class SPTData
{
    [JsonPropertyName("version")]
    public string Version
    {
        get;
        set;
    } = "";
}
