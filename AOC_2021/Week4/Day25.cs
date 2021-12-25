using System;
using System.IO;
using System.Linq;
using Advent.Helpers.Extensions;

namespace Advent._2021.Week4
{
    class Day25
    {
        public static int Y;
        public static int X;
        public static void Execute()
        {
            var map = File.ReadAllLines(@"Week4\input25.txt").ToCharMatrix();
            Y = map.GetLength(0);
            X = map.GetLength(1);

            Console.WriteLine(TaskA(map));
        }

        public static int TaskA(char[,] map)
        {
            var positions = (from y in Enumerable.Range(0, Y) from x in Enumerable.Range(0, X) select (y, x)).ToList();

            var steps = 0;
            bool sthWasChanged;
            char[,] copy;

            do
            {
                steps++;
                sthWasChanged = false;

                copy = map.CopyMatrix();
                positions.ForEach(TryMoveHorizontally);

                copy = map.CopyMatrix();
                positions.ForEach(TryMoveVertically);

            } while (sthWasChanged);

            return steps;

            void TryMoveHorizontally((int y, int x) p)
            {
                if (copy[p.y, p.x] != '>')
                    return;
                var next = (p.x + 1) % X;
                if (copy[p.y, next] == '.')
                {
                    map[p.y, next] = '>';
                    map[p.y, p.x] = '.';
                    sthWasChanged = true;
                }
            }

            void TryMoveVertically((int y, int x) p)
            {
                if (copy[p.y, p.x] != 'v')
                    return;
                var next = (p.y + 1) % Y;
                if (copy[next, p.x] == '.')
                {
                    map[next, p.x] = 'v';
                    map[p.y, p.x] = '.';
                    sthWasChanged = true;
                }
            }
        }
    }
}
