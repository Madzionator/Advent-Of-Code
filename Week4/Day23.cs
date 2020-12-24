using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2020.Week4
{
    public class Day23
    {
        public static void Execute()
        {
            var data = File.ReadAllText(@"Week4\input23.txt").ToString();
            var cups = new List<int>();
            foreach (var x in data)
                cups.Add(int.Parse(x.ToString()));

            string resultA = TaskA(cups.ToList());
            long resultB = TaskB(cups);

            Console.WriteLine(resultA);
            Console.WriteLine(resultB);
        }

        private static string TaskA(List<int> cupsList)
        {
            var partResult = Game(cupsList, 'A', 9, 100);
            string result = "";
            for (var i = partResult.IndexOf(1) + 1; i < 9; i++)
                result += partResult[i].ToString();
            for (var i = 0; i < partResult.IndexOf(1); i++)
                result += partResult[i].ToString();

            return result;
        }

        private static long TaskB(List<int>cupsList)
        {
            for (int i = 10; i <= 1_000_000; i++)
                cupsList.Add(i);
            var partResult = Game(cupsList, 'B', 1_000_000, 10_000_000);
            return (long)partResult[0] * partResult[1];
        }

        private static List<int> Game(List<int> cupsList, char task, int max, int moves)
        {
            var cups = new LinkedList<int>();
            var dict = new Dictionary<int, LinkedListNode<int>>();
            foreach (var cup in cupsList)
            {
                cups.AddLast(cup);
                dict.Add(cups.Last.Value, cups.Last);
            }

            var currentCup = cups.First;
            for (int move = 1; move <= moves; move++)
            {
                var picked = new LinkedListNode<int>[3];
                picked[0] = currentCup.Next();
                picked[1] = picked[0].Next();
                picked[2] = picked[1].Next();
                cups.Remove(picked[0]);
                cups.Remove(picked[1]);
                cups.Remove(picked[2]);

                var toFind = currentCup.Value;
                do
                {
                    toFind--;
                    if (toFind < 1)
                        toFind = max;

                } while (toFind == picked[0].Value || toFind == picked[1].Value || toFind == picked[2].Value);
                var destination = dict[toFind];

                cups.AddAfter(destination, picked[2]);
                cups.AddAfter(destination, picked[1]);
                cups.AddAfter(destination, picked[0]);

                currentCup = currentCup.Next();
            }

            var idx = dict[1];
            if (task == 'A')
                return cups.ToList();
            else
            {
                var res = new List<int>();
                var nr1 = idx.Next();
                res.Add(nr1.Value);
                res.Add(nr1.Next.Value);
                return res;
            }
        }
    }

    internal static class LinkedListExtensions
    {
        public static LinkedListNode<T> Next<T>(this LinkedListNode<T> node)
        {
            return node.Next ?? node.List.First;
        }
    }
}