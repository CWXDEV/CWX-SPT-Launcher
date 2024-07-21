﻿using CWX_SPT_Backend.CWX;

namespace CWX_SPT_Frontend.Helpers;

public class ServerHelper
{
    private static ServerHelper _instance = null;
    private static readonly object Lock = new object();
    private object _profiles = new object();
    public string ConnectedServerId = "";
    public bool SelectedProfile = false;
    
    public static ServerHelper Instance
    {
        get
        {
            lock (Lock)
            {
                return _instance ??= new ServerHelper();
            }
        }
    }

    public async Task<bool> IsServerReachable(ServersClass server)
    {
        await Task.Delay(500);
        return true;
    }
    
    public async Task<bool> GetServerProfiles(ServersClass server)
    {
        await Task.Delay(500);
        return true;
    }
    
    // TODO: Remove
    public async Task<bool> GetFakeMessage1(ServersClass server)
    {
        await Task.Delay(500);
        return true;
    }
    
    // TODO: Remove
    public async Task<bool> GetFakeMessage2(ServersClass server)
    {
        await Task.Delay(500);
        return true;
    }
    
    // TODO: Remove
    public async Task<bool> GetFakeMessage3(ServersClass server)
    {
        await Task.Delay(500);
        return true;
    }

    public void LogoutAndDispose()
    {
        // null profiles
        _profiles = null;
        ConnectedServerId = "";
        SelectedProfile = false;
    }

    public void Login(string serverId)
    {
        ConnectedServerId = serverId;
    }
}