using System;
using System.Collections.Generic;
using System.Linq;

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

        public static int Multiply(this IEnumerable<int> enumerable)
        {
            return enumerable.Aggregate(1, (current, item) => current * item);
        }
        public static long Multiply(this IEnumerable<long> enumerable)
        {
            return enumerable.Aggregate((long)1, (current, item) => current * item);
        }
    }
}