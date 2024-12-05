using System;
using System.Collections.Generic;
using System.Text;

namespace Advent.Helpers.Extensions
{
    public static class DictionaryUtils
    {
        /// <summary>
        /// If the key is not present in the dictionary, add the pair {key, value}.
        /// If the key already exists, update its value by adding the provided value to the existing one.
        /// </summary>
        public static void AddOrSet<T>(this Dictionary<T, int> dictionary, T key, int value)
            => dictionary[key] = dictionary.ContainsKey(key) ? dictionary[key] + value : value;

        /// <summary>
        /// If the key is not present in the dictionary, add the pair {key, value}.
        /// If the key already exists, update its value by adding the provided value to the existing one.
        /// </summary>
        public static void AddOrSet<T>(this Dictionary<T, long> dictionary, T key, long value)
            => dictionary[key] = dictionary.ContainsKey(key) ? dictionary[key] + value : value;

        /// <summary>
        /// If the key is not present, add it with a list containing the value.
        /// If the key exists, append the value to the list.
        /// </summary>
        public static void AddOrUpdateList<T1, T2>(this Dictionary<T1, List<T2>> dictionary, T1 key, T2 value)
        {
            if(dictionary.ContainsKey(key))
            {
                dictionary[key].Add(value);
            }
            else
            {
                dictionary[key] = new List<T2> {value};
            }
        }

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