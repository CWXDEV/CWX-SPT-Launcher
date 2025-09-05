using Spt.Core.Models;

namespace Spt.Core.Helpers;

public class StateHelper
{
    private readonly LogHelper _logHelper;
    public int? CurrentPagination;
    public Dictionary<string, SPTMod> ModList = [];
    public List<MiniProfile> ProfileList = [];
    public Dictionary<string, string> ProfileTypes = new();
    public MiniProfile? SelectedProfile;
    public Server? SelectedServer;

    public StateHelper(
        LogHelper logHelper
    )
    {
        _logHelper = logHelper;
    }

    public void LogoutAndDispose()
    {
        _logHelper.LogInfo($"Logged out of server {SelectedServer?.IpAddress ?? "Unknown"} and disposed");
        ProfileTypes = new Dictionary<string, string>();
        ProfileList = [];
        ModList = [];
        SelectedProfile = null;
        SelectedServer = null;
    }

    public void SetSelectedServer(Server server)
    {
        SelectedServer = server;
    }

    public void SetSelectedProfile(MiniProfile miniProfile)
    {
        SelectedProfile = miniProfile;
    }
}
