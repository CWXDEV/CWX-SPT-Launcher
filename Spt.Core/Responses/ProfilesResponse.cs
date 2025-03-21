using Spt.Core.Spt;

namespace Spt.Core.Responses;

public class ProfilesResponse : ISptResponse<List<MiniProfile>>
{
    public List<MiniProfile> Response
    {
        get;
        set;
    } = [];
}
