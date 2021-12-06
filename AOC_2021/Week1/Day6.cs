using System;
using System.IO;
using System.Linq;

namespace Advent._2021.Week1
{
    class Day6
    {
        public static void Execute()
        {
            var lanternFishTimes = File.ReadAllText(@"Week1\input6.txt").Split(',').Select(int.Parse).ToArray();

            Console.WriteLine(Task(lanternFishTimes, 80));
            Console.WriteLine(Task(lanternFishTimes, 256));
        }

        public static long Task(int[] lanternFishTime, int days)
        {
            var times = new long[9];
            foreach (var internalTime in lanternFishTime)
                times[internalTime]++;

            for (int day = 0; day < days; day++)
                times[(day + 7) % 9] += times[day % 9];

            return times.Sum();
        }
    }
}