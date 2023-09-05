using System.Threading.Channels;

namespace Bnaya.Samples;

using static Helper;

// https://learn.microsoft.com/en-us/dotnet/core/extensions/channels

internal static class ChannelBenchmark
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


        int result = await ReadAllAsync();

        return result;

        async Task<int> ReadAllAsync()
        {
            int sum = 0;
            await foreach (int state in channel.Reader.ReadAllAsync())
            {
                int calc = Calc(state, iterations);
                sum += calc;
            }
            return sum;
        }
    }

    public static async Task<int> Fork(int jobCount, int iterations)
    {
        var channel = Channel.CreateUnbounded<int>(new UnboundedChannelOptions
        {
            SingleWriter = false,
            SingleReader = false,
            AllowSynchronousContinuations = true

        });

        for (int i = 0; i < jobCount; i++)
        {
            await channel.Writer.WriteAsync(i);
        }
        channel.Writer.Complete();


        int[] sums = await Task.WhenAll(Enumerable.Range(0, jobCount).Select(Reading));
        return sums.Sum();

        async Task<int> Reading(int job)
        {
            int sum = 0;
            while (await channel.Reader.WaitToReadAsync())
            {
                if (!channel.Reader.TryRead(out int state))
                    continue;
                int calc = Calc(state, iterations);
                sum += calc;
            }
            return sum;
        }
    }
}
