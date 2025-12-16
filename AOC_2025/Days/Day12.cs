namespace AdventOfCode2025.Days;

//[AocData("example12_1.txt", 2)] doesnt't work for example input
[AocData("input12.txt", 497)]
public class Day12 : Day
{
    private record Region(int Width, int Length, int[] ShapeQuantities);

    public override (object? PartA, object? PartB) Execute(string[] inputLines)
    {
        var lastEmptyLine = inputLines.LastIndexOf("");

        var shapes = inputLines
            .Take(lastEmptyLine)
            .Chunk(5)
            .Select(chunk => chunk.Sum(x => x.Count(c => c == '#')))
            .ToArray();

        var regions = inputLines[(lastEmptyLine + 1)..].Select(line =>
        {
            var spits = line.Split(": ");
            var size = spits[0].Split('x');
            var quantities = spits[1].Split(' ').Select(int.Parse).ToArray();

            return new Region(int.Parse(size[0]), int.Parse(size[1]), quantities);
        }).ToList();

        var canFitCount = 0;

        foreach (var region in regions)
        {
            var maxSize = region.Width * region.Length;
            var size = shapes.Select((t, s) => t * region.ShapeQuantities[s]).Sum();

            if(size <= maxSize)
            {
                canFitCount++;
            }
        }

        return (canFitCount, null);
    }
}