namespace SPTarkov.Core.Helpers;

public class NavigationHelper
{
    public NavigationHelper(
        ConfigHelper configHelper
    )
    {
        if (configHelper.GetConfig().DebugSettings.DebugUser &&
            configHelper.GetConfig().DebugSettings.ShowLoggingPage
           )
        {
            SetLoggingPages(true);
        }
    }

    public bool ShowProfilesPage { get; set; }

    public bool ShowProfilePage { get; set; }

    public bool ShowModPage { get; set; }

    public bool ShowLoggingPage { get; set; }

    public void SetBasicPages(bool state)
    {
        ShowProfilesPage = state;
        ShowModPage = state;
        NotifyStateChanged();
    }

    public void SetProfilePages(bool state)
    {
        ShowProfilePage = state;
        NotifyStateChanged();
    }

    public void SetLoggingPages(bool state)
    {
        ShowLoggingPage = state;
        NotifyStateChanged();
    }

    public event Action? OnStateChanged;

    private void NotifyStateChanged()
    {
        OnStateChanged?.Invoke();
    }
}
