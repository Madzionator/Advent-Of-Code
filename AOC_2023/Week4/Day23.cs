using Advent._2023.Day;

namespace AdventOfCode2023.Week4;

class Day23 : IDay
{
    private char[,] _map;

    public void Execute()
    {
        _map = File.ReadAllLines(@"Week4\input23.txt").ToCharMatrix();

        Console.WriteLine($"A: {Task(true)}");

        Console.WriteLine($"{Environment.NewLine}So far brute force, so task B will take up to 20 min for real input.");
        Console.WriteLine($"B: {Task(false)}");
    }

    record Path
    {
        public int Steps = 0;
        public List<(int Y, int X)> KeyPoints = new();
        public (int dy, int dx) LastDir = (1, 0);

        public Path Copy(int steps)
        {
            return new Path
            {
                Steps = steps,
                KeyPoints = new List<(int X, int Y)>(KeyPoints),
                LastDir = LastDir
            };
        }
    }

    int Task(bool isTaskA)
    {
        var finishedPaths = new List<Path>();

        var paths = new List<Path> { new() };
        paths[0].KeyPoints.Add((0, 1));

        var maxStep = _map.Count(x => x != '#');

        while (paths.Count > 0)
        {
            var newPaths = new List<Path>();
            foreach (var path in paths)
            {
                var point = path.KeyPoints.Last();
                var lastDir = path.LastDir;

                for (int step = 0; ; step++)
                {
                    var (avDir, isSlopeNear) = AvailableDirections(point.Y, point.X, lastDir, path.KeyPoints, isTaskA);

                    if (avDir.Count == 0)
                    {
                        if (point == (_map.GetLength(0) - 1, _map.GetLength(1) - 2))
                        {
                            if (!path.KeyPoints.Contains(point))
                                path.KeyPoints.Add(point);
                            path.Steps += step;
                            finishedPaths.Add(path);
                        }

                        break;
                    }

                    if (avDir.Count == 1)
                    {
                        if (isSlopeNear && !path.KeyPoints.Contains(point))
                            path.KeyPoints.Add(point);
                        point = (point.Y + avDir[0].dy, point.X + avDir[0].dx);
                        lastDir = avDir[0];
                        continue;
                    }

                    if (!path.KeyPoints.Contains(point))
                        path.KeyPoints.Add(point);

                    for (int i = 1; i < avDir.Count; i++)
                    {
                        var newPath = path.Copy(path.Steps + step + 1);
                        newPath.KeyPoints.Add((point.Y + avDir[i].dy, point.X + avDir[i].dx));
                        newPaths.Add(newPath);
                    }

                    point = (point.Y + avDir[0].dy, point.X + avDir[0].dx);
                    path.KeyPoints.Add(point);
                }
            }

            paths = newPaths;
            //Console.WriteLine($"newPath: {paths.Count}");
            //Console.WriteLine(finishedPaths.Count > 0 ? finishedPaths.Max(ppp => ppp.Steps) : 0);
            //Console.WriteLine();
        }

        return finishedPaths.Max(p => p.Steps);
    }

    (List<(int dy, int dx)>, bool) AvailableDirections(int y, int x, (int, int) lastDir, List<(int, int)> visited, bool isTaskA)
    {
        var dir = new List<(int, int)>();
        var slopeNear = false;

        if (lastDir != (1, 0) && y > 0 && _map[y - 1, x] is not '#' && !visited.Contains((y - 1, x)))
        {
            if (isTaskA)
            {
                if (_map[y - 1, x] is 'v')
                    slopeNear = true;
                else
                dir.Add((-1, 0));
            }
            else
                dir.Add((-1, 0));
        }

        if (lastDir != (0, 1) && x > 0 && _map[y, x - 1] is not '#' && !visited.Contains((y, x - 1)))
        {
            if (isTaskA)
            {
                if(_map[y, x - 1] is '>')
                    slopeNear = true;
                else
                    dir.Add((0, -1));
            }
            else
                dir.Add((0, -1));
        }

        if (lastDir != (-1, 0) && y < _map.GetLength(0) - 1 && _map[y + 1, x] is not '#' &&
            !visited.Contains((y + 1, x)))
        {
            dir.Add((1, 0));
        }

        if (lastDir != (0, -1) && x < _map.GetLength(1) - 1 && _map[y, x + 1] is not '#' && !visited.Contains((y, x + 1)))
        {
            dir.Add((0, 1));
        }

        return (dir, slopeNear);
    }
}