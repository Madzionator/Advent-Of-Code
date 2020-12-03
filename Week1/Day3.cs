using System;
using System.Collections.Generic;
using System.IO;

namespace Advent._2019.Week1
{
    public class Day3
    {
        public static void Execute()
        {
            List<int> report = new List<int>();
            StreamReader file = new System.IO.StreamReader(@"Week1\input3.txt");
            string[] map = file.ReadToEnd().Split("\r\n");
            int result_A = TreesCount(map, 1, 3);
            long result_B = Task_B(map);

            file.Close();
            Console.WriteLine(result_A);
            Console.WriteLine(result_B);
        }

        public static int TreesCount(string[] map, int stepY, int stepX)
        {
            int trees = 0;
            int x = 0;
            int y = 0;
            int maxx = map[0].Length;
            int maxy = map.Length;
            while (y < maxy)
            {
                if (map[y][x] == '#')
                    trees++;
                y += stepY;
                x += stepX;
                if (x >= maxx)
                    x = x - maxx;
            }

            return trees;
        }

        public static long Task_B(string[] map)
        {
            long result = (Convert.ToInt64(TreesCount(map, 1, 1)) * TreesCount(map, 1, 3) * TreesCount(map, 1, 5) * TreesCount(map, 1, 7) * TreesCount(map, 2, 1));
            return result;
        }
    }
}