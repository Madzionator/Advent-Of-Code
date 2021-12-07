using System;
using System.IO;
using System.Linq;

namespace Advent._2021.Week1
{
    class Day7
    {
        public static void Execute()
        {
            var positions = File.ReadAllText(@"Week1\input7.txt").Split(',').Select(int.Parse).ToArray();

            Console.WriteLine(Task(positions, x => x));                 // TaskA
            Console.WriteLine(Task(positions, x => x * (1 + x) / 2));   // TaskB
        }

        public static int Task(int[] positions, Func<int, int> operation) => 
            Enumerable.Range(positions.Min(), positions.Max() + 1)
                .Select(x => positions
                    .Select(position => Math.Abs(position - x))
                    .Select(move => operation(move))
                    .Sum()).Min();
    }
}