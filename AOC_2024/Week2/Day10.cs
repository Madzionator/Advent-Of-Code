using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Week2;

internal class Day10 : Day
{
    private int[,] _map;
    private Vector2[] _9positions;
    private Direction2[] _directions;

    public override (object resultA, object resultB) Execute()
    {
        _map = InputLines.ToMatrix();
        _9positions = InputLines.SelectMany((line, y) => line
                .Select((i, x) => new { y, x, c = i })
                .Where(v => v.c == '9')
                .Select(v => new Vector2(v.y, v.x)))
            .ToArray();

        _directions = Direction2.Sides;

        return Task();
    }

    (int, int) Task()
    {
        var resultA = 0;
        var resultB = 0;

        foreach (var pos9 in _9positions)
        {
            var zeros = new List<Vector2>();
            FloodFill(zeros, pos9);

            resultA += zeros.ToHashSet().Count;
            resultB += zeros.Count;
        }

        return (resultA, resultB);
    }

    void FloodFill(List<Vector2> zeros, Vector2 position)
    {
        if (_map[position.Y, position.X] == 0)
        {
            zeros.Add(position);
            return;
        }

        foreach (var dir in _directions)
        {
            var newP = ValidPosition(position, dir);
            if (newP is not null)
            {
                FloodFill(zeros, (Vector2)newP);
            }
        }
    }

    Vector2? ValidPosition(Vector2 pos, Direction2 dir)
    {
        var newPos = pos.Move(dir);
        if (newPos.Y < 0 || newPos.X < 0 || newPos.Y > _map.GetLength(0) - 1 || newPos.X > _map.GetLength(1) - 1)
            return null;

        if (_map[pos.Y, pos.X] - 1 != _map[newPos.Y, newPos.X])
            return null;

        return newPos;
    }
}