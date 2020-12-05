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
            List<int> data = new List<int>();
            var lines = File.ReadAllLines(@"Week1\input5.txt");
            foreach (var line in lines)
                data.Add(Convert_to_binary(line));

            int result_A = data.Max();
            int result_B = Task_B(data);

            Console.WriteLine(result_A);
            Console.WriteLine(result_B);
        }

        private static int Convert_to_binary(string line)
        {
            string binary_number = line.Replace('F', '0').Replace('B', '1').Replace('L', '0').Replace('R', '1');
            return Convert.ToInt32(binary_number, 2);
        }

        private static int Task_B(List<int>results)
        {
            var ordered = results.OrderBy(x => x).ToList();
            for(int i = ordered[1]; i<ordered.Count; i++)
                if (ordered[i] - ordered[i - 1] > 1)
                    return ordered[i] - 1;
            return -1;
        }
    }
}