using System.Text.Json;
using MudBlazor;
using Spt.Core.Models;

namespace Spt.Core.Helpers;

public class ConfigHelper
{
    private static readonly string AppPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CWX-SPT-Launcher\\Resources");

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
    private LogHelper? _logHelper;

    public ConfigHelper()
    {
        LoadSettingsFromFile();
    }

    public ConfigHelper(
        LogHelper logHelper
    )
    {
        _logHelper = logHelper;
        LoadSettingsFromFile();
    }

    private void LoadSettingsFromFile()
    {
        lock (_lock)
        {
            _logHelper.LogInfo("LoadSettingsFromFile...");

            // check if exists
            if (!File.Exists(Path.Combine(AppPath, "settings.json")))
            {
                SaveDefaults();
            }

            // if not save
            _settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(Path.Combine(AppPath, "settings.json")));
        }
    }

    public Settings GetConfig()
    {
        lock (_lock)
        {
            _logHelper.LogInfo("GetConfig...");
            return _settings;
        }
    }

    public void SaveConfig()
    {
        lock (_lock)
        {
            _logHelper.LogInfo("SaveConfig...");
            File.WriteAllText(Path.Combine(AppPath, "settings.json"), JsonSerializer.Serialize(_settings));
        }
    }

    public void SetClientSize(int height, int width)
    {
        lock (_lock)
        {
            _logHelper.LogInfo("SetClientSize...");
            _settings.AppSettings.StartSize.Height = height;
            _settings.AppSettings.StartSize.Width = width;
            SaveConfig();
        }
    }

    public void SetClientLocation(int x, int y)
    {
        lock (_lock)
        {
            _logHelper.LogInfo("SetClientLocation...");
            _settings.AppSettings.StartLocation.X = x;
            _settings.AppSettings.StartLocation.Y = y;
            SaveConfig();
        }
    }

    public void SetFirstRun(bool firstRun)
    {
        lock (_lock)
        {
            _logHelper.LogInfo("SetFirstRun...");
            _settings.FirstRun = firstRun;
            SaveConfig();
        }
    }

    public void SetServers(List<Servers> servers)
    {
        lock (_lock)
        {
            _logHelper.LogInfo("SetServers...");
            _settings.Servers = servers;
            SaveConfig();
        }
    }

    public void SetCloseToTray(bool closeToTray)
    {
        lock (_lock)
        {
            _logHelper.LogInfo("SetCloseToTray...");
            _settings.AppSettings.CloseToTray = closeToTray;
            SaveConfig();
        }
    }

    public void SetMinimizeOnLaunch(bool minimizeOnLaunch)
    {
        lock (_lock)
        {
            _logHelper.LogInfo("SetMinimizeOnLaunch...");
            _settings.AppSettings.MinimizeOnLaunch = minimizeOnLaunch;
            SaveConfig();
        }
    }

    public void SetAlwaysOnTop(bool alwaysOnTop)
    {
        lock (_lock)
        {
            _logHelper.LogInfo("SetAlwaysOnTop...");
            _settings.AppSettings.AlwaysTop = alwaysOnTop;
            SaveConfig();
        }
    }

    public void SetAdvancedUser(bool advancedUser)
    {
        lock (_lock)
        {
            _logHelper.LogInfo("SetAdvancedUser...");
            _settings.AppSettings.AdvancedUser = advancedUser;
            SaveConfig();
        }
    }

    public void SetDebugUser(bool debugUser)
    {
        lock (_lock)
        {
            _logHelper.LogInfo("SetDebugUser...");
            _settings.DebugSettings.DebugUser = debugUser;
            SaveConfig();
        }
    }

    public void SetDebugLoggingPage(bool access)
    {
        lock (_lock)
        {
            _logHelper.LogInfo("SetDebugLoggingPage...");
            _settings.DebugSettings.ShowLoggingPage = access;
            SaveConfig();
        }
    }

    public void SetApiKey(string apiKey)
    {
        lock (_lock)
        {
            _logHelper.LogInfo("SetApiKey...");
            _settings.ApiKey = apiKey;
            SaveConfig();
        }
    }

    private void SaveDefaults()
    {
        lock (_lock)
        {
            _logHelper.LogInfo("SaveDefaults...");
            Directory.CreateDirectory(AppPath);
            File.WriteAllText(Path.Combine(AppPath, "settings.json"), JsonSerializer.Serialize(GetDefaults()));
        }
    }

    private Settings GetDefaults()
    {
        _logHelper.LogInfo("GetDefaults...");
        return new Settings
        {
            FirstRun = true,
            GamePath = Environment.CurrentDirectory,
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
                MinimizeOnLaunch = true,
                AlwaysTop = false,
                AdvancedUser = false
            },
            Servers =
            [
                new Servers
                {
                    Ip = "127.0.0.1:6969",
                    Name = "LocalHost",
                    ServerId = "1721162719",
                    Locked = true
                }
            ],
            DebugSettings = new DebugSettings
            {
                DebugUser = false
            },
            ApiKey = ""
        };
    }
}
