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

            long resultA = TaskA(cups.ToList());
            long resultB = TaskB(cups);

            Console.WriteLine(resultA);
            Console.WriteLine(resultB);
        }

        private static long TaskA(List<int> cupsList)
        {
            return Game(cupsList, 'A', 9, 100);
        }

        private static long TaskB(List<int>cupsList)
        {
            for (int i = 10; i <= 1_000_000; i++)
                cupsList.Add(i);
            return Game(cupsList, 'B', 1_000_000, 10_000_000);
        }

        private static long Game(List<int> cupsList, char task, int max, int moves)
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
            {
                string result = "";
                for (var element = idx.Next(); element.Value > 1; element = element.Next())
                    result += element.Value.ToString();
                return long.Parse(result);
            }
            else
            {
                var nr1 = idx.Next();
                return (long)nr1.Value * nr1.Next().Value;
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