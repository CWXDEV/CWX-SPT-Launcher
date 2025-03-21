using Spt.Core.Spt;

namespace Spt.Core.Responses;

public class PasswordChangeResponse : ISptResponse<bool>
{
    public List<MiniProfile> Profiles
    {
        get;
        set;
    } = [];

    public bool Response
    {
        get;
        set;
    }
}
