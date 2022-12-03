using Advent._2022.Day;

namespace Advent._2022.Week1;

class Day3 : IDay
{
    public void Execute()
    {
        var input = File.ReadAllLines(@"Week1\input3.txt");

        Console.WriteLine(TaskA(input));
        Console.WriteLine(TaskB(input));
    }

    private int TaskA(string[] items)
    {
        return items
            .Select(x => (x[..(x.Length / 2)], x[(x.Length / 2)..]))
            .Select(x => x.Item1
                .Intersect(x.Item2)
                .First())
            .Select(p => p >= 'a' ? p - 'a' + 1 : p - 'A' + 27)
            .Sum();
    }

    private int TaskB(string[] items)
    {
        return items
            .Chunk(3)
            .Select(group => group[0]
                .Intersect(group[1]
                    .Intersect(group[2]))
                .First())
            .Select(p => p >= 'a' ? p - 'a' + 1 : p - 'A' + 27)
            .Sum();
    }
}