using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2020.Week2
{
    public class Day9
    {
        public static void Execute()
        {
            var numbers = File.ReadAllLines(@"Week2\input9.txt").Select(long.Parse).ToList();

            var resultA = TaskA(numbers);
            long resultB = TaskB(numbers, resultA.Item1, resultA.Item2);

            Console.WriteLine(resultA.Item1);
            Console.WriteLine(resultB);
        }

        private static (long, int) TaskA(List<long> numbers)
        {
            for (int i = 25; i < numbers.Count; i++)
            {
                bool correct = false;
                for (int j = i - 25; j < i; j++)
                {
                    for (int k = i - 25; k < i; k++)
                    {
                        if (j == k)
                            continue;
                        if (numbers[i] == numbers[j] + numbers[k])
                        {
                            correct = true;
                            break;
                        }
                    }

                    if (correct)
                        break;
                }

                if (!correct)
                    return (numbers[i], i);
            }
            return (-1, -1);
        }

        private static long TaskB(List<long> numbers, long ItemA, int positionA)
        {
            for (int i = 0; i < positionA; i++)
            {
                long sum, min, max;
                sum = min = max = numbers[i];

                for (int j = i+1; j < positionA; j++)
                {
                    if (numbers[j] > max)
                        max = numbers[j];
                    if (numbers[j] < min)
                        min = numbers[j];

                    sum += numbers[j];
                    if (sum == ItemA)
                        return (min+max);
                    if (sum > ItemA)
                        break;
                }
            }
            return -1;
        }

    }
}