namespace Spt.Core.Responses;

public class TypesResponse : ISptResponse<Dictionary<string, string>>
{
    public Dictionary<string, string> Response
    {
        get;
        set;
    } = new();
}
