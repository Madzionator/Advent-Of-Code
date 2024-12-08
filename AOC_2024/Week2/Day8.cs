using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Week2;

internal class Day8 : Day
{
    private List<(Vector2 A, Vector2 B)> _antennaPairs;

    public override (object resultA, object resultB) Execute()
    {
        _antennaPairs = InputLines
            .SelectMany((line, y) => line
                .Select((c, x) => (c, Position: new Vector2(x, y))))
            .Where(v => v.c is not '.')
            .GroupBy(v => v.c)
            .SelectMany(g =>
                from i in Enumerable.Range(0, g.Count() - 1)
                from j in Enumerable.Range(i + 1, g.Count() - (i + 1))
                select(A: g.ElementAt(i).Position, B: g.ElementAt(j).Position))
            .ToList();

        return (TaskA(), TaskB());
    }

    int TaskA()
    {
        var antinodes = new HashSet<Vector2>();

        foreach (var (a, b) in _antennaPairs)
        {
            var shift = a - b;

            var antinodeA = a + shift;
            var antinodeB = b - shift;

            if (IsInValidPosition(antinodeA))
                antinodes.Add(antinodeA);

            if (IsInValidPosition(antinodeB))
                antinodes.Add(antinodeB);
        }

        return antinodes.Count;
    }

    int TaskB()
    {
        var antinodes = new HashSet<Vector2>();

        foreach (var (a, b) in _antennaPairs)
        {
            antinodes.Add(a);
            antinodes.Add(b);

            var shift = a - b;

            var antinodeA = a + shift;
            var antinodeB = b - shift;

            while (IsInValidPosition(antinodeA))
            {
                antinodes.Add(antinodeA);
                antinodeA += shift;
            }

            while (IsInValidPosition(antinodeB))
            {
                antinodes.Add(antinodeB);
                antinodeB -= shift;
            }
        }

        return antinodes.Count;
    }

    bool IsInValidPosition(Vector2 vector) => vector.Y >= 0 && vector.X >= 0 && vector.Y < InputLines.Length && vector.X < InputLines[0].Length;
}