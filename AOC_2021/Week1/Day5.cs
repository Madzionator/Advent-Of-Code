using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2021.Week1
{
    class Day5
    {
        public static void Execute()
        {
            var ventLines = File.ReadAllLines(@"Week1\input5.txt")
                .Select(x => 
                    x.Replace(" -> ", ",")
                        .Split(",")
                        .Select(int.Parse)
                        .ToArray())
                .ToList();

            var points = new Dictionary<(int, int), int>();

            Console.WriteLine(TaskA(ventLines, points));
            Console.WriteLine(TaskB(ventLines, points));
        }

        public static int TaskA(List<int[]> ventLines, Dictionary<(int, int), int> points)
        {
            foreach (var line in ventLines)
            {
                if (line[1] == line[3] || line[0] == line[2])    // 0-xB, 1-yB, 2-xE, 3-yE 
                {
                    int dirX = line[0] == line[2] ? 0 : 1;
                    int dirY = line[1] == line[3] ? 0 : 1;

                    if (line[0] > line[2])
                        (line[0], line[2]) = (line[2], line[0]);
                    if (line[1] > line[3])
                        (line[1], line[3]) = (line[3], line[1]);

                    for (int x = line[0], y = line[1]; x <= line[2] && y <= line[3]; x+=dirX, y+=dirY)
                    {
                        if (!points.ContainsKey((x, y)))
                            points[(x, y)] = 0;
                        points[(x, y)]++;
                    }
                }
            }

            return points.Where(x => x.Value >= 2).Count();
        }

        public static int TaskB(List<int[]> ventLines, Dictionary<(int, int), int> points)
        {
            foreach (var line in ventLines)
            {
                if (line[1] == line[3] || line[0] == line[2])
                    continue;

                int dirX = line[0] > line[2] ? -1 : 1;
                int dirY = line[1] > line[3] ? -1 : 1;

                for (int x = line[0], y = line[1]; line[2] - x != 0 && line[3] - y != 0; x += dirX, y += dirY)
                {
                    if (!points.ContainsKey((x, y)))
                        points[(x, y)] = 0;
                    points[(x, y)]++;
                }

                if (!points.ContainsKey((line[2], line[3])))
                    points[(line[2], line[3])] = 0;
                points[(line[2], line[3])]++;
            }

            return points.Where(x => x.Value >= 2).Count();
        }
    }
}