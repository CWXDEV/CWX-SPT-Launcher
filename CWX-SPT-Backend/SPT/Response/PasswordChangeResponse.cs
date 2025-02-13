namespace CWX_SPT_Launcher_Backend.SPT.Response;

public class PasswordChangeResponse : ISptResponse<bool>
{
    public List<ServerProfile> Profiles
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
