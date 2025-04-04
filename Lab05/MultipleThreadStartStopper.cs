namespace lab05;

using System;
using System.Threading;

public class MultipleThreadStartStopper
{
    private readonly Thread[] _threads;
    private readonly int _n;
    private readonly ManualResetEvent[] _threadStartEvents;

    public MultipleThreadStartStopper(int threadCount)
    {
        _n = threadCount;
        _threads = new Thread[_n];
        _threadStartEvents = new ManualResetEvent[_n];
        var start = new Thread(Starting);
        start.Start();
    }

    private void Starting()
    {
        Console.WriteLine("Starting...");
        for (var i = 0; i < _n; i++)
        {
            _threadStartEvents[i] = new ManualResetEvent(false);
            var i1 = i;
            _threads[i] = new Thread(() => ThreadTask(i1));
            _threads[i].Start();
        }
        WaitForAllThreadsToStart();
        Console.WriteLine("Stopping...");
        WaitForAllThreadsToFinish();
    }

    private void ThreadTask(int index)
    {
        Console.WriteLine($"Thread {index} started.");
        _threadStartEvents[index].Set();
        Thread.Sleep(2000); 
        Console.WriteLine($"Thread {index} is finishing.");
    }
    
    private void WaitForAllThreadsToStart()
    {
        foreach (var threadEvent in _threadStartEvents)
        {
            threadEvent.WaitOne();
        }
    }
    
    private void WaitForAllThreadsToFinish()
    {
        foreach (var thread in _threads)
        {
            thread.Join(); 
        }
    }
}
