using System.Collections.Concurrent;

namespace Bnaya.Samples;

using static Helper;

internal static class ThreadPoolBenchmark
{
    public static int Compute(int jobCount, int iterations)
    {
        var q = new ConcurrentQueue<int>();
        using var countdown = new CountdownEvent(jobCount);
        for (int job = 0; job < jobCount; job++)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                int calc = Calc((int)state, iterations);
                q.Enqueue(calc);
                countdown.Signal();
            }, job);
        }
        countdown.Wait();
        return q.Sum();
    }
}
