namespace Bnaya.Samples;

internal static class PLinqBenchmark
{
    public static int Compute(int jobCount, int iterations)
    {
        int sum = Enumerable.Range(0, jobCount)
                            .AsParallel()
                                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                            .Select(state =>
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
                                    })
                            .Sum();
        return sum;
    }
}
