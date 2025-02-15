using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using CWX_SPT_Launcher_Backend.Helpers;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using Color = System.Drawing.Color;

namespace CWX_SPT_Frontend;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private static Window WindowMain;
    private readonly SettingsHelper _settings;

    public MainWindow()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddWpfBlazorWebView();
        serviceCollection.AddBlazorWebViewDeveloperTools();
        serviceCollection.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
            config.SnackbarConfiguration.PreventDuplicates = false;
            config.SnackbarConfiguration.VisibleStateDuration = 2000;
            config.SnackbarConfiguration.ShowTransitionDuration = 100;
            config.SnackbarConfiguration.HideTransitionDuration = 100;
        });
        serviceCollection.AddSingleton<PatchHelper>();
        serviceCollection.AddSingleton<ServerHelper>();
        serviceCollection.AddSingleton<SettingsHelper>();
        serviceCollection.AddSingleton<NavigationHelper>();
        serviceCollection.AddSingleton<GameHelper>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        Resources.Add("services", serviceProvider);

        _settings = serviceProvider.GetRequiredService<SettingsHelper>();

        InitializeComponent();
        CustomizeComponent();

        var hWnd = new WindowInteropHelper(this).EnsureHandle();
        var value = true;
        DwmSetWindowAttribute(
            hWnd,
            DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE,
            ref value,
            Marshal.SizeOf<bool>());

        WindowMain = this;
    }

    private void CustomizeComponent()
    {
        BlazorWebViewControl.BlazorWebViewInitialized += (sender, args) =>
        {
            args.WebView.DefaultBackgroundColor = Color.FromArgb(255, 50, 51, 61);
        };

        Topmost = _settings.GetSettings().AppSettings.AlwaysTop;

        if (_settings.GetSettings().FirstRun)
        {
            Width = MinWidth;
            Height = MinHeight;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        else
        {
            Width = _settings.GetSettings().AppSettings.StartSize.Width;
            Height = _settings.GetSettings().AppSettings.StartSize.Height;

            WindowStartupLocation = WindowStartupLocation.Manual;

            Top = _settings.GetSettings().AppSettings.StartLocation.X;
            Left = _settings.GetSettings().AppSettings.StartLocation.Y;
        }
    }

    public static void ChangeTopMostSetting(bool setting)
    {
        WindowMain.Topmost = setting;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        _settings.SetClientLocationSettings((int) Top, (int) Left);
        _settings.SetClientSizeSettings((int) Height, (int) Width);
        _settings.SetFirstRun(false);
        base.OnClosing(e);
    }

    [DllImport("Dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(
        IntPtr hwnd,
        DWMWINDOWATTRIBUTE attribute,
        [In] ref bool pvAttribute,
        int cbAttribute);

    private enum DWMWINDOWATTRIBUTE
    {
        DWMWA_USE_IMMERSIVE_DARK_MODE = 20
    }
}
