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

        //Part B ~ 2s + works on input where: stepsToDo = k * mapWidth + mapWidth/2, so correct for real input (131 x 131), incorrect for example (11 x 11); 
        Console.WriteLine($"B: {TaskB(26_501_365)}");
    }

    int TaskA(int step) => CountGardenPlots(new[] { step })[0].Y;

    long TaskB(long stepsToDo)
    {
        var keyPoint = (int x) => x * _max + _max / 2; // x * 131 + 65
        var points = CountGardenPlots(new[] { keyPoint(0), keyPoint(1), keyPoint(2) }); // 65, 196, 327

        return LagrangeInterpolation(points, stepsToDo);
    }

    Point[] CountGardenPlots(int[] stepsToDo)
    {
        var start = _map.First(x => x == 'S');
        var res = new Point[stepsToDo.Length];
        int p = 0;

        var mapToModify = new HashSet<(int Y, int X)>();
        var mapToCompare = new HashSet<(int Y, int X)>{ start };

        for (var step = 1; step <= stepsToDo.Last(); step++)
        {
            foreach ((int y, int x) in mapToCompare)
            {
                mapToModify.Remove((y, x));

                if (IsNotRock(y - 1, x))
                    mapToModify.Add((y - 1, x));

                if (IsNotRock(y + 1, x))
                    mapToModify.Add((y + 1, x));

                if (IsNotRock(y, x - 1))
                    mapToModify.Add((y, x - 1));

                if (IsNotRock(y, x + 1))
                    mapToModify.Add((y, x + 1));
            }

            mapToCompare = new HashSet<(int Y, int X)>(mapToModify);
            if (stepsToDo.Contains(step))
            {
                res[p] = new Point(step, mapToCompare.Count);
                p++;
            }
        }

        return res;
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