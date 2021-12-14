using System.Collections.Generic;

namespace Advent.Helpers.Extensions
{
    public static class DictionaryUtils
    {
        public static void AddOrSet<T>(this Dictionary<T, int> dictionary, T key, int value)
            => dictionary[key] = dictionary.ContainsKey(key) ? dictionary[key] + value : value;

        public static void AddOrSet<T>(this Dictionary<T, long> dictionary, T key, long value)
            => dictionary[key] = dictionary.ContainsKey(key) ? dictionary[key] + value : value;
    }
}