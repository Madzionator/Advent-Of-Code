namespace AdventOfCode2024.Week4;

internal class Day25 : Day
{
    public override (object resultA, object resultB) Execute()
    {
        var schematics = InputLines.Chunk(8).Select(x => x[..7].ToCharMatrix()).ToList();
        
        return (TaskA(schematics), "");
    }

    int TaskA(List<char[,]> schematics)
    {
        var locks = schematics.Where(x => x[0, 0] == '#').Select(GetHeights).ToList();
        var keys = schematics.Where(x => x[0, 0] == '.').Select(GetHeights).ToList();

        var result = locks
            .Sum(@lock => keys
                .Count(key => @lock
                    .Zip(key, (l, k) => l + k)
                    .All(s => s <= 5)));

        return result;
    }

    int[] GetHeights(char[,] schema) => Enumerable.Range(0, 5)
        .Select(x => Enumerable.Range(0, 7)
            .Count(y => schema[y, x] == '#') - 1)
        .ToArray();
}