using System;

namespace Advent.Helpers.Extensions
{
    public static class BasicUtils
    {
        public static bool IsBetween(this int X, int A, int B)
        {
            var min = Math.Min(A, B);
            var max = Math.Max(A, B);

            return min <= X && X <= max;
        }
    }
}