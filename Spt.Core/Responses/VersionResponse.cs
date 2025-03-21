using Spt.Core.Spt;

namespace Spt.Core.Responses;

public class VersionResponse : ISptResponse<SPTVersion>
{
    public SPTVersion Response
    {
        get;
        set;
    } = new();
}
