using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2020.Week3
{
    public class Day16
    {
        public static void Execute()
        {
            (var rules, var myTicket, var tickets) = ReadData();

            int resultA = TaskA(rules, tickets);
            long resultB = TaskB(rules, myTicket, tickets);

            Console.WriteLine(resultA);
            Console.WriteLine(resultB);
        }

        private static (List<int[]>, int[], List<int[]>) ReadData()
        {
            var data = File.ReadAllLines(@"Week3\input16.txt").ToList();
            int nextDataCategoty = data.IndexOf("your ticket:");

            var rules = new List<int[]>();
            for (int i = 0; i < nextDataCategoty - 1; i++)
            {
                var line = data[i].Replace("or ", "").Replace("-", " ").Split(" ").ToList();
                var parameter = line.GetRange(line.Count - 4, 4);
                var x = parameter.Select(int.Parse).ToArray();
                rules.Add(x);
            }

            var myTicket = data[nextDataCategoty + 1].Split(",").Select(int.Parse).ToArray();

            var tickets = new List<int[]>();
            for (int i = nextDataCategoty + 4; i < data.Count; i++)
                tickets.Add(data[i].Split(",").Select(int.Parse).ToArray());

            return (rules, myTicket, tickets);
        }

        private static int TaskA(List<int[]> rules, List<int[]> tickets)
        {
            int result = 0;
            for (var t = tickets.Count - 1; t>= 0; t--)
                foreach (var value in tickets[t])
                    if (!IsValid(value))
                    {
                        result += value;
                        tickets.RemoveAt(t);
                    }

            return result;

            bool IsValid(int val)
            {
                foreach (var rule in rules)
                    if ((rule[0] <= val && val <= rule[1]) || (rule[2] <= val && val <= rule[3]))
                        return true;
                return false;
            }
        }

        private static long TaskB(List<int[]> rules, int[] myTicket, List<int[]> tickets)
        {
            int quantity = rules.Count;
            var data = rules.Select(x => new int[quantity]).ToList();
            CompletetArray();
            var solution = new Dictionary<int, int>();

            while (solution.Count < quantity)
                for (int rule = 0; rule < quantity; rule++)
                {
                    int k = 0;
                    int position = 0;
                    for (int field = 0; field < quantity; field++)
                        if (data[rule][field] == tickets.Count)
                        {
                            k++;
                            position = field;
                        }
                    if (k == 1)
                    {
                        solution.Add(rule, position);
                        data[rule] = new int[quantity];
                        foreach (var r in data)
                            r[position] = 0;
                    }
                }

            long result = 1;
            for (int i = 0; i<6; i++)
                result *= myTicket[solution[i]];

            return result;

            void CompletetArray()
            {
                foreach (var ticket in tickets)
                    for (int fild = 0; fild < quantity; fild++)
                        for (int rule = 0; rule < quantity; rule++)
                            if ((rules[rule][0] <= ticket[fild] && ticket[fild] <= rules[rule][1]) || (rules[rule][2] <= ticket[fild] && ticket[fild] <= rules[rule][3]))
                                data[rule][fild]++;
            }
        }
    }
}