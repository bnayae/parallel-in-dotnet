namespace Bnaya.Samples;

internal static class Helper
{
    public static int Calc(int start, int iterations)
    {
        int calc = 0;
        for (int i = start; i < iterations; i++)
        {
            calc += (i % 2 == 0) ? i : -i;
        }
        return calc;
    }
}
