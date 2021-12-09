using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2021.Week2
{
    class Day9
    {
        public static int mapY;
        public static int mapX;

        public static void Execute()
        {
            var heightMap = File.ReadAllLines(@"Week2\input9.txt").ToArray();
            mapY = heightMap.Length;
            mapX = heightMap[0].Length;

            var (ansA, lowPoints) = TaskA(heightMap);

            Console.WriteLine(ansA);
            Console.WriteLine(TaskB(heightMap, lowPoints));
        }

        public static (int, List<(int, int)>) TaskA(string[] heightMap)
        {
            var risk = 0;
            var lowPoints = new List<(int, int)>();

            for(var y = 0; y<mapY; y++)
            for (var x = 0; x < mapX; x++)
            {
                if (y > 0 && heightMap[y][x] >= heightMap[y - 1][x])
                    continue;

                if (x > 0 && heightMap[y][x] >= heightMap[y][x - 1])
                    continue;

                if (x < mapX - 1 && heightMap[y][x] >= heightMap[y][x + 1])
                    continue;

                if (y < mapY - 1 && heightMap[y][x] >= heightMap[y + 1][x])
                    continue;

                risk += (heightMap[y][x] - '0') + 1;
                lowPoints.Add((y,x));
            }

            return (risk, lowPoints);
        }

        public static int TaskB(string[] heightMap, List<(int, int)> lowPoints)
        {
            var basinsMap = new Dictionary<(int, int), int>();
            var basinNumber = 0;
            var amountOfBasins = new List<int>();

            foreach (var (x, y) in lowPoints)
                ExploreBasinsMap(x, y, heightMap, basinsMap, basinNumber++);

            for (int l = 0; l < basinNumber; l++)
                amountOfBasins.Add(basinsMap.Values.Count(x => x == l));

            amountOfBasins = amountOfBasins.OrderByDescending(x => x).Take(3).ToList();

            return amountOfBasins[0] * amountOfBasins[1] * amountOfBasins[2];
        }

        private static void ExploreBasinsMap(int y, int x, string[] heightMap, Dictionary<(int, int), int> basinsMap, int basinNumber)
        {
            basinsMap[(y, x)] = basinNumber;

            if (y > 0 &&  heightMap[y - 1][x] != '9' && !basinsMap.ContainsKey((y - 1, x)))
                    ExploreBasinsMap(y - 1, x, heightMap, basinsMap, basinNumber);

            if (x > 0 && heightMap[y][x - 1] != '9' && !basinsMap.ContainsKey((y, x - 1)))
                    ExploreBasinsMap(y, x - 1, heightMap, basinsMap, basinNumber);

            if (x < mapX - 1 && heightMap[y][x + 1] != '9' && !basinsMap.ContainsKey((y, x + 1)))
                    ExploreBasinsMap(y, x + 1, heightMap, basinsMap, basinNumber);

            if (y < mapY - 1 && heightMap[y + 1][x] != '9' && !basinsMap.ContainsKey((y + 1, x)))
                    ExploreBasinsMap(y + 1, x, heightMap, basinsMap, basinNumber);
        }
    }
}
