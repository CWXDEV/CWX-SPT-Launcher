using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using MudBlazor;
using MudBlazor.Services;
using Photino.Blazor;
using Spt.Core.Helpers;

namespace Spt.Frontend;

public class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        var appBuilder = PhotinoBlazorAppBuilder.CreateDefault(new ManifestEmbeddedFileProvider(typeof(Program).Assembly, "Resources"), args);

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

        var app = appBuilder.Build();

        // customize window
        app.MainWindow.SetTitle("Photino Blazor Sample");
        app.MainWindow.DevToolsEnabled = true;
        // use this to disable bottom left status bar like in a browser
        app.MainWindow.BrowserControlInitParameters = "--kiosk";

        AppDomain.CurrentDomain.UnhandledException += (sender, error) =>
        {
            app.MainWindow.ShowMessage("Fatal exception", error.ExceptionObject.ToString());
        };

        app.Run();

    }
}
