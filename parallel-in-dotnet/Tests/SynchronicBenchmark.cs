namespace Bnaya.Samples;

using static Helper;

internal static class SynchronicBenchmark
{
    public static int Compute(int jobCount, int iterations)
    {
        int sum = 0;
        for (int job = 0; job < jobCount; job++)
        {
            int calc = Calc(job, iterations);
            sum += calc;
        }
        return sum;
    }
}
