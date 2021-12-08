using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2021.Week2
{
    class Day8
    {
        public static void Execute()
        {
            var outputDigits = File.ReadAllLines(@"Week2\input8.txt")
                .Select(X => 
                    X.Split('|')[1]
                        .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .ToArray())
                .ToList();

            var digits = File.ReadAllLines(@"Week2\input8.txt")
                .Select(x => x
                    .Replace("| ", "")
                    .Split(" ")
                    .ToArray())
                .ToList();

            Console.WriteLine(TaskA(outputDigits));
            Console.WriteLine(TaskB(digits));
        }

        public static int TaskA(List<string[]> digits) => digits.Sum(digit => digit.Count(x => x.Length is 2 or 3 or 4 or 7));

        public static int TaskB(List<string[]> digits) => digits.Select(Decode).Sum();

        private static int Decode(string[] digits)
        {
            digits = digits.Select(digit => digit.OrderBy(c => c))
                .Select(x => new string(x.ToArray()))
                .ToArray();

            // --- decode segments ---

            var decodedSegments = new char[7];

            var one = digits.First(x => x.Length == 2);
            var seven = digits.First(x => x.Length == 3);

            decodedSegments[0] = seven.First(c => !one.Contains(c));

            var four = digits.First(x => x.Length == 4);
            var s13 = four.Where(c => !one.Contains(c)).ToArray();

            var three = digits.First(x => x.Length == 5 && x.Contains(one[0]) && x.Contains(one[1]));
            var five = digits.First(x => x.Length == 5 && x.Contains(s13[0]) && x.Contains(s13[1]));

            decodedSegments[6] = three.First(c => !decodedSegments.Contains(c) && !four.Contains(c));

            (decodedSegments[5], decodedSegments[2]) = five.Contains(one[0]) ? (one[0], one[1]) : (one[1], one[0]);
            (decodedSegments[3], decodedSegments[1]) = three.Contains(s13[0]) ? (s13[0], s13[1]) : (s13[1], s13[0]);

            decodedSegments[4] = "abcdefg".First(c => !decodedSegments.Contains(c));

            // --- decode output number ---
            
            return digits[10..]
                .Select(digit => new string(decodedSegments.Select(c => digit.Contains(c) ? '1' : '0').ToArray()))
                .Aggregate(0, (current, numberBySegments) => current * 10 + numberBySegments switch
                {
                    "1110111" => 0,
                    "0010010" => 1,
                    "1011101" => 2,
                    "1011011" => 3,
                    "0111010" => 4,
                    "1101011" => 5,
                    "1101111" => 6,
                    "1010010" => 7,
                    "1111111" => 8,
                    "1111011" => 9
                });
        }
    }
}
