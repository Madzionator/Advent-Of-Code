namespace AdventOfCode2024.Week4;

internal class Day23 : Day
{
    private Dictionary<string, List<string>> _connections = new ();

    public override (object resultA, object resultB) Execute()
    {
        ProcessInputToConnections();

        return (TaskA(), TaskB());
    }

    int TaskA()
    {
        var setsOfThree = new HashSet<string>();

        foreach (var c1 in _connections.Where(x => x.Key.StartsWith('t')))
        foreach (var c2 in c1.Value)
        foreach (var c3 in c1.Value.Intersect(_connections[c2]))
        {
            var setStr = string.Join(',', new[] { c1.Key, c2, c3 }.Order());
            setsOfThree.Add(setStr);
        }

        return setsOfThree.Count;
    }

    string TaskB()
    {
        var vertices = _connections.Select(x => x.Key).ToHashSet();

        var largestClique = new List<string>();
        BronKerbosch(new HashSet<string>(), vertices, ref largestClique);

        return string.Join(',', largestClique.Order());
    }

    void BronKerbosch(HashSet<string> currentClique, HashSet<string> potentialVertices, ref List<string> largestClique)
    {
        if (!potentialVertices.Any())
        {
            if (currentClique.Count > largestClique.Count)
            {
                largestClique = currentClique.ToList();
            }
            return;
        }

        var pvCopy = potentialVertices.ToHashSet();
        foreach (var node in pvCopy)
        {
            var newClique = new HashSet<string>(currentClique) { node };
            var remainingVertices = new HashSet<string>(potentialVertices.Intersect(_connections[node]));
            BronKerbosch(newClique, remainingVertices, ref largestClique);

            potentialVertices.Remove(node);
        }
    }

    void ProcessInputToConnections()
    {
        foreach (var (c1, c2) in InputLines.Select(line => (line[..2], line[3..])))
        {
            if (_connections.ContainsKey(c1))
            {
                _connections[c1].Add(c2);
            }
            else
            {
                _connections[c1] = [c2];
            }

            if (_connections.ContainsKey(c2))
            {
                _connections[c2].Add(c1);
            }
            else
            {
                _connections[c2] = [c1];
            }
        }
    }
}