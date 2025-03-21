using Spt.Core.Spt;

namespace Spt.Core.Responses;

public class ModsResponse : ISptResponse<Dictionary<string, SPTMod>>
{
    public Dictionary<string, SPTMod> Response
    {
        get;
        set;
    } = new();
}
