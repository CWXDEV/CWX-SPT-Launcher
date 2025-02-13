namespace CWX_SPT_Launcher_Backend.SPT.Response;

public class ProfilesResponse : ISptResponse<List<ServerProfile>>
{
    public List<ServerProfile> Response
    {
        get;
        set;
    } = [];
}
