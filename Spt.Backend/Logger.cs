namespace Spt.Backend;

public class Logger
{
    public List<string> Logs = new List<string>();
    private Lock _lock = new Lock();
    public event Action OnLogsChanged;

    private void NotifyLogsChanged() => OnLogsChanged?.Invoke();

    public void AddLog(string log)
    {
        lock (_lock)
        {
            Logs.Add(log);
            Console.WriteLine(log);
            NotifyLogsChanged();
        }
    }

    public void RemoveLogAtIndex(int index)
    {
        lock (_lock)
        {
            Logs.RemoveAt(index);
            NotifyLogsChanged();
        }
    }

    public void RemoveLog(string log)
    {
        lock (_lock)
        {
            Logs.Remove(log);
            NotifyLogsChanged();
        }
    }

    public List<string> GetLogs()
    {
        return Logs;
    }

    public void ClearLogs()
    {
        lock (_lock)
        {
            Logs.Clear();
            NotifyLogsChanged();
        }
    }

    public void LogInfo(string log)
    {
        AddLog($"[INFO] {log}");
    }

    public void LogError(string log)
    {
        AddLog($"[ERROR] {log}");
    }

    public void LogWarning(string log)
    {
        AddLog($"[WARNING] {log}");
    }
}
