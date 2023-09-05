using System.Collections.Concurrent;

namespace Bnaya.Samples;

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
                int calc = 0;
                for (int i = (int)state; i < iterations; i++)
                {
                    if (i % 2 == 0)
                        calc += i;
                    else
                        calc -= i;
                }
                q.Enqueue(calc);
                countdown.Signal();
            }, job);
        }
        countdown.Wait();
        return q.Sum();
    }
}
