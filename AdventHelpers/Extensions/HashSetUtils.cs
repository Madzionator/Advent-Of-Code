using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.Helpers.Extensions
{
    public static class HashSetUtils
    {
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
    }
}