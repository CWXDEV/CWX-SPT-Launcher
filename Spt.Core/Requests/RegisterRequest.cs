using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Spt.Core.Requests;

public class RegisterRequest : LoginRequest
{
    [Required]
    [JsonPropertyName("edition")]
    public string Edition
    {
        get;
        set;
    } = "";
}
