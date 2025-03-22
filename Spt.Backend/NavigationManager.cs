namespace Spt.Backend;

public class NavigationManager
{
    public NavigationManager(
        ConfigManager configManager
    )
    {
        if (configManager.GetConfig().DebugSettings.DebugUser &&
            configManager.GetConfig().DebugSettings.ShowLoggingPage
        )
        {
            SetLoggingPages(true);
        }
    }

    private bool _showProfilesPage;

    public bool ShowProfilesPage
    {
        get => _showProfilesPage;
        set => _showProfilesPage = value;
    }

    private bool _showProfilePage;

    public bool ShowProfilePage
    {
        get => _showProfilePage;
        set => _showProfilePage = value;
    }

    private bool _showModPage;

    public bool ShowModPage
    {
        get => _showModPage;
        set => _showModPage = value;
    }

    private bool _showLoggingPage;

    public bool ShowLoggingPage
    {
        get => _showLoggingPage;
        set => _showLoggingPage = value;
    }

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
    private void NotifyStateChanged() => OnStateChanged?.Invoke();
}
