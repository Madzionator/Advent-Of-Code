namespace Advent._2022.Week1;
using Day;

class Day1 : IDay
{
    public void Execute()
    {
        var calories = File.ReadAllText(@"Week1\input1.txt")
            .Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .Sum())
            .OrderByDescending(z => z)
            .ToList();

        Console.WriteLine(TaskA(calories));
        Console.WriteLine(TaskB(calories));
    }

    public int TaskA(List<int> calories) => calories.First();

    public int TaskB(List<int> calories) => calories.Take(3).Sum();
}