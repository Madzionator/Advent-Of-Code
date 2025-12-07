namespace AdventOfCode2025.Days;

[AocData("example5_1.txt", 3, 14L)]
[AocData("input5.txt", 601, 367899984917516L)]
public class Day5 : Day
{
    public override (object? PartA, object? PartB) Execute(string[] inputLines)
    {
        var blankLine = inputLines.IndexOf("");

        var availableIds = inputLines[(blankLine + 1)..].Select(long.Parse).ToList();
        var inputRanges = inputLines[..blankLine].Select(line =>
        {
            var range = line.Split('-');
            return (Start: long.Parse(range[0]), End: long.Parse(range[1]));
        }).ToList();
         
        var notOverlappingRanges = GetNotOverlappingRanges(inputRanges);

        var resultPartA = availableIds.Count(id => notOverlappingRanges.Any(r => id >= r.Start && id <= r.End));
        var resultPartB = notOverlappingRanges.Sum(range => range.End - range.Start + 1);

        return (resultPartA, resultPartB);
    }

    private HashSet<(long Start, long End)> GetNotOverlappingRanges(List<(long Start, long End)> inputRanges)
    {
        var notOverlappingRanges = new HashSet<(long Start, long End)>();

        foreach (var range in inputRanges)
        {
            var newRange = range;
            var overlaps = notOverlappingRanges.Where(r => r.Start <= range.End && r.End >= range.Start).ToList();

            foreach (var overlap in overlaps)
            {
                newRange.Start = Math.Min(newRange.Start, overlap.Start);
                newRange.End = Math.Max(newRange.End, overlap.End);

                notOverlappingRanges.Remove(overlap);
            }

            notOverlappingRanges.Add(newRange);
        }

        return notOverlappingRanges;
    }
}