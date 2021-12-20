using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2021.Week3
{
    class Day20
    {
        public static (int min, int max) X;
        public static (int min, int max) Y;

        public static void Execute()
        {
            var file = File.ReadAllLines(@"Week3\input20.txt");
            var rules = file[0].Replace('#', '1').Replace('.', '0');

            var points = new Dictionary<(int y, int x), char>();
            for (var y = 2; y < file.Length; y++)
                for (var x = 0; x < file[2].Length; x++)
                    points[(y - 2, x)] = file[y][x] == '#' ? '1' : '0';

            Y = (0, file.Length - 3);
            X = (0, file[2].Length - 1);

            Console.WriteLine(Task(points.ToDictionary(p => p.Key, x => x.Value), rules, 2));   //TaskA
            Console.WriteLine(Task(points, rules, 50));     //TaskB
        }

        public static int Task(Dictionary<(int y, int x), char> points, string rules, int steps)
        {
            for (var step = 0; step < steps; step++)
            {
                var infinity = '0';
                if (rules[0] == '1')
                    infinity = step % 2 == 1 ? rules[0] : rules[^1];

                var pointsCopy = points.ToDictionary(p => p.Key, p => p.Value);
                for (var i = Y.min - 1; i <= Y.max + 1; i++)
                    for (var j = X.min - 1; j <= X.max + 1; j++)     // check every discovered point and its border
                    {
                        var binaryValue = "";                           //image enhancement algorithm
                        for (var ny = i - 1; ny <= i + 1; ny++)
                            for (var nx = j - 1; nx <= j + 1; nx++)
                                if (pointsCopy.ContainsKey((ny, nx)))
                                    binaryValue += pointsCopy[(ny, nx)];
                                else
                                    binaryValue += infinity;

                        var value = Convert.ToInt32(binaryValue, 2);
                        points[(i, j)] = rules[value];
                    }

                X = (X.min - 1, X.max + 1);
                Y = (Y.min - 1, Y.max + 1);
            }

            return points.Count(p => p.Value == '1');
        }
    }
}