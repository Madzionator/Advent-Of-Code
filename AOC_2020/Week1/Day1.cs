using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2020.Week1
{
    public class Day1
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
            for (int i = 0; i < report.Count; i++)
                for (int j = 0; j < report.Count; j++)
                {
                    if (i == j)
                        continue;
                    if (report[i] + report[j] == 2020)
                        return report[i] * report[j];
                }

            return 0;
        }

        public static int Task_B(List<int> report)
        {
            for (int i = 0; i < report.Count; i++)
                for (int j = 0; j < report.Count; j++)
                {
                    if (i == j)
                        continue;
                    for (int k = 0; k < report.Count; k++)
                    {
                        if (j == k || i == k)
                            continue;
                        if (report[i] + report[j] + report[k] == 2020)
                            return report[i] * report[j] * report[k];
                    }

                }
            return 0;
        }
    }
}