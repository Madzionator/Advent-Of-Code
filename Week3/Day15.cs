using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2020.Week3
{
    public class Day15
    {
        public static void Execute()
        {
            var data = File.ReadAllText(@"Week3\input15.txt").Split(',').Select(x => int.Parse(x)).ToArray();

            var results = Task(data);
            int resultA = results.Item1;
            int resultB = results.Item2;

            Console.WriteLine(resultA);
            Console.WriteLine(resultB);
        }

        private static (int, int) Task(int[]startData)
        {
            int resultA = 0, resultB;
            var dict = new Dictionary<int, int>();
            for (int i = 1; i <= startData.Length; i++)
                dict.Add(startData[i - 1], i);

            int lastNumber = startData[^1];
            for(int i = startData.Length; i< 30000000; i++)
            {
                if(dict.ContainsKey(lastNumber))
                {
                    int newNumber = i - dict[lastNumber];
                    dict[lastNumber] = i;
                    lastNumber = newNumber;
                }
                else
                {
                    dict.Add(lastNumber, i);
                    lastNumber = 0;
                }

                if (i == 2020 - 1)
                    resultA = lastNumber;
            }
            resultB = lastNumber;
            return (resultA, resultB);
        }
    }
}