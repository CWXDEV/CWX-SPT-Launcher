using CWX_SPT_Frontend.Helpers;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;

namespace CWX_SPT_Frontend;

public partial class Main : Form
{
    private readonly SettingsHelper _settingsHelper;
    private bool closeWhenIMeanIt;
    private static Form MainForm;
    
    public Main()
    {
        _settingsHelper = SettingsHelper.Instance;
        
        SetUpNotificationIcon();
        InitializeComponent(_settingsHelper.GetSettings());
        SetLocationToName();
        SetUpBlazorWebView();
        
        this.FormClosing += Main_FormClosing;
        MainForm = this;
    }
    
    private void SetUpNotificationIcon()
    {
        this.notifyIcon = new NotifyIcon();
        // this.notifyIcon.Icon = new Icon(Path.Combine("Resources", "app.ico"));
        this.notifyIcon.Visible = true;
        this.notifyIcon.Text = "CWX-SPT-LAUNCHER";
        this.notifyIcon.MouseClick += NotifyIconOnClick;
        this.notifyIcon.ContextMenuStrip = new ContextMenuStrip();
        this.notifyIcon.ContextMenuStrip.Items.Add("Open", null, (s, e) =>
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        });
        this.notifyIcon.ContextMenuStrip.Items.Add("Hide", null, (s, e) =>
        {
            this.Hide();
            this.WindowState = FormWindowState.Minimized;
        });
        this.notifyIcon.ContextMenuStrip.Items.Add("Exit", null, (s, e) =>
        {
            closeWhenIMeanIt = true;
            Application.Exit();
        });
    }
    
    private void SetLocationToName()
    {
        // DebugTitleTask = Task.Factory.StartNew(async () =>
        // {
        //     var historicLocationPoint = new Point(0, 0);
        //     var historicSize = new Size(0, 0);
        //
        //     while (true)
        //     {
        //         await Task.Delay(3000);
        //         var location = this.Location;
        //         var size = this.Size;
        //
        //         if (location == historicLocationPoint && size == historicSize)
        //         {
        //             continue;
        //         }
        //
        //         historicLocationPoint = location;
        //         historicSize = size;
        //         this.Text = $"LOCATION: {historicLocationPoint.ToString()}, SIZE: {historicSize.ToString()}";
        //     }
        // });
    }
    
    private void NotifyIconOnClick(object sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left)
        {
            return;
        }
            
        this.Show();
        this.WindowState = FormWindowState.Normal;
    }

    private void Main_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (!_settingsHelper.GetSettings().AppSettings.CloseToTray || closeWhenIMeanIt)
        {
            return;
        }
        
        e.Cancel = true;
        this.Hide();
        this.WindowState = FormWindowState.Minimized;
    }
    
    private void SetUpBlazorWebView()
    {
        var services = new ServiceCollection();
        services.AddWindowsFormsBlazorWebView();
        services.AddBlazorWebViewDeveloperTools();
        services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
            config.SnackbarConfiguration.PreventDuplicates = false;
            config.SnackbarConfiguration.VisibleStateDuration = 2000;
            config.SnackbarConfiguration.ShowTransitionDuration = 100;
            config.SnackbarConfiguration.HideTransitionDuration = 100;
        });

        var blazorWebView = new EmbeddedBlazorWebView()
        {
            UseEmbeddedResources = true,
            Dock = DockStyle.Fill,
            HostPage = "index.html",
            Services = services.BuildServiceProvider(),
        };
        
        blazorWebView.RootComponents.Add<BlazorApp>("#app");
        Controls.Add(blazorWebView);
    }
}