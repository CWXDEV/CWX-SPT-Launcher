namespace CWX_SPT_Launcher_Backend.Helpers;

public class NavigationHelper
{
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

    private bool _showAdminPage;
    public bool ShowAdminPage
    {
        get => _showAdminPage;
        set => _showAdminPage = value;
    }

    public void SetBasicPages(bool state)
    {
        ShowProfilePage = state;
        ShowModPage = state;
        NotifyStateChanged();
    }

    public void SetAdminPages(bool state)
    {
        ShowAdminPage = state;
        NotifyStateChanged();
    }

    public event Action? OnStateChanged;
    private void NotifyStateChanged() => OnStateChanged?.Invoke();
}
