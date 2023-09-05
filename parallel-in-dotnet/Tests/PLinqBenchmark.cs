namespace Bnaya.Samples;

using static Helper;

internal static class PLinqBenchmark
{
    public static int Compute(int jobCount, int iterations)
    {
        int sum = Enumerable.Range(0, jobCount)
                            .AsParallel()
                                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                            .Select(state =>
                                    {
                                        int calc = Calc(state, iterations);
                                        return calc;
                                    })
                            .Sum();
        return sum;
    }
}
