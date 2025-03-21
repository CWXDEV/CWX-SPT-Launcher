using System.Text.Json;
using Spt.Core.App;
using MudBlazor;

namespace Spt.Backend;

public class ConfigManager
{
    private static readonly string AppPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "CWX-SPT-Launcher\\Resources");

    public readonly DialogOptions DialogOptions = new()
    {
        Position = DialogPosition.Center,
        MaxWidth = MaxWidth.ExtraSmall,
        BackdropClick = true,
        CloseOnEscapeKey = true,
        NoHeader = true,
        FullWidth = true,
        BackgroundClass = "dialog-backdrop-class"
    };

    private Settings? _settings;
    private Lock _lock = new Lock();
    private Logger? _logger;

    public ConfigManager()
    {
        LoadSettingsFromFile();
    }

    public ConfigManager(
        Logger logger
    )
    {
        _logger = logger;
        LoadSettingsFromFile();
    }

    private void LoadSettingsFromFile()
    {
        lock (_lock)
        {
            _logger.LogInfo("LoadSettingsFromFile...");

            // check if exists
            if (!File.Exists(Path.Combine(AppPath, "settings.json")))
            {
                SaveDefaults();
            }

            // if not save
            _settings = JsonSerializer.Deserialize<Settings>(
                File.ReadAllText(Path.Combine(AppPath, "settings.json")));
        }
    }

    public Settings GetConfig()
    {
        lock (_lock)
        {
            _logger.LogInfo("GetConfig...");
            return _settings;
        }
    }

    public void SaveConfig()
    {
        lock (_lock)
        {
            _logger.LogInfo("SaveConfig...");
            File.WriteAllText(Path.Combine(AppPath, "settings.json"), JsonSerializer.Serialize(_settings));
        }
    }

    public void SetClientSize(int height, int width)
    {
        lock (_lock)
        {
            _logger.LogInfo("SetClientSize...");
            _settings.AppSettings.StartSize.Height = height;
            _settings.AppSettings.StartSize.Width = width;
            SaveConfig();
        }
    }

    public void SetClientLocation(int x, int y)
    {
        lock (_lock)
        {
            _logger.LogInfo("SetClientLocation...");
            _settings.AppSettings.StartLocation.X = x;
            _settings.AppSettings.StartLocation.Y = y;
            SaveConfig();
        }
    }

    public void SetFirstRun(bool firstRun)
    {
        lock (_lock)
        {
            _logger.LogInfo("SetFirstRun...");
            _settings.FirstRun = firstRun;
            SaveConfig();
        }
    }

    public void SetServers(List<Servers> servers)
    {
        lock (_lock)
        {
            _logger.LogInfo("SetServers...");
            _settings.Servers = servers;
            SaveConfig();
        }
    }

    public void SetCloseToTray(bool closeToTray)
    {
        lock (_lock)
        {
            _logger.LogInfo("SetCloseToTray...");
            _settings.AppSettings.CloseToTray = closeToTray;
            SaveConfig();
        }
    }

    public void SetMinimizeOnLaunch(bool minimizeOnLaunch)
    {
        lock (_lock)
        {
            _logger.LogInfo("SetMinimizeOnLaunch...");
            _settings.AppSettings.MinimizeOnLaunch = minimizeOnLaunch;
            SaveConfig();
        }
    }

    public void SetAlwaysOnTop(bool alwaysOnTop)
    {
        lock (_lock)
        {
            _logger.LogInfo("SetAlwaysOnTop...");
            _settings.AppSettings.AlwaysTop = alwaysOnTop;
            SaveConfig();
        }
    }

    public void SetAdvancedUser(bool advancedUser)
    {
        lock (_lock)
        {
            _logger.LogInfo("SetAdvancedUser...");
            _settings.AppSettings.AdvancedUser = advancedUser;
            SaveConfig();
        }
    }

    public void SetDebugUser(bool debugUser)
    {
        lock (_lock)
        {
            _logger.LogInfo("SetDebugUser...");
            _settings.DebugSettings.DebugUser = debugUser;
            SaveConfig();
        }
    }

    public void SetUseProfileColors(bool profileColors)
    {
        lock (_lock)
        {
            _logger.LogInfo("SetUseProfileColors...");
            _settings.AppSettings.UseProfileColors = profileColors;
            SaveConfig();
        }
    }

    private void SaveDefaults()
    {
        lock (_lock)
        {
            _logger.LogInfo("SaveDefaults...");
            Directory.CreateDirectory(AppPath);
            File.WriteAllText(Path.Combine(AppPath, "settings.json"), GetDefaults());
        }
    }

    private string GetDefaults()
    {
        _logger.LogInfo("GetDefaults...");
        // work around not being able to read embedded json
        var settings = new Settings()
        {
            FirstRun = true,
            AppSettings = new AppSettings
            {
                StartLocation = new StartLocation
                {
                    X = 0,
                    Y = 0
                },
                StartSize = new StartSize
                {
                    Height = 0,
                    Width = 0
                },
                CloseToTray = false,
                MinimizeOnLaunch = false,
                AlwaysTop = false,
                UseProfileColors = true,
                AdvancedUser = false
            },
            Servers =
            [
                new Servers
                {
                    Ip = "127.0.0.1:6969",
                    Name = "LocalHost",
                    ServerId = "1721162719",
                    GamePath = "C:\\Games\\Spt"
                }
            ],
            DebugSettings = new DebugSettings
            {
                DebugUser = false
            }
        };

        return JsonSerializer.Serialize(settings);
    }
}
