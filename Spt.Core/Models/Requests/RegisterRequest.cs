using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Spt.Core.Models;

public class RegisterRequest : LoginRequest
{
    [Required]
    [JsonPropertyName("edition")]
    public string Edition { get; set; } = "";
}
