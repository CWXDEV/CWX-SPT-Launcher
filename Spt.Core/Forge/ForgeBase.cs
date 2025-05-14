using System.Text.Json.Serialization;
using Spt.Core.Forge.Meta;

namespace Spt.Core.Forge;

public record ForgeBase
{
    [JsonPropertyName("id")]
    public int? Id
    {
        get;
        set;
    }

    [JsonPropertyName("hub_id")]
    public int? HubId
    {
        get;
        set;
    }

    [JsonPropertyName("name")]
    public string? Name
    {
        get;
        set;
    }

    [JsonPropertyName("slug")]
    public string? Slug
    {
        get;
        set;
    }

    [JsonPropertyName("teaser")]
    public string? Teaser
    {
        get;
        set;
    }

    [JsonPropertyName("thumbnail")]
    public string? Thumbnail
    {
        get;
        set;
    }

    [JsonPropertyName("downloads")]
    public int? Downloads
    {
        get;
        set;
    }

    [JsonPropertyName("description")]
    public string? Description
    {
        get;
        set;
    }

    [JsonPropertyName("source_code_link")]
    public string? SourceCodeLink
    {
        get;
        set;
    }

    [JsonPropertyName("featured")]
    public bool? Featured
    {
        get;
        set;
    }

    [JsonPropertyName("contains_ads")]
    public bool? ContainsAds
    {
        get;
        set;
    }

    [JsonPropertyName("contains_ai_content")]
    public bool? ContainsAiContent
    {
        get;
        set;
    }

    [JsonPropertyName("published_at")]
    public string? PublishedAt
    {
        get;
        set;
    }

    [JsonPropertyName("created_at")]
    public string? CreatedAt
    {
        get;
        set;
    }

    [JsonPropertyName("updated_at")]
    public string? UpdatedAt
    {
        get;
        set;
    }

    [JsonPropertyName("owner")]
    public ForgeUser? Owner
    {
        get;
        set;
    }

    [JsonPropertyName("authors")]
    public List<ForgeUser>? Authors
    {
        get;
        set;
    }

    [JsonPropertyName("versions")]
    public List<ForgeModVersion>? Versions
    {
        get;
        set;
    }

    [JsonPropertyName("license")]
    public ForgeLicense? License
    {
        get;
        set;
    }
}
