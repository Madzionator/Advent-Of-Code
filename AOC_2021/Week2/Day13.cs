using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent.Helpers.Extensions;

namespace Advent._2021.Week2
{
    class Day13
    {
        private static HashSet<(int, int)> dots;
        private static List<(char, int)> folds;

        public static void Execute()
        {
            dots = new HashSet<(int, int)>();
            folds = new List<(char, int)>();
            var file = File.ReadAllLines(@"Week2\input13.txt");

            foreach (var l in file)
            {
                if (l.Contains(','))
                {
                    var x = l.Split(',');
                    dots.Add((int.Parse(x[0]), int.Parse(x[1])));
                }
                else if (l.Contains('='))
                {
                    var x = l.Split('=');
                    folds.Add((x[0][^1], int.Parse(x[1])));
                }
            }

            Console.WriteLine(TaskA());
            TaskB();
        }

        public static int TaskA() => Fold(0, 1);
        public static void TaskB()
        {
            Fold(1, folds.Count);
            dots.DrawMap();
        }

        public static int Fold(int startIdx, int stopIdx)
        {
            for (var i = startIdx; i < stopIdx; i++)
            {
                if (folds[i].Item1 == 'x')
                {
                    var val = folds[i].Item2;
                    var toAdd = (from dot in dots where dot.Item1 > val let newX = 2 * val - dot.Item1 select (newX, dot.Item2)).ToList();
                    foreach (var t in toAdd.Where(t => !dots.Contains(t)))
                        dots.Add(t);

                    dots.RemoveWhere(x => x.Item1 >= val);
                }
                else
                {
                    var val = folds[i].Item2;
                    var toAdd = (from dot in dots where dot.Item2 > val let newY = 2 * val - dot.Item2 select (dot.Item1, newY)).ToList();
                    foreach (var t in toAdd.Where(t => !dots.Contains(t)))
                        dots.Add(t);

                    dots.RemoveWhere(y => y.Item2 >= val);
                }
            }

            return dots.Count;
        }
    }
}
