using System.Threading.Tasks.Dataflow;

namespace Bnaya.Samples;

using static Helper;

internal static class DataflowBenchmark
{
    public static async Task<int> ComputeAsync(int jobCount, int iterations)
    {
        var block = new TransformBlock<int, int>(state =>
                        {
                            int calc = Calc(state, iterations);
                            return calc;
                        }, new ExecutionDataflowBlockOptions
                        {
                            SingleProducerConstrained = true,
                            MaxDegreeOfParallelism = jobCount,
                        });
        var batch = new BatchBlock<int>(jobCount);
        block.LinkTo(batch, new DataflowLinkOptions { PropagateCompletion = true });
        for (int i = 0; i < jobCount; i++)
        {
            block.Post(i);
        }
        block.Complete();

        int[] items = await batch.ReceiveAsync();
        return items.Sum();
    }
}
