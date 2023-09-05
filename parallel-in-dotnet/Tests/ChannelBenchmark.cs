using System.Threading.Channels;

namespace Bnaya.Samples;

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

    public static async Task Example()
    {
        var channel = Channel.CreateUnbounded<int>(new UnboundedChannelOptions
        {
            SingleWriter = false,
            SingleReader = false,
            AllowSynchronousContinuations = true

        });

        for (int i = 0; i < 5; i++)
        {
            await channel.Writer.WriteAsync(i);
        }
        channel.Writer.Complete();

        await Task.WhenAll(
                Reading("A"),
                Reading("B"),
                Reading("C")
            );

        async Task Reading(string tag)
        {
            while (await channel.Reader.WaitToReadAsync())
            {
                if (!channel.Reader.TryRead(out int v))
                    continue;
                Console.WriteLine($"Channel [{tag}]: {v}");
                await Task.Yield();
            }
        }
    }
}
