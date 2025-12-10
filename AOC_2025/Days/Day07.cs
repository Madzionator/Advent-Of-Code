using AdventOfCode2025.Helpers;

namespace AdventOfCode2025.Days;

[AocData("example07_1.txt", 21, 40L)]
[AocData("input07.txt", 1615, 43560947406326L)]
public class Day07 : Day
{
    public override (object? PartA, object? PartB) Execute(string[] inputLines)
    {
        var lengthX = inputLines[0].Length;
        var lengthY = inputLines.Length;

        var timelines = new long[lengthY, lengthX];
        timelines[0, inputLines[0].IndexOf('S')] = 1;

        var splits = 0;

        for (var y = 0; y < lengthY - 1; y++)
        for (var x = 0; x < lengthX; x++)
            if (timelines[y, x] > 0)
            {
                var current = timelines[y, x];

                if (inputLines[y+1][x] == '^')
                {
                    splits++;

                    timelines[y + 1, x - 1] += current;
                    timelines[y + 1, x + 1] += current;
                }
                else
                {
                    timelines[y + 1, x] += current;
                }
            }

        var totalTimelines = timelines.GetRow(lengthY - 1).Sum();
        return (splits, totalTimelines);
    }
}