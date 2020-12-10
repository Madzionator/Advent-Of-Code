using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2020.Week2
{
    public class Day10
    {
        public static void Execute()
        {
            var joltageRatings = File.ReadAllLines(@"Week2\input10.txt").Select(int.Parse).OrderBy(x => x).ToList();
            joltageRatings.Insert(0, 0);
            joltageRatings.Add(joltageRatings[^1] + 3);

            int resultA = TaskA(joltageRatings);
            long resultB = TaskB(joltageRatings);

            Console.WriteLine(resultA);
            Console.WriteLine(resultB);
        }

        private static int TaskA(List<int> joltageRatings)
        {
            int diff1 = 0;
            int r3 = 0;
            for (int i = 0; i < joltageRatings.Count - 1; i++)
                if (joltageRatings[i + 1] - joltageRatings[i] == 1)
                    diff1++;
                else if (joltageRatings[i + 1] - joltageRatings[i] == 3)
                    r3++;

            return diff1 * r3;
        }

        private static long TaskB(List<int> joltageRatings)
        {
            List <long> previousSum = new List<long> { 1, 1, 1 };
            for (int i = joltageRatings.Count - 2; i > 0; i--)
            {
                long currentSum = 0;

                for(int j = 1; j<=3; j++)
                    if (i + j < joltageRatings.Count && joltageRatings[i + j] - joltageRatings[i] <= 3)
                        currentSum += previousSum[j-1];

                previousSum.Insert(0, currentSum);
                previousSum.RemoveAt(3);
            }

            long total = 0;
            for(int i = 1; i<=3; i++)
                if (joltageRatings.Contains(i))
                    total += previousSum[i - 1];

            return total;
        }
    }
}