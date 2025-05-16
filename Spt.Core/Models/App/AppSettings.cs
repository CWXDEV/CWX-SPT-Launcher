namespace Spt.Core.Models;

public class AppSettings
{
    public StartLocation StartLocation
    {
        get;
        set;
    }

    public StartSize StartSize
    {
        get;
        set;
    }

    // left panel options
    public bool CloseToTray
    {
        get;
        set;
    }

    public bool MinimizeOnLaunch
    {
        get;
        set;
    }

    public bool AlwaysTop
    {
        get;
        set;
    }

    // Advanced panel options
    public bool AdvancedUser
    {
        get;
        set;
    }
}
