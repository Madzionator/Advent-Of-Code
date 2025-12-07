namespace AdventOfCode2025.Days;

[AocData("example3_1.txt", 357, 3121910778619L)]
[AocData("input3.txt", 17330, 171518260283767L)]
public class Day3 : Day
{
    public override (object? PartA, object? PartB) Execute(string[] inputLines)
    {
        var input = inputLines
            .Where(x => x.Length > 0)
            .Select(line => line
                .Select((c, i) => (Battery: c - '0', Index: i))
                .ToArray())
            .ToList();

        return (TaskA(input), TaskB(input));
    }

    private int TaskA(List<(int Battery, int Index)[]> input) =>
        (from bank in input 
            let firstMax = bank[..^1].MaxBy(x => x.Battery) 
            let secondMax = bank[(firstMax.Index + 1)..].MaxBy(x => x.Battery) 
            select (firstMax.Battery * 10 + secondMax.Battery))
        .Sum();

    private long TaskB(List<(int Battery, int Index)[]> input)
    {
        var totalOutputJoltage = 0L;

        foreach (var bank in input)
        {
            var outputJoltage = 0L;
            var searchStartIndex = 0;

            for (var b = 1; b <= 12; b++)
            {
                var currentMax = bank[searchStartIndex..^(12-b)].MaxBy(x => x.Battery);
                outputJoltage = outputJoltage * 10 + currentMax.Battery;
                searchStartIndex = currentMax.Index + 1;
            }

            totalOutputJoltage += outputJoltage;
        }

        return totalOutputJoltage;
    }
}