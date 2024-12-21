using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Week3;

internal class Day20 : Day
{
    private Dictionary<Vector2, int> _track = new();
    private char[,] _map;

    private List<(int steps, int gain)> _cheats = new();

    public override (object resultA, object resultB) Execute()
    {
        _map = InputLines.ToCharMatrix();

        FindMainTrack();
        CalculateAvailableCheats();

        return (TaskA(), TaskB());
    }

    int TaskA() => _cheats.Count(cheat => cheat is { steps: 2, gain: >= 100 });

    int TaskB() => _cheats.Count(cheat => cheat.gain >= 100);

    void CalculateAvailableCheats()
    {
        var shifts = new List<Vector2>();
        for (var y = -20; y <= 20; y++)
        for (var x = -20; x <= 20; x++) 
            if (!(y == 0 && x == 0) && Math.Abs(y) + Math.Abs(x) <= 20) 
                shifts.Add((y, x));

        foreach (var (startPos, startCost) in _track.SkipLast(1))
        foreach (var shift in shifts)
        {
            if (!_track.TryGetValue(startPos + shift, out var endCost))
                continue;

            var steps = Math.Abs(shift.Y) + Math.Abs(shift.X);
            var gain = endCost - (startCost + steps);

            if (gain > 0)
            {
                _cheats.Add((steps, gain));
            }
        }
    }

    void FindMainTrack()
    {
        // start point
        var p = InputLines.SelectMany((line, y) => line.Select((c, x) => new { y, x, c }))
            .Where(v => v.c == 'S')
            .Select(v => new Vector2(v.y, v.x))
            .First();

        var cost = 0;
        _track.Add(p, cost);

        // start direction
        Direction2 dir = _map[p.Y - 1, p.X] == '.' ? Direction.Up
            : _map[p.Y, p.X + 1] == '.' ? Direction.Right
            : _map[p.Y + 1, p.X] == '.' ? Direction.Down 
            : Direction.Left;

        // follow '.' path
        while (_map[p.Y, p.X] != 'E')
        {
            cost++;
            var ps = p.Move(dir);
            if (_map[ps.Y, ps.X] is '.' or 'E')
            {
                _track.Add(ps, cost);
                p = ps;
                continue;
            }

            var pr = p.Move(dir.Rotate(Rotation.Right90));
            if (_map[pr.Y, pr.X] is '.' or 'E')
            {
                _track.Add(pr, cost);
                p = pr;
                dir = dir.Rotate(Rotation.Right90);
                continue;
            }

            var pl = p.Move(dir.Rotate(Rotation.Left90));
            _track.Add(pl, cost);
            p = pl;
            dir = dir.Rotate(Rotation.Left90);
        }
    }
}