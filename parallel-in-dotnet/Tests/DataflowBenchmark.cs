using System.Threading.Tasks.Dataflow;

namespace Bnaya.Samples;

internal static class DataflowBenchmark
{
    public static async Task<int> ComputeAsync(int jobCount, int iterations)
    {
        var block = new TransformBlock<int, int>(state =>
                        {
                            int calc = 0;
                            for (int i = state; i < iterations; i++)
                            {
                                if (i % 2 == 0)
                                    calc += i;
                                else
                                    calc -= i;
                            }
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
