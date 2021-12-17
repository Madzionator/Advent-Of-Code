using System;
using System.IO;
using System.Linq;
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
            var regex = new Regex(@"target area: x=(.*)\.\.(.*), y=(.*)\.\.(.*)");
            var intputVal = regex.Match(input).Groups.Cast<Group>().Skip(1).Select(x => int.Parse(x.Value)).ToArray();

            X = (intputVal[0], intputVal[1]);
            Y = (intputVal[2], intputVal[3]);

            var (answA, answB) = Task();
            Console.WriteLine(answA);
            Console.WriteLine(answB);
        }

        public static (int, int) Task()
        {
            var highest = Y.min;
            var counter = 0;

            for (var vx = 0; vx <= X.max; vx++)
                for (var vy = 1500; vy >= -1500; vy--)
                {
                    var (isReached, height) = IsReached(vx, vy);

                    if (isReached && height > highest)
                        highest = height;

                    if (isReached)
                        counter++;
                }
            
            return (highest, counter);
        }

        public static (bool, int) IsReached(int vx, int vy)
        {
            bool IsInArea(int x, int y) => X.min <= x && x <= X.max && Y.min <= y && y <= Y.max;

            var localMaxH = Y.min;
            var (px, py) = (0, 0);

            while (py >= Y.min && px <= X.max)
            {
                px += vx;
                py += vy;

                if (py > localMaxH)
                    localMaxH = py;

                if (IsInArea(px, py))
                    return (true, localMaxH);

                if (vx == 0 && !(X.min <= px && px <= X.max))
                    break;

                vx = vx > 0 ? --vx : vx;
                vy--;
            }

            return (false, Y.min);
        }
    }
}
