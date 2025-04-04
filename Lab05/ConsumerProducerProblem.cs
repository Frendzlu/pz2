namespace lab05;

using System;
using System.Collections.Generic;
using System.Threading;

internal class ConsumerProducerProblem
{
    private readonly Queue<string?> _sharedQueue = new Queue<string?>();
    private readonly object _queueLock = new object();
    private readonly Dictionary<string, int> _producerCount = new Dictionary<string, int>();
    private bool _isRunning = true;
    private readonly List<Thread> _consumerThreads = new List<Thread>();
    private readonly List<Thread> _producerThreads = new List<Thread>();
    private const int MinWaitTime = 500;
    private const int MaxWaitTime = 2500;
    private readonly Thread _stopThread;

    public ConsumerProducerProblem(int numberOfConsumers, int numberOfProducers)
    {
        for (var i = 0; i < numberOfConsumers; i++)
        {
            var i1 = i;
            var consumerThread = new Thread(() => MakeConsumer(i1));
            _consumerThreads.Add(consumerThread);
            consumerThread.Start();
        }
        
        for (var i = 0; i < numberOfProducers; i++)
        {
            var i1 = i;
            var producerThread = new Thread(() => MakeProducer(i1));
            _producerThreads.Add(producerThread);
            producerThread.Start();
        }
        Console.WriteLine("Consumer and producer thread started");
        _stopThread = new Thread(StopThreads);
        _stopThread.Start();
        Console.WriteLine("Stop thread started");
    }
    
    private void MakeConsumer(int id)
    {
        var consumerName = $"CONS-{id}";

        while (_isRunning)
        {
            // Console.WriteLine($"Consumer {consumerName}");
            string? consumedItem = null;
            lock (_queueLock)
            {
                if (_sharedQueue.Count > 0)
                {
                    consumedItem = _sharedQueue.Dequeue();
                }
            }

            if (consumedItem != null)
            {
                var producerName = consumedItem.Split(',')[1];
                Console.WriteLine($"{consumerName} consumed {consumedItem}");
                
                lock (_producerCount)
                {
                    _producerCount.TryAdd(producerName, 0);
                    _producerCount[producerName]++;
                }
            }
            
            Thread.Sleep(new Random().Next(MinWaitTime, MaxWaitTime)); 
        }
    }
    
    private void MakeProducer(int id)
    {
        var producerName = $"PROD-{id}";

        var objectId = 0;

        while (_isRunning)
        {
            objectId++;
            var objectData = $"OBJECT-{objectId},{producerName}";

            lock (_queueLock)
            {
                _sharedQueue.Enqueue(objectData);
                Console.WriteLine($"{producerName} produced {objectData}");
            }
            
            Thread.Sleep(new Random().Next(MinWaitTime, MaxWaitTime));
        }
    }

    // Method to stop threads gracefully
    private void StopThreads()
    {
        Console.WriteLine("Press 'q' to stop.");
        while (_isRunning)
        {
            if (Console.ReadKey(true).Key != ConsoleKey.Q) continue;
            _isRunning = false;
            foreach (var thread in _consumerThreads)
            {
                thread.Join();
            }
            foreach (var thread in _producerThreads)
            {
                thread.Join();
            }
            PrintStatistics();
            break;
        }
    }

    // Print the statistics about consumption
    private void PrintStatistics()
    {
        Console.WriteLine("Consumption Statistics:");
        lock (_producerCount)
        {
            foreach (var entry in _producerCount)
            {
                Console.WriteLine($"\t{entry.Key} produced {entry.Value} items that were consumed.");
            }
        }
    }
}