using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Spt.Core.Models;

public class PasswordChangeRequest : LoginRequest
{
    [Required]
    [JsonPropertyName("change")]
    public string Change
    {
        get;
        set;
    } = "";
}
