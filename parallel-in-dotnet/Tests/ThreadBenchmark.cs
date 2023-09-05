using System.Collections.Concurrent;

namespace Bnaya.Samples;

internal static class ThreadBenchmark
{
    public static int Compute(int jobCount, int iterations)
    {
        var q = new ConcurrentQueue<int>();
        using var countdown = new CountdownEvent(jobCount);
        for (int job = 0; job < jobCount; job++)
        {
            var t = new Thread(state =>
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
            });
            t.Start(job);
        }
        countdown.Wait();
        return q.Sum();
    }
}
