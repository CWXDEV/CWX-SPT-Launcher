﻿using System.Reflection;
using System.Text.Json;
using CWX_SPT_Launcher.Models;
using MudBlazor;

namespace CWX_SPT_Launcher.Helpers;

public class SettingsHelper
{
    private static SettingsHelper _instance = null;
    private static readonly object Lock = new object();
    private SettingsClass _settings = null;

    public DialogOptions DialogOptions = new DialogOptions
    {
        Position = DialogPosition.Center,
        MaxWidth = MaxWidth.ExtraSmall,
        BackdropClick = true,
        CloseOnEscapeKey = true,
        NoHeader = true,
        FullWidth = true,
        BackgroundClass = "dialog-backdrop-class"
    };

    private SettingsHelper()
    {
        LoadSettingsFromFile();
    }

    public static SettingsHelper Instance
    {
        get
        {
            lock (Lock)
            {
                return _instance ??= new SettingsHelper();
            }
        }
    }

    private void LoadSettingsFromFile()
    {
        _settings = JsonSerializer.Deserialize<SettingsClass>(
            File.ReadAllText(Path.Combine(Main.AppPath, "settings.json")));
    }

    public SettingsClass GetSettings()
    {
        return _settings;
    }

    public void SaveSettings()
    {
        File.WriteAllText(Path.Combine(Main.AppPath, "settings.json"), JsonSerializer.Serialize(_settings));
    }

    public void SetClientSizeSettings(int height, int width)
    {
        _settings.AppSettings.StartSize.Height = height;
        _settings.AppSettings.StartSize.Width = width;
        SaveSettings();
    }

    public void SetClientLocationSettings(int x, int y)
    {
        _settings.AppSettings.StartLocation.X = x;
        _settings.AppSettings.StartLocation.Y = y;
        SaveSettings();
    }

    public void SetFirstRun(bool firstRun)
    {
        _settings.FirstRun = firstRun;
        SaveSettings();
    }

    public void SetServerSettings(List<ServersClass> servers)
    {
        _settings.Servers = servers;
        SaveSettings();
    }

    public void SetCloseToTray(bool closeToTray)
    {
        _settings.AppSettings.CloseToTray = closeToTray;
        SaveSettings();
    }

    public void SetHomePageDenseMode(bool mode)
    {
        _settings.AppSettings.HomePageDenseMode = mode;
        SaveSettings();
    }
    
    public void SetProfilePageDenseMode(bool mode)
    {
        _settings.AppSettings.ProfilePageDenseMode = mode;
        SaveSettings();
    }
}