using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SnailFishNumberList = System.Collections.Generic.List<(int val, int depth)>;

namespace Advent._2021.Week3
{
    class Day18
    {
        public static void Execute()
        {
            var file = File.ReadAllLines(@"Week3\input18.txt");
            var reducedInput = file.Select(ReadList).ToList();

            Console.WriteLine(TaskA(reducedInput));
            Console.WriteLine(TaskB(reducedInput));
        }

        public static int TaskA(List<SnailFishNumberList> reducedNumbers)
        {
            var resultNumber = reducedNumbers[0];
            foreach (var number in reducedNumbers.Skip(1))
            {
                resultNumber = ConnectList(resultNumber, number);
                Reduce(resultNumber);
            }

            return CalculateMagnitude(resultNumber);
        }

        public static int TaskB(List<SnailFishNumberList> reducedNumbers) => Enumerable.Range(0, 100)
                .SelectMany(l => Enumerable.Range(0, 100), (l, r) => (l, r))
                .Where(x => x.l != x.r)
                .Select(x =>
                {
                    var sum = ConnectList(reducedNumbers[x.l], reducedNumbers[x.r]);
                    Reduce(sum);
                    return CalculateMagnitude(sum);
                })
                .Max();

        public static SnailFishNumberList ConnectList(SnailFishNumberList A, SnailFishNumberList B) => A.Concat(B).Select(x => (x.val, x.depth+1)).ToList();

        public static SnailFishNumberList ReadList(string line)
        {
            var list = new SnailFishNumberList();
            var br = 0;
            foreach (var c in line)
            {
                switch (c)
                {
                    case '[':
                        br++;
                        break;
                    case ']':
                        br--;
                        break;
                    case >= '0' and <= '9':
                        list.Add((c - '0', br));
                        break;
                }
            }

            Reduce(list);
            return list;
        }

        public static void Reduce(SnailFishNumberList list)
        {
            do
            {
                for (var i = 0; i < list.Count; i++)
                {
                    if (i < list.Count - 1 && list[i].depth > 4 && list[i + 1].depth == list[i].depth) // explode
                    {
                        if (i > 0)
                            list[i - 1] = (list[i - 1].val + list[i].val, list[i - 1].depth);

                        if (i < list.Count - 2)
                            list[i + 2] = (list[i + 2].val + list[i + 1].val, list[i + 2].depth);

                        list[i] = (0, list[i].depth - 1);
                        list.RemoveAt(i + 1);
                    }
                }

                for (var i = 0; i < list.Count; i++)
                {
                    if (list[i].val > 9) //split
                    {
                        var left = list[i].val / 2;
                        var right = (int)Math.Ceiling(list[i].val / 2.0);

                        list[i] = (left, list[i].depth + 1);
                        list.Insert(i + 1, (right, list[i].depth));
                        break;
                    }
                }

            } while (list.Any(x => x.depth > 4) || list.Any(x=>x.val > 9));
        }

        public static int CalculateMagnitude(SnailFishNumberList list)
        {
            while (list.Count > 1)
                for (int i = 0; i < list.Count - 1; i++)
                    if (list[i].depth == list[i + 1].depth)
                    {
                        list[i] = (list[i].val * 3 + list[i + 1].val * 2, list[i].depth - 1);
                        list.RemoveAt(i + 1);
                        i = -1;
                    }

            return list[0].val;
        }
    }
}


