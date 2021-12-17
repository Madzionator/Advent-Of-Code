using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Advent._2021.Week3
{
    class Day17
    {
        private static (int min, int max) X;
        private static (int min, int max) Y;

        public static void Execute()
        {
            var input = File.ReadAllText(@"Week3\input17.txt");
            Regex regex = new Regex(@"target area: x=(.*)\.\.(.*), y=(.*)\.\.(.*)");
            var values = regex.Match(input).Groups;

            X = (int.Parse(values[1].Value), int.Parse(values[2].Value));
            Y = (int.Parse(values[3].Value), int.Parse(values[4].Value));

            var (answA, answB) = Task();
            Console.WriteLine(answA);
            Console.WriteLine(answB);
        }

        public static (int, int) Task()
        {
            int highest = -1000;
            int counter = 0;

            for (int x = 0; x <= X.max; x++)
                for (int y = 1000; y >= -1000; y--)
                {
                    var (isReached, hight) = IsReached(x, y);
                    if (isReached && (hight > highest))
                        highest = hight;

                    if (isReached)
                        counter++;
                }
                             
                return (highest, counter);
        }

        public static (bool, int) IsReached(int vx, int vy)
        {
            int localH = -10000;
            var (px, py) = (0, 0);

            while (py >= Y.min && px <= X.max)
            {
                px += vx;
                py += vy;

                if (py > localH)
                    localH = py;

                if (IsInArea(px, py))
                    return (true, localH);

                if (vx == 0 && !(X.min <= px && px <= X.max))
                    break;

                vx = vx > 0 ? --vx : vx;
                vy--;
            }

            return (false, -1000);

            bool IsInArea(int x, int y)
            {
                if (X.min > x || x > X.max)
                    return false;

                if (Y.min > y || y > Y.max)
                    return false;

                return true;
            }
        }
    }
}
