using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Week3;

internal class Day16 : Day
{
    private char[,] _maze;
    private PriorityQueue<(Vector2 pos, Direction2 dir, List<Vector2> path), int> _pathQueue = new();
    private Dictionary<(Vector2 pos, Direction2 dir), int> _costs = new();

    public override (object resultA, object resultB) Execute()
    {
        _maze = InputLines.ToCharMatrix();
        return Task();
    }

    (int, int) Task()
    {
        var minFinishCost = int.MaxValue;

        var startPosition = new Vector2(_maze.GetLength(0) - 2, 1);
        var start = (startPosition, Direction.Right, new List<Vector2>{ startPosition });
        
        _pathQueue.Enqueue(start, 0);
        _costs.Add((start.Item1, start.Item2), 0);

        var bestPaths = new List<Vector2>();

        while (_pathQueue.TryDequeue(out var step, out var cost))
        {
            if (_maze[step.pos.Y, step.pos.X] == 'E')
            {
                if (cost < minFinishCost)
                {
                    minFinishCost = cost;
                    bestPaths.Clear();
                }

                if (cost == minFinishCost)
                {
                    bestPaths.AddRange(step.path);
                }

                continue;
            }

            TryExplore(step.pos, step.path, step.dir, cost + 1);
            TryExplore(step.pos, step.path, step.dir.Rotate(Rotation.Left90), cost + 1001);
            TryExplore(step.pos, step.path, step.dir.Rotate(Rotation.Right90), cost + 1001);
        }

        return (minFinishCost, bestPaths.Distinct().Count());
    }

    void TryExplore(Vector2 pos, List<Vector2> path, Direction2 newDir, int newCost)
    {
        var newPosition = pos.Move(newDir);

        if (_maze[newPosition.Y, newPosition.X] == '#')
            return;

        if (!_costs.ContainsKey((newPosition, newDir)) || _costs[(newPosition, newDir)] >= newCost)
        {
            _costs.TryAdd((newPosition, newDir), newCost);
            _pathQueue.Enqueue((newPosition, newDir, [.. path, newPosition]), newCost);
        }
    }
}