using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Week3;

internal class Day18 : Day
{
    private int Max;
    private int ByteToSimulate;

    private Dictionary<Vector2, int> _bytes;
    private Dictionary<Vector2, int> _costs = new();

    public override (object resultA, object resultB) Execute()
    {
        Max = InputLines.Length < 1000 ? 6 : 70;
        ByteToSimulate = InputLines.Length < 1000 ? 12 : 1024;

        _bytes = InputLines.Select((line, i) =>
            {
                var split = line.Split(',');
                return new { P = new Vector2(int.Parse(split[1]), int.Parse(split[0])), ns = i };
            })
            .ToDictionary(x => x.P, x => x.ns);

        return (TaskA(), TaskB());
    }

    int TaskA()
    {
        _costs.Add(new Vector2(0, 0), 0);
        ReachExit((0, 0), 0);

        return _costs.ContainsKey((Max, Max)) 
            ? _costs[(Max, Max)] 
            : -1;
    }
    
    string TaskB()
    {
        var min = ByteToSimulate + 1;
        var max = _bytes.Count - 1;

        while (min <= max)
        {
            var mid = min + (max - min) / 2;
            ByteToSimulate = mid;

            _costs.Clear();
            TaskA();

            if (!_costs.ContainsKey((Max, Max)))
            {
                max = mid - 1;
            }
            else
            {
                min = mid + 1;
            }
        }

        var block = _bytes.FirstOrDefault(x => x.Value == min - 1);

        return $"{block.Key.X},{block.Key.Y}";
    }

    void ReachExit(Vector2 position, int cost)
    {
        if (position == (Max, Max))
            return;

        foreach (var dir in Direction2.Sides)
        {
            var newPos = position.Move(dir);
            if (IsValidPosition(newPos, cost + 1))
            {
                _costs[newPos] = cost + 1;
                ReachExit(newPos, cost + 1);
            }
        }
    }

    bool IsValidPosition(Vector2 p, int pCost)
    {
        if(p.Y < 0 || p.X < 0 || p.Y > Max || p.X > Max)
            return false;

        if(_bytes.ContainsKey(p) && _bytes[p] < ByteToSimulate)
            return false;

        if (!_costs.TryGetValue(p, out var cost))
            return true;

        return cost > pCost;
    }
}