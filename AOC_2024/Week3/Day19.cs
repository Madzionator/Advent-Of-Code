namespace AdventOfCode2024.Week3;

internal class Day19 : Day
{
    private HashSet<string> _patterns;
    private string[] _designs;

    private Dictionary<string, long> _possibleDesigns = new();

    // Takes a few seconds
    public override (object resultA, object resultB) Execute()
    {
        _patterns = InputLines[0].Split(", ").ToHashSet();
        _designs = InputLines[2..].ToArray();

        return (TaskA(), TaskB());
    }

    int TaskA() => _designs.Count(d => PossibleArrangements(d) > 0);
    long TaskB() => _designs.Sum(PossibleArrangements);
    

    private long PossibleArrangements(string design)
    {
        if (_possibleDesigns.TryGetValue(design, out long p)) 
            return p;

        p = 0;
        foreach (var pattern in _patterns)
        {
            if (design.Length < 1) 
                return 1;

            if (design.StartsWith(pattern))
            {
                p += PossibleArrangements(design[pattern.Length..]);
            }
        }

        _possibleDesigns[design] = p;
        return p;
    }
}