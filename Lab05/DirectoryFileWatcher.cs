namespace lab05;

using System;
using System.IO;
using System.Threading;

internal class DirectoryFileWatcher
{
    private bool _isRunning = true;
    private readonly Thread _monitoringThread;
    private readonly FileSystemWatcher _directoryWatcher;
    
    public DirectoryFileWatcher(string directory)
    {
        _directoryWatcher = new FileSystemWatcher();
        _directoryWatcher.Path = directory;
        _directoryWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName;
        _directoryWatcher.Filter = "*.*";
        
        _directoryWatcher.Created += (sender, e) => Console.WriteLine($"added file {e.Name}");
        _directoryWatcher.Deleted += (sender, e) => Console.WriteLine($"removed file {e.Name}");
        _directoryWatcher.Renamed += (sender, e) => Console.WriteLine($"renamed file from {e.OldName} to {e.Name}");
        
        _monitoringThread = new Thread(Monitor);
        _monitoringThread.Start();

        var interceptThread = new Thread(Intercept);

        while (true)
        {
            var key = Console.ReadKey(intercept: true).KeyChar.ToString();
            if (key.ToLower() == "q")
            {
                break;
            }
        }
    }

    private void Monitor()
    {
        _directoryWatcher.EnableRaisingEvents = true; // Rozpoczynamy monitorowanie
        Console.WriteLine("Monitoring started. Press 'q' to stop.");
    }

    private void Intercept()
    {
        while (_isRunning)
        {
            if (Console.ReadKey(true).Key != ConsoleKey.Q) continue;
            _isRunning = false;
            _directoryWatcher.EnableRaisingEvents = false;
            _monitoringThread.Join();
            Console.WriteLine("Monitoring stopped.");
        }
    }
}
