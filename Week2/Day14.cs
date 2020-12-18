using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2020.Week2
{
    public class Day14
    {
        public static void Execute()
        {
            var data = File.ReadAllLines(@"Week2\input14.txt");

            long resultA = TaskA(data);
            long resultB = TaskB(data);

            Console.WriteLine(resultA);
            Console.WriteLine(resultB);
        }

        private static long TaskA(string[] data)
        {
            var dict = new Dictionary<int, long>();
            string mask = "";
            foreach (var line in data)
            {
                if (line[2] == 's') //mask
                {
                    mask = line.Substring(7);
                }
                else //mem
                {
                    var a = line.Split("] = ");
                    int key = int.Parse(a[0].Substring(4));
                    int value10 = int.Parse(a[1]);
                    long newValue10 = getNewValueA(mask, value10);
                    if (dict.ContainsKey(key))
                        dict[key] = newValue10;
                    else
                        dict.Add(key, newValue10);
                }
            }
            long result = dict.Values.Sum();
            return result;
        }

        private static long getNewValueA(string mask, int value10)
        {
            var binary = Convert.ToString(value10, 2);
            binary = binary.PadLeft(36, '0');

            var value = new char[36];

            for (int i = 0; i < 36; i++)
            {
                if (mask[i] != 'X')
                    value[i] = mask[i];
                else
                    value[i] = binary[i];
            }
            string result = new string(value);
            return Convert.ToInt64(result, 2);
        }

        private static long TaskB(string[] data)
        {
            var dict = new Dictionary<long, long>();
            string mask = "";
            foreach (var line in data)
            {
                if (line[2] == 's') //mask
                {
                    mask = line.Substring(7);
                }
                else //mem
                {
                    var a = line.Split("] = ");
                    int key = int.Parse(a[0].Substring(4));
                    int value = int.Parse(a[1]);
                    long[] newKey10 = getNewValueB(mask, key);
                    foreach (var nKey in newKey10)
                        if (dict.ContainsKey(nKey))
                            dict[nKey] = value;
                        else
                            dict.Add(nKey, value);
                }
            }
            long result = dict.Values.Sum();
            return result;
        }

        private static long[] getNewValueB(string mask, int key10)
        {
            var binary = Convert.ToString(key10, 2);
            binary = binary.PadLeft(36, '0');

            var key2 = new char[36];

            for (int i = 0; i < 36; i++)
            {
                if (mask[i] == 'X' || mask[i] == '1')
                    key2[i] = mask[i];
                else
                    key2[i] = binary[i];
            }

            var newKeys = new List<char[]>();
            newKeys.Add(key2);
            newKeys = modifyKeys(newKeys);

            var result = new List<long>();
            foreach (var key in newKeys)
            {
                string partResultStr = new string(key);
                result.Add(Convert.ToInt64(partResultStr, 2));
            }
            return result.ToArray();
        }

        private static List<char[]> modifyKeys(List<char[]> keys)
        {
            for (int i = 35; i >= 0; i--)
            {
                if (keys[0][i] != 'X')
                    continue;
                var newKeys = new List<char[]>();
                foreach (var key in keys)
                {
                    var temp1 = key.ToArray();
                    temp1[i] = '0';
                    newKeys.Add(temp1);
                    var temp2 = key.ToArray();
                    temp2[i] = '1';
                    newKeys.Add(temp2);
                }
                keys.Clear();
                keys = newKeys;
            }
            return keys;
        }
    }
}