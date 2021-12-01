using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent._2021.Week1
{
    class Day1
    {
        public static void Execute()
        {
            var file = File.ReadAllLines(@"Week1\input1.txt");
            var measurements = file.Select(int.Parse).ToArray();

            Console.WriteLine(Task_A(measurements));
            Console.WriteLine(Task_B(measurements));
        }

        public static int Task_A(int [] measurements)
        {
            int increased = 0;
            int measured = measurements.Count();

            for(int i = 1; i < measured; i++ )
                if ((measurements[i] - measurements[i - 1]) > 0)
                    increased++;

            return increased;
        }

        public static int Task_B(int[] measurements)
        {
            int increased = 0;
            int measured = measurements.Count();

            for (int i = 3; i < measured; i++)
                if (measurements[i] - measurements[i - 3] > 0)
                    increased++;

            return increased;
        }
    }
}
