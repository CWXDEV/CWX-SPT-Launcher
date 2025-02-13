namespace CWX_SPT_Launcher_Backend.SPT.Response;

public class VersionResponse : ISptResponse<SPTVersion>
{
    public SPTVersion Response
    {
        get;
        set;
    } = new();
}
