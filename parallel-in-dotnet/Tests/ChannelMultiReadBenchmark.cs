using System.Threading.Channels;

namespace Bnaya.Samples;

// https://learn.microsoft.com/en-us/dotnet/core/extensions/channels

internal static class ChannelMultiReadBenchmark
{
    public static async Task<int> ComputeAsync(int jobCount, int iterations)
    {
        var channel = Channel.CreateUnbounded<int>(new UnboundedChannelOptions
        {
            //SingleWriter = false,
            //SingleReader = false,
            AllowSynchronousContinuations = true

        });


        for (int i = 0; i < jobCount; i++)
        {
            ValueTask _ = channel.Writer.WriteAsync(i);
        }
        channel.Writer.Complete();


        int[] result = await Task.WhenAll(
                                Enumerable.Range(0, Environment.ProcessorCount)
                                          .Select(ReadAllAsync));


        return result.Sum();

        async Task<int> ReadAllAsync(int job)
        {
            int sum = 0;
            while (await channel.Reader.WaitToReadAsync())
            {
                if (!channel.Reader.TryRead(out int state))
                    continue;
                int calc = 0;
                for (int i = state; i < iterations; i++)
                {
                    if (i % 2 == 0)
                        calc += i;
                    else
                        calc -= i;
                }
                sum += calc;
            }
            return sum;
        }
    }
}
