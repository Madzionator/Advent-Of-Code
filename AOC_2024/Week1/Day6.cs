using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Week1;

class Day6 : Day
{
    private HashSet<Vector2> _obstacles = [];
    private Vector2 _start;

    public override (object resultA, object resultB) Execute()
    {
        _start = InputLines
            .SelectMany((line, y) => line.Select((c, x) => (c, x, y)))
            .Where(v => v.c == '^')
            .Select(v => (Vector2)(v.y, v.x))
            .First();

        _obstacles = InputLines
            .SelectMany((line, y) => line.Select((c, x) => (c, x, y)))
            .Where(v => v.c == '#')
            .Select(v => (Vector2)(v.y, v.x))
            .ToHashSet();

        var visited = TaskA();

        return (visited.Count, TaskB(visited));
    }

    HashSet<Vector2> TaskA()
    {
        var dir = new Direction2(Direction.Up);
        var position = _start;

        HashSet<Vector2> visited = [];
        do
        {
            visited.Add(position);
            var next = position.Move(dir);
            if (IsValidPosition(next) && _obstacles.Contains(next))
            {
                dir = dir.Rotate(Rotation.Right90);
                next = position.Move(dir);
            }

            position = next;
        } while (IsValidPosition(position));

        return visited;
    }

    int TaskB(HashSet<Vector2> possibleObstacles)
    {
        var result = 0;

        Parallel.ForEach(possibleObstacles, obstacle =>
        {
            var dir = new Direction2(Direction.Up);
            var position = _start;

            HashSet<(Vector2 pos, Direction2 dir)> visited = new();

            do
            {
                if (!visited.Add((position, dir)))
                {
                    Interlocked.Increment(ref result);
                    break;
                }

                var next = position.Move(dir);
                if (IsValidPosition(next) && (_obstacles.Contains(next) || next == obstacle))
                {
                    dir = dir.Rotate(Rotation.Right90);
                }
                else
                {
                    position = next;
                }

            } while (IsValidPosition(position));
        });

        return result;
    }

    bool IsValidPosition(Vector2 p) => p.X >= 0 && p.Y >= 0 && p.X < InputLines[0].Length && p.Y < InputLines.Length;
}