namespace lab05;

using System;
using System.IO;
using System.Threading;

public class PastaFinder
{
    private readonly string _searchPattern;
    private readonly Queue<string?> _sharedQueue = new Queue<string?>();
    private readonly object _queueLock = new object();
    private bool _running = true;
    private readonly Thread _searchThread;
    private int _counter = 0;
    
    public PastaFinder(string directory, string searchPattern)
    {
        _searchPattern = searchPattern;
        var writeThread = new Thread(Writer);
        writeThread.Start();
        
        _searchThread = new Thread(() => SearchFiles(directory));
        _searchThread.Start();
    }

    private void Writer()
    {
        while (_running)
        {
            lock (_queueLock)
            {
                while (_sharedQueue.Count > 0)
                {
                    Console.WriteLine(_sharedQueue.Dequeue());
                }
            }
        }
        _searchThread.Join();
        lock (_queueLock)
        {
            while (_sharedQueue.Count > 0)
            {
                Console.WriteLine(_sharedQueue.Dequeue());
            }
        }
        Console.WriteLine($"Search completed. Found {_counter} files.");
    }
    
    private void SearchFiles(string directory)
    {
        try
        {
            var files = Directory.GetFiles(directory);
            foreach (var file in files)
            {
                if (!Path.GetFileName(file).Contains(_searchPattern)) continue;
                lock (_queueLock)
                {
                    _sharedQueue.Enqueue(file);
                }
                _counter++;
            }

            // Przechodzimy do podkatalogów
            var directories = Directory.GetDirectories(directory);
            foreach (var dir in directories)
            {
                // Rekurencyjne wywołanie dla podkatalogu
                SearchFiles(dir);
            }
        }
        catch (UnauthorizedAccessException)
        {
            // Obsługuje wyjątek, jeśli nie mamy dostępu do jakiegoś katalogu
            Console.WriteLine("Access denied to directory: " + directory);
        }
        catch (Exception ex)
        {
            // Inne wyjątki (np. błąd I/O)
            Console.WriteLine("Error: " + ex.Message);
        }
        _running = false;
    }
}
