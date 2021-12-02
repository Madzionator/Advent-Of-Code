using System;
using System.IO;
using System.Linq;

namespace Advent._2021.Week1
{
    class Day1
    {
        public static void Execute()
        {
            var file = File.ReadAllLines(@"Week1\input1.txt");
            var measurements = file.Select(int.Parse).ToArray();

            Console.WriteLine(TaskA(measurements));
            Console.WriteLine(TaskB(measurements));
        }

        public static int TaskA(int[] measurements)
        {
            int increased = 0;

            for(int i = 1; i < measurements.Length; i++ )
                if ((measurements[i] > measurements[i - 1]))
                    increased++;

            return increased;
        }

        public static int TaskB(int[] measurements)
        {
            int increased = 0;

            for (int i = 3; i < measurements.Length; i++)
                if (measurements[i] > measurements[i - 3])
                    increased++;

            return increased;
        }
    }
}
