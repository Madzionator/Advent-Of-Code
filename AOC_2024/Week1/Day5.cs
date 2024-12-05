namespace AdventOfCode2024.Week1;

class Day5 : Day
{
    private readonly Dictionary<int, List<int>> Rules = new();
    private List<int>[] _updates;

    public override (object resultA, object resultB) Execute()
    {
        var emptyLineIdx = Array.FindIndex(InputLines, string.IsNullOrWhiteSpace);

        _ = InputLines
            .Take(emptyLineIdx)
            .Select(line => 
            {
                var split = line.Split('|').Select(int.Parse).ToArray();
                Rules.AddOrUpdateList(split[0], split[1]);
                return line;

            }).ToArray();

        _updates = InputLines
            .Skip(emptyLineIdx+1)
            .Select(line => line.Split(',').Select(int.Parse).ToList())
            .ToArray();

        return (TaskA(), TaskB());
    }

    public int TaskA() => _updates
        .Where(IsUpdateValid)
        .Select(x => x[x.Count / 2])
        .Sum();

    public int TaskB() => _updates
        .Where(x => !IsUpdateValid(x))
        .Select(x =>
        {
            x.Sort(CompareUpdate);
            return x[x.Count / 2];
        })
        .Sum();

    bool IsUpdateValid(List<int> update) 
        => !update
            .Skip(1)
            .Where((num, i) => Rules.TryGetValue(num, out var rule) && rule.Contains(update[i]))
            .Any();
    
    public int CompareUpdate(int x, int y)
    {
        if (Rules.TryGetValue(y, out var rule) && rule.Contains(x))
        {
            return -1;
        }

        if (Rules.TryGetValue(x, out rule) && rule.Contains(y))
        {
            return 1;
        }

        return 0;
    }
}
