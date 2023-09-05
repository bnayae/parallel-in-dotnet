namespace Bnaya.Samples;

using static Helper;

internal static class AsyncBenchmark
{
    public static async Task<int> ComputeAsync(int jobCount, int iterations)
    {
        int[] tasks = await Task.WhenAll(
                        Enumerable.Range(0, jobCount)
                        .Select(j =>
                                Task.Factory.StartNew((state) => Calc((int)state, iterations), j)));
        return tasks.Sum();
    }
}
