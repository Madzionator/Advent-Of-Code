using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PointsList = System.Collections.Generic.List<(int x, int y, int z)>;

namespace Advent._2021.Week3
{
    class Day19
    {
        public static void Execute()
        {
            var file = File.ReadAllLines(@"Week3\input19.txt");
            var scannerList = new List<Scanner>();
            for (var i = 0; i < file.Length; i++)
            {
                if (file[i].Contains("-"))
                {
                    var newScanner = new Scanner();
                    i++;
                    while (i < file.Length && !string.IsNullOrWhiteSpace(file[i]))
                    {
                        var values = file[i].Split(',').Select(int.Parse).ToArray();
                        newScanner.Points.Add((values[0], values[1], values[2]));
                        i++;
                    }
                    newScanner.PointsCopy = newScanner.Points.ToList();
                    scannerList.Add(newScanner);
                }
            }

            var (ansA, ansB) = Task(scannerList);
            Console.WriteLine(ansA);
            Console.WriteLine(ansB);
        }

        public static (int, int) Task(List<Scanner> scanners)
        {
            var main = scanners[0];
            main.CalculateBeaconsDistance();
            var remaining = scanners.Skip(1).ToList();
            var scannersPosition = new PointsList {(0, 0, 0)};

            for (var i = 0; remaining.Count > 0; i++)
            {
                var scanner = remaining[i];
                for (var n = 0; n < 24; n++)
                {
                    scanner.RotateAllPoints(n);
                    var commonDifferences = main.BeaconsDist.Where(x => scanner.BeaconsDist.Contains(x)).ToList();
                    if (commonDifferences.Count > 3)
                    {
                        var difference = commonDifferences[0];
                        var p1 = main.FindWhereDifference(difference);
                        var p2 = scanner.FindWhereDifference(difference);

                        var p1_1 = main.Points[p1.i];
                        var p2_1 = scanner.Points[p2.i];
                        var diff = (p1_1.x - p2_1.x, p1_1.y - p2_1.y, p1_1.z - p2_1.z);
                        
                        scannersPosition.Add(diff);
                        scanner.AddToAllPoint(diff);

                        main.Points = main.Points.Concat(scanner.Points).Distinct().ToList();
                        main.CalculateBeaconsDistance();

                        i = -1;
                        remaining.Remove(scanner);
                        break;
                    }
                }
            }

            var maxManhattanDist = int.MinValue;
            foreach (var p1 in scannersPosition)
            foreach (var p2 in scannersPosition)
                if (p1 != p2)
                    maxManhattanDist = Math.Max(maxManhattanDist, Math.Abs(p1.x - p2.x) + Math.Abs(p1.y - p2.y) + Math.Abs(p1.z - p2.z));

            return (main.Points.Count, maxManhattanDist);
        }
    }

    class Scanner
    {
        public PointsList Points = new();
        public PointsList PointsCopy = new();
        public HashSet<(int x, int y, int z)> BeaconsDist = new();

        public void CalculateBeaconsDistance()
        {
            BeaconsDist = new();
            for (var i = 0; i < Points.Count; i++)
                for (var j = 0; j < i; j++)
                    BeaconsDist.Add((Points[i].x - Points[j].x, Points[i].y - Points[j].y, Points[i].z - Points[j].z));
        }

        public void AddToAllPoint((int x, int y, int z) difference)
        {
            for (int i = 0; i < Points.Count; i++)
                Points[i] = (Points[i].x + difference.x, Points[i].y + difference.y, Points[i].z + difference.z);
        }

        public void RotateAllPoints(int n)
        {
            for (var i = 0; i < Points.Count; i++)
                Points[i] = GetRotatedPoint(PointsCopy[i], n);
            CalculateBeaconsDistance();
        }

        public (int i, int j) FindWhereDifference((int x, int y, int z) difference)
        {
            for (var i = 0; i < Points.Count; i++)
                for (var j = 0; j < i; j++)
                    if (difference == (Points[i].x - Points[j].x, Points[i].y - Points[j].y, Points[i].z - Points[j].z))
                        return (i, j);
            return (-1, -1);
        }

        public static (int, int, int) GetRotatedPoint((int, int, int) p, int n)
        {
            (int x, int y, int z) = p;
            return n switch
            {
                0 => (x, y, z),
                1 => (x, -y, -z),
                2 => (-x, y, -z),
                3 => (-x, -y, z),

                4 => (x, z, -y),
                5 => (x, -z, y),
                6 => (-x, z, y),
                7 => (-x, -z, -y),

                8 => (y, z, x),
                9 => (y, -z, -x),
                10 => (-y, z, -x),
                11 => (-y, -z, x),

                12 => (y, x, -z),
                13 => (y, -x, z),
                14 => (-y, x, z),
                15 => (-y, -x, -z),

                16 => (z, x, y),
                17 => (z, -x, -y),
                18 => (-z, x, -y),
                19 => (-z, -x, y),

                20 => (z, y, -x),
                21 => (z, -y, x),
                22 => (-z, y, x),
                23 => (-z, -y, -x),
            };
        }
    }
}