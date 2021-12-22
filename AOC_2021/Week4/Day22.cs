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
            var points = new List<Cube>();
            var file = File.ReadAllLines(@"Week4\input22.txt");
            foreach (var line in file)
            {
                var val = line.Replace(",y", "").Replace(",z", "").Replace("..", "=").Split("=");
                var vals = val[1..].Select(int.Parse).ToArray();
                points.Add(new Cube(vals[0], vals[1], vals[2], vals[3], vals[4], vals[5], val[0][1] == 'n'));
            }

            Console.WriteLine(TaskA(points));
            Console.WriteLine(TaskB(points));
        }

        public static int TaskA(List<Cube> points)
        {
            var dict = new Dictionary<(int x, int y, int z), bool>();
            foreach (var p in points)
            {
                if (p.minX > 50 || p.minY > 50 || p.minZ > 50)
                    continue;
                if (p.maxX < -50 || p.maxY < -50 || p.maxZ < -50)
                    continue;

                if (p.on)
                    Turn(true);
                else
                    Turn(false);

                void Turn(bool on)
                {
                    for (int ix = p.minX; ix <= p.maxX; ix++)
                        for (int iy = p.minY; iy <= p.maxY; iy++)
                            for (int iz = p.minZ; iz <= p.maxZ; iz++)
                            {
                                if (Math.Abs(ix) > 50 || Math.Abs(iy) > 50 || Math.Abs(iz) > 50)
                                    continue;

                                dict[(ix, iy, iz)] = on;
                            }
                }
            }

            return dict.Count(x => x.Value == true);
        }

        public static long TaskB(List<Cube> points)
        {
            List<Cube> cubes = new();
            foreach (var point in points)
            {
                cubes.AddRange(
                    cubes.Where(point.IsIntersect)
                        .ToList()
                        .Select(inter => point.Intersection(inter, !inter.on)));

                if (point.on) cubes.Add(point);
            }

            return cubes.Sum(x=> x.V * (x.on ? 1L : -1L));
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
