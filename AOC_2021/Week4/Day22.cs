using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2021.Week4
{
    class Day22
    {
        public static void Execute()
        {
            var rebootSteps = new List<Cube>();
            var file = File.ReadAllLines(@"Week4\input22.txt");
            foreach (var line in file)
            {
                var lineVal = line.Replace(",y", "").Replace(",z", "").Replace("..", "=").Split("=");
                var values = lineVal[1..].Select(int.Parse).ToArray();
                rebootSteps.Add(new Cube(values[0], values[1], values[2], values[3], values[4], values[5], lineVal[0][1] == 'n'));
            }

            Console.WriteLine(TaskA(rebootSteps));
            Console.WriteLine(TaskB(rebootSteps));
        }

        public static int TaskA(List<Cube> steps)
        {
            var cuboit = new Dictionary<(int x, int y, int z), bool>();
            foreach (var s in steps)
            {
                if (s.minX > 50 || s.minY > 50 || s.minZ > 50 || s.maxX < -50 || s.maxY < -50 || s.maxZ < -50)
                    continue;

                Turn(s.on);

                void Turn(bool on)
                {
                    for (var ix = s.minX; ix <= s.maxX; ix++)
                    for (var iy = s.minY; iy <= s.maxY; iy++)
                    for (var iz = s.minZ; iz <= s.maxZ; iz++)
                        if (Math.Abs(ix) <= 50 && Math.Abs(iy) <= 50 && Math.Abs(iz) <= 50) 
                            cuboit[(ix, iy, iz)] = on;
                }
            }

            return cuboit.Count(x => x.Value == true);
        }

        public static long TaskB(List<Cube> cuboits)
        {
            List<Cube> cubes = new();
            foreach (var cuboit in cuboits)
            {
                cubes.AddRange(
                    cubes.Where(cuboit.IsIntersect)
                        .ToList()
                        .Select(inter => cuboit.Intersection(inter, !inter.on)));

                if (cuboit.on)
                    cubes.Add(cuboit);
            }

            return cubes.Sum(x => x.V * (x.on ? 1L : -1L));
        }

        public record Cube(int minX, int maxX, int minY, int maxY, int minZ, int maxZ, bool on)
        {
            public bool IsIntersect(Cube cube) =>
                !(maxX < cube.minX || minX > cube.maxX
                                   || maxY < cube.minY || minY > cube.maxY
                                   || maxZ < cube.minZ || minZ > cube.maxZ);
            public Cube Intersection(Cube cube, bool on)
            {
                var minx = Math.Max(minX, cube.minX);
                var miny = Math.Max(minY, cube.minY);
                var minz = Math.Max(minZ, cube.minZ);
                var maxx = Math.Min(maxX, cube.maxX);
                var maxy = Math.Min(maxY, cube.maxY);
                var maxz = Math.Min(maxZ, cube.maxZ);
                return new Cube(minx, maxx, miny, maxy, minz, maxz, on);
            }

            public long V => (maxX - minX + 1L) * (maxY - minY + 1L) * (maxZ - minZ + 1L);
        }
    }
}
