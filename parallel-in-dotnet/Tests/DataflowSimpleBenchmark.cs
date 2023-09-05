using System.Collections.Concurrent;
using System.Threading.Tasks.Dataflow;

namespace Bnaya.Samples;

using static Helper;

internal static class DataflowSimpleBenchmark
{
    public static async Task<int> ComputeAsync(int jobCount, int iterations)
    {
        var q = new ConcurrentQueue<int>();
        var block = new ActionBlock<int>(state =>
                        {
                            int calc = Calc(state, iterations);
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
