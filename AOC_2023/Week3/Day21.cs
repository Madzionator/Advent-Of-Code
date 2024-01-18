using Advent._2023.Day;

namespace AdventOfCode2023.Week3;

class Day21 : IDay
{
    private char[,] _map;
    private int _max;

    record Point(int X, int Y); // f(x) = y

    public void Execute()
    {
        _map = File.ReadAllLines(@"Week3\input21.txt").ToCharMatrix();
        _max = _map.GetLength(1);

        Console.WriteLine($"A: {TaskA(64)}");

        //Part B : works on input where: stepsToDo = k * mapWidth + mapWidth/2, so correct for real input (131 x 131), incorrect for example (11 x 11) (with 2650136 steps); 
        Console.WriteLine($"B: {TaskB(26_501_365)}");
    }

    int TaskA(int stepsToDo) => CountGardenPlots(new[] { stepsToDo })[0].Y;

    long TaskB(long stepsToDo)
    {
        var keyPointCalculator = (int x) => x * _max + _max / 2; // x * 131 + 65
        var points = CountGardenPlots(new[] { keyPointCalculator(0), keyPointCalculator(1), keyPointCalculator(2) }); //calculate for 65, 196 and 327 steps

        return LagrangeInterpolation(points, stepsToDo);
    }

    Point[] CountGardenPlots(int[] stepsToDo)
    {
        var start = _map.First(x => x == 'S');
        var res = new Point[stepsToDo.Length];
        var p = 0;

        var allVisitedEven = new HashSet<(int Y, int X)>{ start };
        var allVisitedOdd = new HashSet<(int Y, int X)>();

        var lastEven = new HashSet<(int Y, int X)>{ start };
        var lastOdd = new HashSet<(int Y, int X)>();
        var temp = new HashSet<(int Y, int X)>();

        for (var step = 1; step <= stepsToDo.Last(); step++)
        {
            if(step % 2 != 0) // step 1, 3, 5 etc
            {
                foreach (var (y, x) in lastEven)
                {
                    TryAddNeighbours(y, x, allVisitedOdd);
                }

                lastOdd = temp;
            }
            else //step 2, 4, 6 etc.
            {
                foreach (var (y, x) in lastOdd)
                {
                    TryAddNeighbours(y, x, allVisitedEven);
                }

                lastEven = temp;
            }

            temp = new HashSet<(int Y, int X)>();
            if (stepsToDo.Contains(step))
            {
                res[p] = new Point(step, step % 2 == 0 ? allVisitedEven.Count : allVisitedOdd.Count);
                p++;
            }
        }

        return res;

        void TryAddNeighbours(int y, int x, HashSet<(int, int)> tryAddTo)
        {
            TryAddPosition(y - 1, x, tryAddTo);
            TryAddPosition(y + 1, x, tryAddTo);
            TryAddPosition(y, x - 1, tryAddTo);
            TryAddPosition(y, x + 1, tryAddTo);
        }

        void TryAddPosition(int y, int x, HashSet<(int, int)> tryAddTo)
        {
            if (!IsNotRock(y, x) || lastOdd.Contains((y, x)) || lastEven.Contains((y, x))) 
                return;

            tryAddTo.Add((y, x));
            temp.Add((y, x));
        }
    }

    bool IsNotRock(int y, int x)
    {
        var yn = y % _max;
        if (yn < 0) yn += _max;

        var xn = x % _max;
        if (xn < 0) xn += _max;

        return _map[yn, xn] != '#';
    }

    long LagrangeInterpolation(Point[] points, long x) //for exactly 3 points
    {
        double result = 0;

        for (var p1 = 0; p1 < 3; p1++)
        {
            double term = points[p1].Y;

            for (var p2 = 0; p2 < 3; p2++)
            {
                if (p2 != p1)
                    term = term * (x - points[p2].X) / (points[p1].X - points[p2].X);
            }

            result += term;
        }

        return (long)result;
    }
}