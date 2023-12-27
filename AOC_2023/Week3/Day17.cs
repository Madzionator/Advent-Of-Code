using Advent._2023.Day;

namespace AdventOfCode2023.Week3;

class Day17 : IDay
{
    int[,] _costs;
    int _maxX;
    int _maxY;

    record Point(int Y, int X, int dY, int dX, int Length, bool CanFinish);

    public void Execute()
    {
        _costs = File.ReadAllLines(@"Week3\input17.txt").ToMatrix();
        _maxY = _costs.GetLength(0)-1;
        _maxX = _costs.GetLength(1)-1;

        Console.WriteLine($"A: {Task(true)}");
        Console.WriteLine($"B: {Task(false)}");
    }

    int Task(bool isTaskA)
    {
        var queue = new PriorityQueue<Point, int>();
        var visited = new Dictionary<Point, int>(); //point, cost

        var start = new Point(0, 0, 0, 1, 0, false);
        queue.Enqueue(start, 0);
        visited.Add(start, 0);

        while (queue.Count > 0)
        {
            var p = queue.Dequeue();
            if (p.Y == _maxY && p.X == _maxX && p.CanFinish)
            {
                return visited[p];
            }

            var newPoints = GetValidPoints(p, isTaskA);
            foreach (var point in newPoints)
            {
                var actualCost = visited[p] + _costs[point.Y, point.X];
                if (visited.ContainsKey(point))
                {
                    if (actualCost < visited[point])
                    {
                        visited[point] = actualCost;
                        queue.Enqueue(point, actualCost);
                    }
                }
                else
                {
                    visited.Add(point, actualCost);
                    queue.Enqueue(point, actualCost);
                }
            }
        }

        throw new Exception();
    }

    IEnumerable<Point> WhereToGoA(Point p)
    {
        if (p.Length < 3)
            yield return new Point(p.Y + p.dY, p.X + p.dX, p.dY, p.dX, p.Length + 1, true);

        yield return new Point(p.Y + p.dX, p.X + p.dY, p.dX, p.dY, 1, true);
        yield return new Point(p.Y - p.dX, p.X - p.dY, -p.dX, -p.dY, 1, true);
    }

    IEnumerable<Point> WhereToGoB(Point p)
    {
        if (p.Length >= 4)
        {
            yield return new Point(p.Y + p.dX, p.X + p.dY, p.dX, p.dY, 1, false);
            yield return new Point(p.Y - p.dX, p.X - p.dY, -p.dX, -p.dY, 1, false);
        }

        if (p.Length < 10)
        {
            yield return new Point(p.Y + p.dY, p.X + p.dX, p.dY, p.dX, p.Length + 1, p.Length + 1 >= 4);
        }
    }

    IEnumerable<Point> GetValidPoints(Point basePoint, bool isTaskA)
    {
        var points = isTaskA ? WhereToGoA(basePoint) : WhereToGoB(basePoint);
        foreach (var p in points)
        {
            if(p.Y >= 0 && p.X >= 0 && p.Y <= _maxY && p.X <= _maxX)
                yield return p;
        }
    }
}