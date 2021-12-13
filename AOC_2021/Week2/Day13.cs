using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent.Helpers.Extensions;

namespace Advent._2021.Week2
{
    class Day13
    {
        private static HashSet<(int x, int y)> dots;
        private static List<(char, int)> folds;

        public static void Execute()
        {
            dots = new HashSet<(int x, int y)>();
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

        public static int TaskA() => Fold(folds.Take(1).ToList());
        public static void TaskB()
        {
            Fold(folds.Skip(1).ToList());
            dots.DrawMap();
        }

        public static int Fold(List<(char, int)> folds)
        {
            foreach (var (dir, val) in folds)
            {
                if (dir == 'x')
                {
                    var toAdd = (from dot in dots where dot.x > val select (2 * val - dot.x, dot.y)).ToList();
                    foreach (var item in toAdd.Where(t => !dots.Contains(t)))
                        dots.Add(item);

                    dots.RemoveWhere(dot => dot.x >= val);
                }
                else
                {
                    var toAdd = (from dot in dots where dot.y > val select (dot.x, 2 * val - dot.y)).ToList();
                    foreach (var item in toAdd.Where(t => !dots.Contains(t)))
                        dots.Add(item);

                    dots.RemoveWhere(dot => dot.y >= val);
                }
            }

            return dots.Count;
        }
    }
}
