using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

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

        public static int TaskA(List<string[]> digits) => digits
            .Sum(digit => digit
                .Select(x => x.Length)
                .Count(y => (y == 2 || y == 4 || y == 3 || y == 7)));

        public static int TaskB(List<string[]> digits) => digits.Select(Decode).Sum();

        private static int Decode(string[] digits)
        {
            digits = digits.Select(digit => digit.OrderBy(c => c))
                .Select(x => x.ToArray())
                .Select(x => new string(x))
                .ToArray();

            // --- decode segments ---

            var decodedSegments = new char[7];

            var one = digits.FirstOrDefault(x => x.Length == 2);
            var seven = digits.FirstOrDefault(x => x.Length == 3);
            decodedSegments[0] = seven.FirstOrDefault(x => !one.Contains((char)x));

            var four = digits.FirstOrDefault(x => x.Length == 4);
            var s13 = four.Where(x => !one.Contains(x)).ToArray();

            var three = digits.FirstOrDefault(x => x.Length == 5 && x.Contains(one[0]) && x.Contains(one[1]));
            var five = digits.FirstOrDefault(x => x.Length == 5 && x.Contains(s13[0]) && x.Contains(s13[1]));

            decodedSegments[6] = three.FirstOrDefault(x => !decodedSegments.Contains(x) && !four.Contains((char)x));

            decodedSegments[5] = five.Contains(one[0]) ? one[0] : one[1];
            decodedSegments[2] = five.Contains(one[0]) ? one[1] : one[0];

            decodedSegments[3] = three.Contains(s13[0]) ? s13[0] : s13[1];
            decodedSegments[1] = three.Contains(s13[0]) ? s13[1] : s13[0];

            decodedSegments[4] = "abcdefg".FirstOrDefault(x => !decodedSegments.Contains(x));

            // --- decode output number ---

            int decodedNumber = 0;
            for (int i = 10; i < 14; i++)
            {
                var codedNumber = new char[7];
                for(int j = 0; j<7; j++)
                    if (digits[i].Contains(decodedSegments[j]))
                        codedNumber[j] = '1';
                    else
                        codedNumber[j] = '0';

                switch (new string(codedNumber))
                {
                    case "1110111":
                        decodedNumber *= 10;
                        break;
                    case "0010010":
                        decodedNumber = decodedNumber * 10 + 1;
                        break;
                    case "1011101":
                        decodedNumber = decodedNumber * 10 + 2;
                        break;
                    case "1011011":
                        decodedNumber = decodedNumber * 10 + 3;
                        break;
                    case "0111010":
                        decodedNumber = decodedNumber * 10 + 4;
                        break;
                    case "1101011":
                        decodedNumber = decodedNumber * 10 + 5;
                        break;
                    case "1101111":
                        decodedNumber = decodedNumber * 10 + 6;
                        break;
                    case "1010010":
                        decodedNumber = decodedNumber * 10 + 7;
                        break;
                    case "1111111":
                        decodedNumber = decodedNumber * 10 + 8;
                        break;
                    case "1111011":
                        decodedNumber = decodedNumber * 10 + 9;
                        break;
                }
            }

            return decodedNumber;
        }
    }
}
