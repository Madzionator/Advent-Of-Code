using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2021.Week2
{
    class Day10
    {
        public static void Execute()
        {
            var lines = File.ReadAllLines(@"Week2\input10.txt")
                .Select(x => x.Replace(')', '*'))
                .ToArray();

            Console.WriteLine(TaskA(lines));
            Console.WriteLine(TaskB(lines));
        }

        public static int TaskA(string[] lines) => lines.Select(CalculateCorrupted).Sum();

        public static long TaskB(string[] lines)
        {
            var totalScores = lines.Select(CalculateIncomplete).Where(score => score > 0).ToList();
            totalScores.Sort();

            return totalScores[totalScores.Count / 2];
        }

        private static int CalculateCorrupted(string line)
        {
            var brackets = new Stack<char>();

            foreach (var c in line)
            {
                if (c is '(' or '<' or '[' or '{')
                {
                    brackets.Push(c);
                    continue;
                }

                if (c - brackets.Pop() == 2)
                    continue;

                return c switch
                {
                    '*' => 3,
                    ']' => 57,
                    '}' => 1197,
                    '>' => 25137
                };
            }

            return 0;
        }

        private static long CalculateIncomplete(string line)
        {
            var brackets = new Stack<char>();
            var isCorrupted = false;

            foreach (var c in line)
            {
                if (c is '(' or '<' or '[' or '{')
                {
                    brackets.Push(c);
                    continue;
                }

                if (c - brackets.Pop() == 2) 
                    continue;

                isCorrupted = true;
                break;
            }

            if (brackets.Count == 0 || isCorrupted)
                return 0;

            return brackets.Aggregate<char, long>(0, (current, br) => current * 5 + br switch
            {
                '(' => 1,
                '[' => 2,
                '{' => 3,
                '<' => 4
            });
        }
    }
}
