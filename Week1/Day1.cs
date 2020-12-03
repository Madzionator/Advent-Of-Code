using System;
using System.Collections.Generic;
using System.IO;

namespace Advent._2019.Week1
{
    public class Day1
    {
        public static void Execute()
        {
            //task 1
            string line;
            List<int> report = new List<int>();
            StreamReader file = new System.IO.StreamReader(@"Week1\input1.txt");
            while ((line = file.ReadLine()) != null)
            {
                report.Add(Int32.Parse(line));
            }
            int resultA = Find_2020_2(report);
            int resultB = Find_2020_3(report);
            file.Close();
            Console.WriteLine(resultA);
            Console.WriteLine(resultB);

        }

        public static int Find_2020_2(List<int> report)
        {
            int value1 = 0, value2 = 0;
            for (int i = 0; i < report.Count; i++)
            {
                for (int j = 0; j < report.Count; j++)
                {
                    if (i == j)
                        continue;
                    if (report[i] + report[j] == 2020)
                    {
                        value1 = report[i];
                        value2 = report[j];
                        break;
                    }
                }
                if (value1 + value2 == 2020)
                    break;
            }
            return value1 * value2;

        }

        public static int Find_2020_3(List<int> report)
        {
            int value1 = 0, value2 = 0, value3 = 0;
            for (int i = 0; i < report.Count; i++)
            {
                for (int j = 0; j < report.Count; j++)
                {
                    if (i == j)
                        continue;
                    for (int k = 0; k < report.Count; k++)
                    {
                        if (j == k || i == k)
                            continue;
                        if (report[i] + report[j] + report[k] == 2020)
                        {
                            value1 = report[i];
                            value2 = report[j];
                            value3 = report[k];
                            break;
                        }
                    }
                    if (value1 + value2 + value3 == 2020)
                        break;
                }
                if (value1 + value2 + value3 == 2020)
                    break;
            }
            return value1 * value2 * value3;

        }

    }
}