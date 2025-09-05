using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Spt.Core.Models;

public class LoginRequest
{
    [Required]
    [JsonPropertyName("username")]
    public string Username { get; set; } = "";

    [Required]
    [JsonPropertyName("password")]
    public string Password { get; set; } = "";
}
