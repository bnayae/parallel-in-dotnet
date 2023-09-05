namespace Bnaya.Samples;

internal static class Helper
{
    public static int Calc(int start, int iterations)
    {
        int calc = 0;
        for (int i = start; i < iterations; i++)
        {
            if (i % 2 == 0)
                calc += i;
            else
                calc -= i;
        }
        return calc;
    }
}
