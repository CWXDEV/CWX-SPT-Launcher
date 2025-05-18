using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using MudBlazor;
using MudBlazor.Services;
using Photino.Blazor;
using Photino.NET;
using Spt.Core.Helpers;

namespace Spt.Frontend;

public class Program
{
    public static PhotinoBlazorApp App { get; set; }
    public static ManifestEmbeddedFileProvider EmbedProvider { get; set; }
    public static ConfigHelper ConfigHelper { get; set; }

    [STAThread]
    static void Main(string[] args)
    {
        EmbedProvider = new ManifestEmbeddedFileProvider(typeof(Program).Assembly, "Resources");
        var appBuilder = PhotinoBlazorAppBuilder.CreateDefault(EmbedProvider, args);

        appBuilder.Services
            .AddSingleton<ConfigHelper>()
            .AddSingleton<ForgeHelper>()
            .AddSingleton<GameHelper>()
            .AddSingleton<HttpHelper>()
            .AddSingleton<LogHelper>()
            .AddSingleton<ModHelper>()
            .AddSingleton<NavigationHelper>()
            .AddSingleton<PatchHelper>()
            .AddSingleton<StateHelper>()
            .AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
                config.SnackbarConfiguration.PreventDuplicates = false;
                config.SnackbarConfiguration.VisibleStateDuration = 2000;
                config.SnackbarConfiguration.ShowTransitionDuration = 100;
                config.SnackbarConfiguration.HideTransitionDuration = 100;
            });

        // register root component and selector
        appBuilder.RootComponents.Add<App>("app");

        App = appBuilder.Build();
        ConfigHelper = App.Services.GetService<ConfigHelper>();
        var http = App.Services.GetService<HttpHelper>();
        var modLoader = App.Services.GetService<ModHelper>();
        http.IsInternetAccessAvailable();
        _ = modLoader.GetClientMods();
        _ = modLoader.GetServerMods();

        CustomizeComponent();

        AppDomain.CurrentDomain.UnhandledException += (sender, error) =>
        {
            App.MainWindow.ShowMessage("Fatal exception", error.ExceptionObject.ToString());
        };

        App.Run();
    }

    private static void CustomizeComponent()
    {
        // customize window
        App.MainWindow.SetTitle("Spt.LauncherV2");
        App.MainWindow.SetIconFile(EmbedProvider.GetFileInfo("Resources/icon.ico").PhysicalPath);
        App.MainWindow.DevToolsEnabled = true;
        App.MainWindow.LogVerbosity = 0;

        // use this to disable bottom left status bar like in a browser
        // comment out to gain devtools - this flag disables it.
        // App.MainWindow.BrowserControlInitParameters = "--kiosk";

        App.MainWindow.Topmost = ConfigHelper.GetConfig().AppSettings.AlwaysTop;
        App.MainWindow.MinHeight = 550;
        App.MainWindow.MinWidth = 1070;

        if (ConfigHelper.GetConfig().FirstRun)
        {
            App.MainWindow.Width = 1070;
            App.MainWindow.Height = 550;
            App.MainWindow.SetUseOsDefaultLocation(true);
        }
        else
        {
            App.MainWindow.Width = ConfigHelper.GetConfig().AppSettings.StartSize.Width;
            App.MainWindow.Height = ConfigHelper.GetConfig().AppSettings.StartSize.Height;

            App.MainWindow.SetUseOsDefaultLocation(false);

            App.MainWindow.Top = ConfigHelper.GetConfig().AppSettings.StartLocation.X;
            App.MainWindow.Left = ConfigHelper.GetConfig().AppSettings.StartLocation.Y;
        }

        App.MainWindow.RegisterWindowClosingHandler(new PhotinoWindow.NetClosingDelegate(OnExit));
        App.MainWindow.SetMinimized(true);

        App.MainWindow.RegisterWebMessageReceivedHandler((sender, message) =>
        {
            if (message.StartsWith("open-external:"))
            {
                var url = message.Substring("open-external:".Length);
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to open URL: " + ex.Message);
                }
            }
        });
    }

    private static bool OnExit(object sender, EventArgs e)
    {
        ConfigHelper.SetClientLocation(App.MainWindow.Top, App.MainWindow.Left);
        ConfigHelper.SetClientSize(App.MainWindow.Height, App.MainWindow.Width);
        ConfigHelper.SetFirstRun(false);

        if (ConfigHelper.GetConfig().AppSettings.CloseToTray)
        {
            App.MainWindow.SetMinimized(true);
            return true;
        }

        return false;
    }
}
