using System;
using System.Collections.Generic;
using System.IO;
using Advent.Helpers.Extensions;

namespace Advent._2021.Week3
{
    class Day15
    {
        public static void Execute()
        {
            var grid = File.ReadAllLines(@"Week3\input15.txt").ToMatrix();

            Console.WriteLine(TaskA(grid));
            Console.WriteLine(TaskB(grid));
        }

        public static int TaskA(int[,] grid) => LowestTotalRisk(grid);
        public static int TaskB(int[,] grid) => LowestTotalRisk(MakeBigGrid(grid));

        public static int LowestTotalRisk(int[,] grid)
        {
            var maxX = grid.GetLength(0);
            var maxY = grid.GetLength(1);

            var costs = new int[maxX, maxY];
            var queue = new Queue<(int x, int y)>();
            var queueValue = new Dictionary<(int x, int y), int>();

            queue.Enqueue((0, 0));
            queueValue[(0, 0)] = 0;

            while (queue.Count > 0)
            {
                var (x, y) = queue.Dequeue();
                var newQVal = queueValue[(x, y)] + grid[x, y];
                queueValue.Remove((x, y));

                costs[x, y] = newQVal;

                TryAddToQueue(x - 1, y, newQVal);
                TryAddToQueue(x, y - 1, newQVal);
                TryAddToQueue(x + 1, y, newQVal);
                TryAddToQueue(x, y + 1, newQVal);
            }

            void TryAddToQueue(int x, int y, int val)
            {
                if (x < 0 || y < 0 || x >= maxX || y >= maxY || costs[x, y] < val + grid[x, y] && costs[x, y] > 0)
                    return;

                if (queueValue.ContainsKey((x, y)))
                {
                    if (queueValue[(x, y)] >= val)
                        queueValue[(x, y)] = val;
                    return;
                }

                queue.Enqueue((x, y));
                queueValue[(x, y)] = val;
            }

            return costs[maxX - 1, maxY - 1] - costs[0, 0];
        }

        public static int[,] MakeBigGrid(int[,] grid)
        {
            var maxX = grid.GetLength(0);
            var maxY = grid.GetLength(1);
            var newGrid = new int[maxX * 5, maxY * 5];

            for (var x = 0; x < 5 * maxX; x++)
                for (var y = 0; y < 5 * maxY; y++)
                {
                    var increment = x / maxX + y / maxY;
                    newGrid[x, y] = grid[x % maxX, y % maxY] + increment;
                    if (newGrid[x, y] > 9)
                        newGrid[x, y] = newGrid[x, y] % 10 + 1;
                }

            return newGrid;
        }
    }
}
