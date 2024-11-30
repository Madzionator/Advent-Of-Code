namespace AdventOfCode2024.Week1;

class Day1 : IDay
{
    public void Execute()
    {
        var input = File.ReadAllText(@"Week1\input1.txt")
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        Console.WriteLine($"A: {TaskA(input)}");
        Console.WriteLine($"B: {TaskB(input)}");
    }

    int TaskA(List<string> input)
    {
        return 0;
    }

    int TaskB(List<string> input)
    {
        return 0;
    }
}