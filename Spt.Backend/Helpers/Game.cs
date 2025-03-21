using System.Diagnostics;
using System.Runtime.InteropServices;
using Spt.Core.Eft;
using Spt.Core.Spt;
using Microsoft.Win32;

namespace Spt.Backend;

public class Game
{
    public Game(
        StateManager stateManager
    )
    {
        _stateManager = stateManager;
    }

    private StateManager _stateManager;

    private const string registryInstall = @"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\EscapeFromTarkov";
    private const string registrySettings = @"Software\Battlestate Games\EscapeFromTarkov";

    private string? DetectOriginalGamePath()
    {
        // We can't detect the installed path on non-Windows
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return null;
        }

        var installLocation = Registry.LocalMachine.OpenSubKey(registryInstall, false)?.GetValue("InstallLocation");
        var info = (installLocation is string key) ? new DirectoryInfo(key) : null;
        return info?.FullName;
    }

    public async Task<bool> LaunchGame()
    {
        // setup directories
        // if (IsInstalledInLive())
        // {
        //     return false;
        // }
        //
        // SetupGameFiles();

        // check game path
        var clientExecutable = Path.Join(_stateManager.ConnectedServer.GamePath, "EscapeFromTarkov.exe");

        if (!File.Exists(clientExecutable))
        {
            Console.WriteLine("[LaunchGame] Valid Game Path   :: FAILED");
            Console.WriteLine($"Could not find {clientExecutable}");
            return false;
        }

        // apply patches
        // TODO: set up patching

        //start game
        var args =
            $"-force-gfx-jobs native -token={_stateManager.SelectedProfile.ProfileID} -config=" + "{\'BackendUrl\':\'" + $"{_stateManager.ConnectedServer.Ip}" + "\',\'Version\':\'live\',\'MatchingVersion\':\'live\'}";
         // $"-force-gfx-jobs native -token=67b4b04b0003dc184199f6f6 -config='BackendUrl':'https://127.0.0.1:6969','Version':'live','MatchingVersion':'live'";

        var clientProcess = new ProcessStartInfo(clientExecutable)
        {
            Arguments = args,
            UseShellExecute = false,
            WorkingDirectory = _stateManager.ConnectedServer.GamePath,
        };

        try
        {
            Process.Start(clientProcess);
            Console.WriteLine("[LaunchGame] Game process started");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }

        return true;
    }

    bool IsInstalledInLive()
    {
        var isInstalledInLive = false;

        try
        {
            FileInfo[] files =
            [
                // SPT files
                new FileInfo(Path.Combine(_stateManager.ConnectedServer.GamePath, "SPT.Launcher.exe")),
                new FileInfo(Path.Combine(_stateManager.ConnectedServer.GamePath, "SPT.Server.exe")),

                // bepinex files
                new FileInfo(Path.Combine(_stateManager.ConnectedServer.GamePath, @"doorstep_config.ini")),
                new FileInfo(Path.Combine(_stateManager.ConnectedServer.GamePath, @"winhttp.dll")),

                // licenses
                new FileInfo(Path.Combine(_stateManager.ConnectedServer.GamePath, @"LICENSE-BEPINEX.txt")),
                new FileInfo(Path.Combine(_stateManager.ConnectedServer.GamePath, @"LICENSE-ConfigurationManager.txt")),
                new FileInfo(Path.Combine(_stateManager.ConnectedServer.GamePath, @"LICENSE-Launcher.txt")),
                new FileInfo(Path.Combine(_stateManager.ConnectedServer.GamePath, @"LICENSE-Modules.txt")),
                new FileInfo(Path.Combine(_stateManager.ConnectedServer.GamePath, @"LICENSE-Server.txt"))
            ];
            DirectoryInfo[] directories =
            [
                new DirectoryInfo(Path.Combine(_stateManager.ConnectedServer.GamePath, @"SPT_Data")),
                new DirectoryInfo(Path.Combine(_stateManager.ConnectedServer.GamePath, @"BepInEx"))
            ];

            foreach (var file in files)
            {
                if (!File.Exists(file.FullName))
                {
                    continue;
                }

                File.Delete(file.FullName);

                isInstalledInLive = true;
            }

            foreach (var directory in directories)
            {
                if (!Directory.Exists(directory.FullName))
                {
                    continue;
                }

                RemoveFilesRecurse(directory);

                isInstalledInLive = true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return isInstalledInLive;
    }


    void SetupGameFiles()
    {
        var files = new[]
        {
            GetFileForCleanup("BattlEye"),
            GetFileForCleanup("Logs"),
            GetFileForCleanup("ConsistencyInfo"),
            GetFileForCleanup("EscapeFromTarkov_BE.exe"),
            GetFileForCleanup("Uninstall.exe"),
            GetFileForCleanup("UnityCrashHandler64.exe"),
            GetFileForCleanup("WinPixEventRuntime.dll")
        };

        foreach (var file in files)
        {
            if (file == null)
            {
                continue;
            }

            if (Directory.Exists(file))
            {
                RemoveFilesRecurse(new DirectoryInfo(file));
            }

            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
    }

    private string GetFileForCleanup(string fileName)
    {
        //!_excludeFromCleanup.Contains(fileName) ? Path.Combine(gamePath, fileName) : null
        return Path.Join(_stateManager.ConnectedServer.GamePath, fileName);
    }

    /// <summary>
    /// Clean the temp folder
    /// </summary>
    /// <returns>returns true if the temp folder was cleaned succefully or doesn't exist. returns false if something went wrong.</returns>
    public bool CleanTempFiles()
    {
        var rootdir = new DirectoryInfo(Path.Join(_stateManager.ConnectedServer.GamePath, "user\\sptappdata"));

        return !rootdir.Exists || RemoveFilesRecurse(rootdir);
    }

    private bool RemoveFilesRecurse(DirectoryInfo basedir)
    {
        if (!basedir.Exists)
        {
            return true;
        }

        try
        {
            // remove subdirectories
            foreach (var dir in basedir.EnumerateDirectories())
            {
                RemoveFilesRecurse(dir);
            }

            // remove files
            var files = basedir.GetFiles();

            foreach (var file in files)
            {
                file.IsReadOnly = false;
                file.Delete();
            }

            // remove directory
            basedir.Delete();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }

        return true;
    }
}
