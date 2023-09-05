using System.Collections.Concurrent;
using System.Threading.Tasks.Dataflow;

namespace Bnaya.Samples;

internal static class DataflowSimpleBenchmark
{
    public static async Task<int> ComputeAsync(int jobCount, int iterations)
    {
        var q = new ConcurrentQueue<int>();
        var block = new ActionBlock<int>(state =>
                        {
                            int calc = 0;
                            for (int i = state; i < iterations; i++)
                            {
                                if (i % 2 == 0)
                                    calc += i;
                                else
                                    calc -= i;
                            }
                            q.Enqueue(calc);
                        }, new ExecutionDataflowBlockOptions
                        {
                            SingleProducerConstrained = true,
                            MaxDegreeOfParallelism = jobCount,
                        });
        for (int i = 0; i < jobCount; i++)
        {
            block.Post(i);
        }
        block.Complete();
        await block.Completion;
        return q.Sum();
    }
}
