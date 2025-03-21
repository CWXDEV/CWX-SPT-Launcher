namespace Spt.Backend;

public class Navigation
{
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

    private bool _showAdminPage;
    public bool ShowAdminPage
    {
        get => _showAdminPage;
        set => _showAdminPage = value;
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

    public void SetAdminPages(bool state)
    {
        ShowAdminPage = state;
        NotifyStateChanged();
    }

    public event Action? OnStateChanged;
    private void NotifyStateChanged() => OnStateChanged?.Invoke();
}
