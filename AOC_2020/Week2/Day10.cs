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
            var numbers = File.ReadAllLines(@"Week2\input10.txt")
                .Select(int.Parse)
                .OrderBy(x => x)
                .ToList();

            numbers.Insert(0, 0);
            numbers.Add(numbers[^1] + 3);

            int resultA = TaskA(numbers);
            long resultB = TaskB(numbers);

            Console.WriteLine(resultA);
            Console.WriteLine(resultB);
        }

        private static int TaskA(List<int> numbers)
        {
            int diff1 = 0;
            int r3 = 0;
            for (int i = 0; i < numbers.Count - 1; i++)
                if (numbers[i + 1] - numbers[i] == 1)
                    diff1++;
                else if (numbers[i + 1] - numbers[i] == 3)
                    r3++;

            return diff1 * r3;
        }

        private static long TaskB(List<int> numbers)
        {
            long[] previousSum = new long[] { 1, 1, 1 };
            for (int i = numbers.Count - 2; i > 0; i--)
            {
                long currentSum = 0;

                for (int j = 1; j <= 3; j++)
                    if (i + j < numbers.Count && numbers[i + j] - numbers[i] <= 3)
                        currentSum += previousSum[j - 1];

                previousSum[2] = previousSum[1];
                previousSum[1] = previousSum[0];
                previousSum[0] = currentSum;
            }

            long total = 0;
            for (int i = 1; i <= 3; i++)
                if (numbers.Contains(i))
                    total += previousSum[i - 1];

            return total;
        }
    }
} 