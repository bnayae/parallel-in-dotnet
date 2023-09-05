namespace Bnaya.Samples;

internal static class AsyncBenchmark
{
    public static async Task<int> ComputeAsync(int jobCount, int iterations)
    {
        int[] tasks = await Task.WhenAll(
                        Enumerable.Range(0, jobCount)
                        .Select(j =>
                                Task.Factory.StartNew((state) =>
                                {
                                    int calc = 0;
                                    for (int i = (int)state; i < iterations; i++)
                                    {
                                        if (i % 2 == 0)
                                            calc += i;
                                        else
                                            calc -= i;
                                    }
                                    return calc;
                                }, j)));
        return tasks.Sum();
    }
}
