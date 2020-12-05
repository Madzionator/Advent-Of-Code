using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2020.Week1
{
    public class Day5
    {
        public static void Execute()
        {
            var lines = File.ReadAllLines(@"Week1\input5.txt");
            int result_A_max = 0;
            List<int> list = new List<int>();
            foreach (var line in lines)
            {
                int result = Task_A(line);
                list.Add(result);
                if (result > result_A_max)
                    result_A_max = result;
            }
            
            int result_B = Task_B(list);
            Console.WriteLine(result_A_max);
            Console.WriteLine(result_B);
        }

        private static int Task_A(string line)
        {
            string row = line.Substring(0, 7).Replace('F', '0').Replace('B', '1');
            string column = line.Substring(7, line.Length - 7).Replace('L', '0').Replace('R', '1');
            int id = Convert.ToInt32(row, 2) * 8 + Convert.ToInt32(column, 2);
            return id;
        }

        private static int Task_B(List<int>results)
        {
            var ordered_results = results.OrderBy(x => x).ToList();
            for(int i = ordered_results[1]; i<ordered_results.Count; i++)
            {
                if (ordered_results[i] - ordered_results[i - 1] > 1)
                    return ordered_results[i] - 1;
            }
            return -1;
        }
    }
}