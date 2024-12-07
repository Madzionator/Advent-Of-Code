namespace AdventOfCode2024.Helpers;

internal static class NumericExtensions
{
    public static long Concat(this long num1, int num2)
    {
        var multiplier = 1;
        var tempNum2 = num2;

        while (tempNum2 > 0)
        {
            multiplier *= 10;
            tempNum2 /= 10;
        }

        return num1 * multiplier + num2;
    }
}