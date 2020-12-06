using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2020.Week1
{
    public class Day6
    {
        public static void Execute()
        {
            var lines = File.ReadAllLines(@"Week1\input6.txt");
            var groups = new List<string>();
            string group = "";
            foreach (var line in lines)
            {
                if (line == "")
                {
                    groups.Add(group);
                    group = "";
                }
                else
                    group = group + " " + line;
            }
            groups.Add(group);

            int result_A = 0;
            int result_B = 0;
            foreach (var gr in groups)
            {
                var (a, b) = CountYes(gr);
                result_A += a;
                result_B += b;
            }
            Console.WriteLine(result_A);
            Console.WriteLine(result_B);
        }
        private static (int, int) CountYes(string line)
        {
            int[] answer = new int[26];
            int people = 0;
            for (int i = 0; i<line.Length; i++)
            {
                if (line[i] == ' ')
                    people++;
                else
                    answer[line[i] - 'a']++;
            }

            int anyone = 0;
            int everyone = 0;
            for(int i = 0; i<26; i++)
            {
                if (answer[i] > 0)
                    anyone++;

                if (answer[i] == people)
                    everyone++;
            }
            return (anyone, everyone);
        }
    }
}