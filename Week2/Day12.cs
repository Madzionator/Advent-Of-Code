using System;
using System.IO;
using System.Linq;

namespace Advent._2020.Week2
{
    public class Day12
    {
        public static void Execute()
        {
            var coords = File.ReadAllLines(@"Week2\input12.txt")
                .Select(lines => (lines[0], int.Parse(lines.Substring(1))))
                .ToArray();

            int resultA = TaskA(coords);
            int resultB = TaskB(coords);

            Console.WriteLine(resultA);
            Console.WriteLine(resultB);
        }

        private static int TaskA((char dir, int val)[] coordsData)
        {
            (int dx, int dy)[] directions = { (1, 0), (0, -1), (-1, 0), (0, 1) };
            var (currentX, currentY) = (0, 0);
            var currentDirection = 0;
            foreach (var c in coordsData)
            {
                switch (c.dir)
                {
                    case 'N':
                        currentY += c.val;
                        break;
                    case 'E':
                        currentX += c.val;
                        break;
                    case 'S':
                        currentY -= c.val;
                        break;
                    case 'W':
                        currentX -= c.val;
                        break;
                    case 'R':
                        int a = c.val / 90;
                        a = (currentDirection + a) % 4;
                        currentDirection = a;
                        break;
                    case 'L':
                        a = c.val / 90;
                        a = (currentDirection - a + 4) % 4;
                        currentDirection = a;
                        break;
                    case 'F':
                        currentX += directions[currentDirection].dx * c.val;
                        currentY += directions[currentDirection].dy * c.val;
                        break;
                }
            }

            return Math.Abs(currentX) + Math.Abs(currentY);
        }

        private static int TaskB((char dir, int val)[] coordsData)
        {
            var (ShipX, ShipY) = (0, 0);
            var (PointDx, PointDy) = (10, 1);

            foreach (var c in coordsData)
            {
                switch (c.dir)
                {
                    case 'N':
                        PointDy += c.val;
                        break;
                    case 'E':
                        PointDx += c.val;
                        break;
                    case 'S':
                        PointDy -= c.val;
                        break;
                    case 'W':
                        PointDx -= c.val;
                        break;
                    case 'R':
                        int a = c.val / 90;
                        for (int k = 1; k <= a; k++)
                        {
                            (PointDx, PointDy) = (PointDy, PointDx);
                            PointDy *= -1;
                        }
                        break;
                    case 'L':
                        a = c.val / 90;
                        for (int k = 1; k <= a; k++)
                        {
                            (PointDx, PointDy) = (PointDy, PointDx);
                            PointDx *= -1;
                        }
                        break;
                    case 'F':
                        int moveX = PointDx * c.val;
                        int moveY = PointDy * c.val;
                        ShipX += moveX;
                        ShipY += moveY;
                        break;
                }
            }

            return Math.Abs(ShipX) + Math.Abs(ShipY);
        }
    }
}