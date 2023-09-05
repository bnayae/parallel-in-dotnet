namespace Bnaya.Samples;

internal static class SynchronicBenchmark
{
    public static int Compute(int jobCount, int iterations)
    {
        int sum = 0;
        for (int job = 0; job < jobCount; job++)
        {
            int calc = 0;
            for (int i = job; i < iterations; i++)
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
