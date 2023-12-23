using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.Helpers.Extensions
{
    public static class HashSetUtils
    {
        public static void AddRange<T>(this HashSet<T> hashSet, HashSet<T> toAdd)
        {
            foreach (var element in toAdd)
            {
                hashSet.Add(element);
            }
        }

        public static void DrawMap(this HashSet<(int, int)> map)
        {
            var maxX = map.Select(w => w.Item1).Max();
            var minX = map.Select(w => w.Item1).Min();
            var maxY = map.Select(w => w.Item2).Max();
            var minY = map.Select(w => w.Item2).Min();

            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                    Console.Write(map.Contains((x, y)) ? '#' : ' ');
                Console.Write('\n');
            }
        }

        public static void DrawMap(this HashSet<(int, int)> map, int fromX, int toX, int fromY, int toY)
        {
            var sb = new StringBuilder();
            for (var y = fromY; y <= toY; y++)
            {
                for (var x = fromX; x <= toX; x++)
                {
                    sb.Append(map.Contains((x, y)) ? "#" : " ");
                }

                sb.AppendLine();
            }
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(sb);
        }
    }
}