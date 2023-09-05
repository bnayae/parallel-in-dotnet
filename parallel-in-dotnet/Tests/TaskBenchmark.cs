namespace Bnaya.Samples;

internal static class TaskBenchmark
{
    public static Task<int> ComputeAsync(int jobCount, int iterations)
    {
        Task<int[]> tasks = Task.WhenAll(
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
        return tasks.ContinueWith(t => t.Result.Sum());
    }
}
