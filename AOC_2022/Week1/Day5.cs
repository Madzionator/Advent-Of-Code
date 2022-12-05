using Advent._2022.Day;

namespace Advent._2022.Week1;

class Day5 : IDay
{
    public void Execute()
    {
        var input = File.ReadAllText(@"Week1\input5.txt")
            .Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Split(Environment.NewLine))
            .ToArray();

        var procedure = input[1]
            .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Select(x => (int.Parse(x[1]), int.Parse(x[3]) - 1, int.Parse(x[5]) - 1));

        var stacks = new string[(input[0][0].Length+1)/4];
        for (var i = input[0].Length - 2; i >= 0; i--)
        for (var j = 1; j < input[0][0].Length; j += 4)
                stacks[(j - 1) / 4] += input[0][i][j] == ' ' ? "" : input[0][i][j];

        Console.WriteLine(TaskA(stacks.ToArray(), procedure));
        Console.WriteLine(TaskB(stacks, procedure));
    }

    private string TaskA(string[] stacks, IEnumerable<(int, int, int)> procedure)
    {
        foreach (var (quantity, from, to) in procedure)
            for (var i = 0; i < quantity; i++)
            {
                var toMove = stacks[from][^1];
                stacks[from] = stacks[from][..^1];
                stacks[to] += toMove;
            }

        return string.Join("", stacks.Select(x => x[^1]));
    }

    private string TaskB(string[] stacks, IEnumerable<(int, int, int)> procedure)
    {
        foreach (var (quantity, from, to) in procedure)
        {
            var toMove = stacks[from][(stacks[from].Length - quantity)..stacks[from].Length];
            stacks[from] = stacks[from][..(stacks[from].Length - quantity)];
            stacks[to] += toMove;
        }

        return string.Join("", stacks.Select(x => x[^1]));
    }
}