using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Week3;

internal class Day21 : Day
{
    private readonly Dictionary<(char, char), List<string>> _numericPaths = [];
    private readonly Dictionary<(char, char), List<string>> _directionPaths = [];

    private readonly Dictionary<(int robot, string entry), long> _memorized = [];

    public override (object resultA, object resultB) Execute()
    {
        GeneratePadKeyPaths(_numPad, _numericPaths);
        GeneratePadKeyPaths(_dirPad, _directionPaths);

        return (Task(2), Task(25));
    }

    long Task(int robots) => InputLines.Sum(line => FirstRobotKeys(line).Min(x => OtherRobotKeys(x, robots)) * int.Parse(line[..^1]));

    List<string> FirstRobotKeys(string entry)
    {
        var keyToPush = new List<string> { "" };
        var previousKey = 'A';

        foreach (var currentKey in entry)
        {
            keyToPush = keyToPush
                .SelectMany(path => _numericPaths[(previousKey, currentKey)]
                    .Select(option => $"{path}{option}A"))
                .ToList();

            previousKey = currentKey;
        }

        return keyToPush;
    }

    long OtherRobotKeys(string entry, int robot)
    {
        if (robot == 0)
        {
            return entry.Length;
        }

        if (_memorized.TryGetValue((robot, entry), out var length))
        {
            return length;
        }

        var splits = entry.Split('A')
            .SkipLast(1)
            .Select(x => $"{x}A")
            .ToArray();

        var totalLength = splits
            .Select(FindPaths)
            .Select(paths => paths
                .Select(x => OtherRobotKeys(x, robot - 1))
                .Min())
            .Sum();

        _memorized[(robot, entry)] = totalLength; 
        return totalLength;
    }

    private List<string> FindPaths(string split)
    {
        List<string> currentPaths = [""];
        var previousKey = 'A';

        foreach (var c in split)
        {
            currentPaths = currentPaths
                .SelectMany(path => _directionPaths[(previousKey, c)]
                    .Select(option => $"{path}{option}A"))
                .ToList();

            previousKey = c;
        }

        return currentPaths;
    }

    #region Generate key paths based on pad layouts.

    void GeneratePadKeyPaths(Dictionary<char, Vector2> pad, Dictionary<(char, char), List<string>> finalPaths)
    {
        var keys = pad.Keys.ToArray();
        var combinations = keys.SelectMany(x => keys, (k1, k2) => (k1, k2)).ToArray();

        foreach (var combination in combinations)
        {
            var paths = new List<string>();
            TraversePadKeyPath(pad[combination.k1], pad[combination.k2], "", paths, pad);
            finalPaths[(combination.k1, combination.k2)] = paths;
        }
    }

    void TraversePadKeyPath(Vector2 currentP, Vector2 endKey, string currentPath, List<string> paths, Dictionary<char, Vector2> pad)
    {
        if (currentP == endKey)
        {
            paths.Add(currentPath);
            return;
        }

        var diff = currentP - endKey;

        if (diff.X != 0)
        {
            var (newP, c) = diff.X < 0
                ? (currentP + (0, 1), '>')
                : (currentP + (0, -1), '<');

            if (pad.ContainsValue(newP))
            {
                TraversePadKeyPath(newP, endKey, currentPath + c, paths, pad);
            }
        }

        if (diff.Y != 0)
        {
            var (newP, c) = diff.Y < 0
                ? (currentP + (1, 0), 'v')
                : (currentP + (-1, 0), '^');

            if (pad.ContainsValue(newP))
            {
                TraversePadKeyPath(newP, endKey, currentPath + c, paths, pad);
            }
        }
    }

    private Dictionary<char, Vector2> _numPad = new()
    {
        {'7', (0 , 0)},
        {'8', (0 , 1)},
        {'9', (0 , 2)},
        {'4', (1 , 0)},
        {'5', (1 , 1)},
        {'6', (1 , 2)},
        {'1', (2 , 0)},
        {'2', (2 , 1)},
        {'3', (2 , 2)},
        {'0', (3 , 1)},
        {'A', (3 , 2)},
    };

    private Dictionary<char, Vector2> _dirPad = new()
    {
        {'^', (0 , 1)},
        {'A', (0 , 2)},
        {'<', (1 , 0)},
        {'v', (1 , 1)},
        {'>', (1 , 2)}
    };

    #endregion
}