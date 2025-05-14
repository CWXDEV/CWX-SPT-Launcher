using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using ComponentAce.Compression.Libs.zlib;
using Spt.Core.App;
using Spt.Core.Spt;
using Spt.Core.Responses;

namespace Spt.Backend;

public class StateHelper
{
    public Servers? SelectedServer;
    public MiniProfile? SelectedProfile;
    public Dictionary<string, SPTMod> ModList = [];
    public List<MiniProfile> ProfileList = [];
    public Dictionary<string, string> ProfileTypes = new();
    private readonly LogHelper _logHelper;
    public int? CurrentPagination;

    public StateHelper(
        LogHelper logHelper
    )
    {
        _logHelper = logHelper;
    }

    public void LogoutAndDispose()
    {
        _logHelper.LogInfo($"Logged out of server {(SelectedServer?.Ip ?? "Unknown")} and disposed");
        ProfileTypes = new Dictionary<string, string>();
        ProfileList = [];
        ModList = [];
        SelectedProfile = null;
        SelectedServer = null;
    }

    public void SetSelectedServer(Servers server)
    {
        SelectedServer = server;
    }

    public void SetSelectedProfile(MiniProfile miniProfile)
    {
        SelectedProfile = miniProfile;
    }
}
