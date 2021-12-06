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
            long[] times = new long[9];
            foreach (var internalTime in lanternFishTime)
                times[internalTime]++;

            for (int day = 1; day <= days; day++)
            {
                long temp = times[0];
                for (int i = 1; i <= 8; i++)
                    times[i - 1] = times[i];
                times[6] += temp;
                times[8] = temp;
            }

            return times.Sum();
        }
    }
}
