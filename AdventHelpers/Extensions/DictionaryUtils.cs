using System;
using System.Collections.Generic;
using System.Text;

namespace Advent.Helpers.Extensions
{
    public static class DictionaryUtils
    {
        public static void AddOrSet<T>(this Dictionary<T, int> dictionary, T key, int value)
            => dictionary[key] = dictionary.ContainsKey(key) ? dictionary[key] + value : value;

        public static void AddOrSet<T>(this Dictionary<T, long> dictionary, T key, long value)
            => dictionary[key] = dictionary.ContainsKey(key) ? dictionary[key] + value : value;

        public static void DrawMap(this Dictionary<(int, int), char> map, int fromX, int toX, int fromY, int toY)
        {
            var sb = new StringBuilder();
            for (var y = fromY; y <= toY; y++)
            {
                for (var x = fromX; x <= toX; x++)
                {
                    sb.Append(map.ContainsKey((x, y)) 
                        ? map[(x, y)].ToString() 
                        : " ");
                }

                sb.AppendLine();
            }
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(sb);
        }
    }
}