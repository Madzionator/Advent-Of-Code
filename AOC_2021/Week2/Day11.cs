using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2021.Week2
{
    class Day11
    {
        public static void Execute()
        {
            var lines = File.ReadAllLines(@"Week2\input11.txt");
            var cavern = new int[10, 10];
            for (var i = 0; i < 10; i++)
                for (var j = 0; j < 10; j++)
                    cavern[i, j] = lines[i][j] - '0';

            var (ansA, ansB) = Task(cavern);

            Console.WriteLine(ansA);
            Console.WriteLine(ansB);
        }

        public static (int, int) Task(int[,] cavern)
        {
            var adjacent = new[] { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };
            var flashes = 0;

            for (var step = 1; step <= 500; step++)
            {
                if (cavern.Cast<int>().Count(c => c == 0) == 100)
                    return (flashes, step - 1);

                for (var i = 0; i < 10; i++)
                    for (var j = 0; j < 10; j++)
                        cavern[i, j]++;

                while (true)
                {
                    var highlighted = new HashSet<(int, int)>();
                    for (var y = 0; y < cavern.GetLength(0); y++)
                        for (var x = 0; x < cavern.GetLength(1); x++)
                            if (cavern[y, x] > 9)
                            {
                                cavern[y, x] = 0;
                                highlighted.Add((y, x));
                            }

                    if (highlighted.Count < 1)
                        break;

                    if (step <= 100)
                        flashes += highlighted.Count;

                    foreach (var (y, x) in highlighted)
                        foreach (var (dy, dx) in adjacent)
                            if (y + dy is >= 0 and < 10 && x + dx is >= 0 and < 10 && cavern[y + dy, x + dx] != 0)
                                cavern[y + dy, x + dx]++;
                }
            }

            return (flashes, -1);
        }
    }
}
