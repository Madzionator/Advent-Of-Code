using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2020.Week3
{
    public class Day18
    {
        public static void Execute()
        {
            var data = File.ReadAllLines(@"Week3\input18.txt");

            var results = Task(data);
            long resultA = results.Item1;
            long resultB = results.Item2;

            Console.WriteLine(resultA);
            Console.WriteLine(resultB);
        }

        private static (long, long) Task(string[] data)
        {
            long resultA = 0;
            long resultB = 0;

            foreach (var line in data)
            {
                var elements = GetElements(line.Replace(" ", ""));
                resultA += SumA(elements.ToList());
                resultB += SumB(elements.ToList());
            }

            return (resultA, resultB);
        }

        private static long SumA(List<string> elements)
        {
            while (elements.Count > 1)
            {
                int start = 0; int end = elements.Count - 1;
                for (int i = 0; i < elements.Count; i++)
                {
                    if (elements[i] == "(")
                        start = i + 1;
                    if (elements[i] == ")")
                    {
                        end = i - 1;
                        elements.RemoveAt(end + 1);
                        elements.RemoveAt(start - 1);
                        start--;
                        break;
                    }
                }

                for (int i = start; i < end - 1; end -= 2)
                {
                    string res = "";
                    switch (elements[i + 1])
                    {
                        case "+":
                            res = (long.Parse(elements[i]) + long.Parse(elements[i + 2])).ToString();
                            break;
                        case "*":
                            res = (long.Parse(elements[i]) * long.Parse(elements[i + 2])).ToString();
                            break;
                    }
                    elements[start] = res;
                    elements.RemoveAt(start + 2);
                    elements.RemoveAt(start + 1);
                }

            }
            return long.Parse(elements[0]);
        }

        private static long SumB(List<string> elements)
        {
            while (elements.Count > 1)
            {
                int start = 0; int end = elements.Count - 1;
                for (int i = 0; i < elements.Count; i++)
                {
                    if (elements[i] == "(")
                        start = i + 1;
                    if (elements[i] == ")")
                    {
                        end = i - 1;
                        elements.RemoveAt(end + 1);
                        elements.RemoveAt(start - 1);
                        start--;
                        break;
                    }
                }

                for (int i = start; i < end - 1;)
                    switch (elements[i + 1])
                    {
                        case "+":
                            elements[i] = (long.Parse(elements[i]) + long.Parse(elements[i + 2])).ToString();
                            elements.RemoveAt(i + 2);
                            elements.RemoveAt(i + 1);
                            end -= 2;
                            break;
                        case "*":
                            i += 2;
                            break;
                    }

                for (int i = start; i < end - 1; end -= 2)
                {
                    elements[start] = (long.Parse(elements[i]) * long.Parse(elements[i + 2])).ToString();
                    elements.RemoveAt(start + 2);
                    elements.RemoveAt(start + 1);
                }
            }
            return long.Parse(elements[0]);
        }

        private static List<string> GetElements(string str)
        {
            var elements = new List<string>();
            var last = -1;
            for (var i = 0; i < str.Length; i++)
                if (!(str[i] >= '0' && str[i] <= '9'))
                {
                    if (str.Substring(last + 1, i - 1 - last) != "")
                        elements.Add(str.Substring(last + 1, i - 1 - last));
                    if (str[i].ToString() != "")
                        elements.Add(str[i].ToString());
                    last = i;
                }
            if (last < str.Length - 1)
                elements.Add(str.Substring(last + 1, str.Length - 1 - last));

            return elements;
        }
    }
}