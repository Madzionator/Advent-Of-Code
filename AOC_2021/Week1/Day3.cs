using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2021.Week1
{
    class Day3
    {
        public static void Execute()
        {
            var numbers = File.ReadAllLines(@"Week1\input3.txt").ToList();

            Console.WriteLine(TaskA(numbers));
            Console.WriteLine(TaskB(numbers));
        }

        public static int TaskA(List<string> numbers)
        {
            char[] gamma = new char[numbers[0].Length];
            char[] epsilon = new char[numbers[0].Length];

            for (int i = 0; i < numbers[0].Length; i++)
            {
                gamma[i] = MostCommon(numbers, i);
                epsilon[i] = gamma[i] == '0' ? '1' : '0';
            }

            return Convert.ToInt32(new string(gamma), 2) * Convert.ToInt32(new string(epsilon), 2);
        }

        public static int TaskB(List<string> numbers)
        {
            var oxygenGenerator = numbers.ToList();
            var CO2Scrubber = numbers.ToList();

            return LastNumber(oxygenGenerator, true) * LastNumber(CO2Scrubber, false);
        }

        private static int LastNumber(List<string> numbers, bool isMostCommon)
        {
            for (int i = 0; i < numbers[0].Length; i++)
            {
                char mostCommon = isMostCommon ? MostCommon(numbers, i) : LeastCommon(numbers, i);
                for (int j = numbers.Count - 1; j >= 0 && numbers.Count > 1; j--)
                    if (numbers[j][i] != mostCommon)
                        numbers.RemoveAt(j);
            }

            return Convert.ToInt32(numbers[0], 2);
        }

        private static char MostCommon(List<string> numbers, int i) 
            => 2 * numbers.Where(x => x[i] == '1').Count() >= numbers.Count ? '1' : '0';

        private static char LeastCommon(List<string> numbers, int i) 
            => 2 * numbers.Where(x => x[i] == '1').Count() < numbers.Count ? '1' : '0';
    }
}
