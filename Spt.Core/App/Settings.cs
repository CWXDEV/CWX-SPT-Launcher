using System.ComponentModel.DataAnnotations;

namespace Spt.Core.App;

public class Settings
{
    public bool FirstRun
    {
        get;
        set;
    }

    [Required]
    public string GamePath
    {
        get;
        set;
    }

    public AppSettings AppSettings
    {
        get;
        set;
    }

    public DebugSettings DebugSettings
    {
        get;
        set;
    }

    public List<Servers> Servers
    {
        get;
        set;
    }

    public string ApiKey
    {
        get;
        set;
    }
}
