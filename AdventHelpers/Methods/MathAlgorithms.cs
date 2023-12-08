namespace Advent.Helpers.Methods
{
    public static class MathAlgorithms
    {
        // Least Common Multiple
        public static long LCM(long a, long b) => (a / GCD(a, b)) * b;

        // Greatest Common Divisor
        public static long GCD(long a, long b) => b == 0 ? a : GCD(b, a % b);
    }
}
