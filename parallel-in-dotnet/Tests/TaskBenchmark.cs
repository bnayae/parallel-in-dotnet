namespace Bnaya.Samples;

using static Helper;

internal static class TaskBenchmark
{
    public static Task<int> ComputeAsync(int jobCount, int iterations)
    {
        Task<int[]> tasks = Task.WhenAll(
                        Enumerable.Range(0, jobCount)
                        .Select(j =>
                                Task.Factory.StartNew((state) =>
                                {
                                    int calc = Calc((int)state, iterations);
                                    return calc;
                                }, j)));
        return tasks.ContinueWith(t => t.Result.Sum());
    }
}
