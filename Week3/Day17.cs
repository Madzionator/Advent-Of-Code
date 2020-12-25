using System;
using System.Collections.Generic;
using System.IO;

namespace Advent._2020.Week3
{
    public class Day17
    {
        public static void Execute()
        {
            var grade = new Dictionary<(int, int, int, int), bool>(transformData(File.ReadAllLines(@"Week3\input17.txt")));
            var gradeCopy = new Dictionary<(int, int, int, int), bool>(grade);

            int resultA = Task(grade, 'A');
            int resultB = Task(gradeCopy, 'B');

            Console.WriteLine(resultA);
            Console.WriteLine(resultB);
        }

        private static int Task(Dictionary<(int, int, int, int), bool> grade, char task)
        {
            var neighbors = new (int, int, int, int)[0];
            if (task == 'A')
                neighbors = MakeNeighborsPosition3();
            else
                neighbors = MakeNeighborsPosition4();

            for (int cycle = 1; cycle <= 6; cycle++)
            {
                var toCheck = new Dictionary<(int, int, int, int), bool>(grade);
                foreach (var cube in grade.Keys)
                    foreach (var neighbor in neighbors)
                        TryAddToCheck(cube, neighbor, toCheck);

                foreach (var cube in toCheck)
                {
                    int activeNeighbor = CountActiveNeighbor(neighbors, toCheck, cube.Key);
                    if (cube.Value && (activeNeighbor < 2 || activeNeighbor > 3))
                        grade.Remove(cube.Key);
                    else if (!cube.Value && activeNeighbor == 3)
                        grade.Add(cube.Key, true);
                }
                toCheck.Clear();
            }

            return grade.Count;
        }

        private static void TryAddToCheck((int x, int y, int z, int w) cube, (int x, int y, int z, int w) neighbor, Dictionary<(int, int, int, int), bool> toCheck)
        {
            (int, int, int, int) newcube = (cube.x + neighbor.x, cube.y + neighbor.y, cube.z + neighbor.z, cube.w + neighbor.w);
            if (!toCheck.ContainsKey(newcube))
                toCheck.Add(newcube, false);
        }

        private static int CountActiveNeighbor((int x, int y, int z, int w)[] neighbors, Dictionary<(int x, int y, int z, int w), bool> grade, (int x, int y, int z, int w) cube)
        {
            int activeNeighbors = 0;
            foreach (var neighbor in neighbors)
            {
                (int, int, int, int) newcube = (cube.x + neighbor.x, cube.y + neighbor.y, cube.z + neighbor.z, cube.w + neighbor.w);
                if (grade.ContainsKey(newcube) && grade[newcube])
                    activeNeighbors++;
            }
            return activeNeighbors;
        }

        private static Dictionary<(int, int, int, int), bool> transformData(string[] data)
        {
            var grade = new Dictionary<(int, int, int, int), bool>();
            for (int y = 0; y < data.Length; y++)
                for (int x = 0; x < data[0].Length; x++)
                    if (data[y][x] == '#')
                        grade.Add((x, y, 0, 0), true);
            return grade;
        }

        private static (int, int, int, int)[] MakeNeighborsPosition3()
        {
            var neighbors = new List<(int, int, int, int)>();
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                    for (int z = -1; z <= 1; z++)
                        neighbors.Add((x, y, z, 0));
            neighbors.Remove((0, 0, 0, 0));
            return neighbors.ToArray();
        }

        private static (int, int, int, int)[] MakeNeighborsPosition4()
        {
            var neighbors = new List<(int, int, int, int)>();
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                    for (int z = -1; z <= 1; z++)
                        for (int w = -1; w <= 1; w++)
                            neighbors.Add((x, y, z, w));
            neighbors.Remove((0, 0, 0, 0));
            return neighbors.ToArray();
        }
    }
}