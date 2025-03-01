using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using CWX_SPT_Launcher_Backend.Helpers;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using MediaColor = System.Windows.Media.Color;
using Color = System.Drawing.Color;
using Colors = MudBlazor.Colors;

namespace CWX_SPT_Frontend;

public partial class MainWindow
{
    public static Window WindowMain;
    private readonly SettingsHelper _settings;
    private readonly MediaColor _titleBarColor = MediaColor.FromArgb(255, 39, 39, 47);

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
        serviceCollection.AddSingleton<LogHelper>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        Resources.Add("services", serviceProvider);

        _settings = serviceProvider.GetRequiredService<SettingsHelper>();

        InitializeComponent();
        CustomizeComponent();
        CustomizeWindow();
        WindowMain = this;
    }

    private void CustomizeWindow()
    {
        // Defer the DWM call - this is to stop issues happening later regarding to changes to the Window.
        Dispatcher.BeginInvoke(new Action(() =>
        {
            var hWnd = new WindowInteropHelper(this).EnsureHandle();
            // This is the NavBar Background colour.
            int colorRef = _titleBarColor.R | (_titleBarColor.G << 8) | (_titleBarColor.B << 16);

            DwmSetWindowAttribute(
                hWnd,
                DWMWINDOWATTRIBUTE.DWMWA_CAPTION_COLOR,
                ref colorRef,
                Marshal.SizeOf<bool>()
            );
        }), DispatcherPriority.ContextIdle);

        Dispatcher.BeginInvoke(new Action(async () =>
        {
            // Webview and WPF should be init by this point, *hopefully* removing all white flashes
            WindowMain.WindowState = WindowState.Normal;
        }), DispatcherPriority.ContextIdle);
    }

    private void CustomizeComponent()
    {
        BlazorWebViewControl.BlazorWebViewInitialized += (sender, args) =>
        {
            // Do this to mitigate the white flash from webview
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

    protected override void OnClosing(CancelEventArgs e)
    {
        _settings.SetClientLocationSettings((int) Top, (int) Left);
        _settings.SetClientSizeSettings((int) Height, (int) Width);
        _settings.SetFirstRun(false);

        if (_settings.GetSettings().AppSettings.CloseToTray)
        {
            WindowMain.WindowState = WindowState.Minimized;
            e.Cancel = true;
            return;
        }

        base.OnClosing(e);
    }

    [DllImport("Dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attribute, [In] ref int pvAttribute, int cbAttribute);

    private enum DWMWINDOWATTRIBUTE
    {
        DWMWA_CAPTION_COLOR = 35
    }
}
