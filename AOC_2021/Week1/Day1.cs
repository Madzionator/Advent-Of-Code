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
            var report = file.Select(int.Parse).ToList();

            Console.WriteLine(Task_A(report));
            Console.WriteLine(Task_B(report));
        }

        public static int Task_A(List<int> report)
        {
            return 0;
        }

        public static int Task_B(List<int> report)
        {
            return 0;
        }
    }
}
