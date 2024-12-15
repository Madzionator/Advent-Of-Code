using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Week3;

internal class Day15 : Day
{
    private HashSet<Vector2> _boxesA = [];
    private HashSet<Vector2> _wallsA = [];

    private Dictionary<Vector2, Vector2> _boxesB = [];
    private HashSet<Vector2> _wallsB = [];

    private Direction[] _moves;

    public override (object resultA, object resultB) Execute()
    {
        var start = ProcessInput();
        return (TaskA(start), TaskB((start.Y, start.X*2)));
    }

    long TaskA(Vector2 start)
    {
        var robotP = start;

        foreach (var dir in _moves)
        {
            var nextFree = robotP.Move(dir);

            while (true)
            {
                if (_wallsA.Contains(nextFree))
                {
                    break;
                }

                if (_boxesA.Contains(nextFree))
                {
                    nextFree = nextFree.Move(dir);
                    continue;
                }

                var nextP = robotP.Move(dir);
                if (nextP != nextFree)
                {
                    _boxesA.Add(nextFree);
                    _boxesA.Remove(nextP);
                }

                robotP = nextP;
                break;
            }
        }

        return _boxesA.Sum(v => v.Y * 100 + v.X);
    }

    long TaskB(Vector2 start)
    {
        var robotP = start;

        foreach (var dir in _moves)
        {
            var affected = new HashSet<Vector2>();

            if(CanMove(robotP.Move(dir), dir, affected))
            {
                var temp = new List<(Vector2, Vector2)>();
                foreach (var affectedP in affected)
                {
                    temp.Add((affectedP, _boxesB[affectedP]));
                    _boxesB.Remove(affectedP);
                }

                foreach (var t in temp)
                {
                    _boxesB.Add(t.Item1.Move(dir), t.Item2.Move(dir));
                }

                robotP = robotP.Move(dir); ;
            };
        }

        return _boxesB.Chunk(2)
            .Select(pair => pair[0].Key.X < pair[1].Key.X ? pair[0].Key : pair[1].Key)
            .Sum(v => v.Y * 100 + v.X);
    }

    bool CanMove(Vector2 p, Direction dir, HashSet<Vector2> affected)
    {
        if (!_boxesB.ContainsKey(p) && !_wallsB.Contains(p))
        {
            return true;
        }

        if (_wallsB.Contains(p))
        {
            return false;
        }

        var otherHalf = _boxesB[p];
        affected.Add(p);
        affected.Add(otherHalf);

        return dir switch
        {
            Direction.Right => CanMove((p.Y, Math.Max(p.X, otherHalf.X) + 1), dir, affected),
            Direction.Left => CanMove((p.Y, Math.Min(p.X, otherHalf.X) - 1), dir, affected),
            Direction.Up => CanMove((p.Y - 1, p.X), dir, affected) && CanMove((otherHalf.Y - 1, otherHalf.X), dir, affected),
            Direction.Down => CanMove((p.Y + 1, p.X), dir, affected) && CanMove((otherHalf.Y + 1, otherHalf.X), dir, affected),
            _ => false
        };
    }

    Vector2 ProcessInput()
    {
        var emptyLineIdx = Array.FindIndex(InputLines, string.IsNullOrWhiteSpace);
        Vector2 start = new Vector2();

        InputLines.Take(emptyLineIdx).SelectMany((line, y) => line.Select((c, x) =>
        {
            switch (c)
            {
                case '#':
                    _wallsA.Add((y, x));
                    _wallsB.Add((y, x * 2));
                    _wallsB.Add((y, x * 2 + 1));
                    break;
                case 'O':
                    _boxesA.Add((y, x));
                    _boxesB.Add((y, 2 * x), (y, 2 * x + 1));
                    _boxesB.Add((y, 2 * x + 1), (y, 2 * x));
                    break;
                case '@':
                    start = (y, x);
                    break;
            }

            return 0;

        })).ToArray();

        _moves = string.Join("", InputLines[(emptyLineIdx + 1)..]).Select(c => c switch
        {
            '^' => Direction.Up,
            '>' => Direction.Right,
            'v' => Direction.Down,
            '<' => Direction.Left,
            _ => throw new Exception("Wrong input")
        }).ToArray();

        return start;
    }
}