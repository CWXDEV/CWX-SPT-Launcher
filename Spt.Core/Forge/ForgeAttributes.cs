using System.Text.Json.Serialization;

namespace Spt.Core.Forge;

public record ModAttributes
{
    [JsonPropertyName("hub_id")]
    public int? HubId { get; set; }

    [JsonPropertyName("mod_id")]
    public int? ModId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    // Not really needed
    [JsonPropertyName("slug")]
    public string? Slug { get; set; }

    [JsonPropertyName("teaser")]
    public string? Teaser { get; set; }

    // No idea what that is
    [JsonPropertyName("license_id")]
    public int? LicenseId { get; set; }

    [JsonPropertyName("source_code_link")]
    public string? SourceCodeLink { get; set; }

    [JsonPropertyName("featured")]
    public bool? Featured { get; set; }

    [JsonPropertyName("contains_ai_content")]
    public bool? ContainsAiContent { get; set; }

    [JsonPropertyName("contains_ads")]
    public bool? ContainsAds { get; set; }

    [JsonPropertyName("created_at")]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public string? UpdatedAt { get; set; }

    [JsonPropertyName("published_at")]
    public string? PublishedAt { get; set; }

    [JsonPropertyName("user_role_id")]
    public int? UserRoleId { get; set; }

    [JsonPropertyName("virus_total_link")]
    public string? VirusTotalLink { get; set; }

    [JsonPropertyName("downloads")]
    public int? Downloads { get; set; }

    [JsonPropertyName("link")]
    public string? Link { get; set; }
}
