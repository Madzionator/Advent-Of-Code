namespace Advent._2022.Week1;
using Day;

class Day2 : IDay
{
    public void Execute()
    {
        var rounds = File.ReadAllLines(@"Week1\input2.txt")
            .Select(x => (x[0], x[2]))
            .ToList();

        Console.WriteLine(TaskA(rounds));
        Console.WriteLine(TaskB(rounds));
    }

    public int TaskA(List<(char, char)> rounds)
    {
        return rounds.Select(round => round switch
            {
                ('A', 'X') => 1 + 3,
                ('A', 'Y') => 2 + 6,
                ('A', 'Z') => 3 + 0,
                ('B', 'X') => 1 + 0,
                ('B', 'Y') => 2 + 3,
                ('B', 'Z') => 3 + 6,
                ('C', 'X') => 1 + 6,
                ('C', 'Y') => 2 + 0,
                ('C', 'Z') => 3 + 3
            })
            .Sum();
    }

    public int TaskB(List<(char, char)> rounds)
    {
        return rounds.Select(round => round switch
            {
                ('A', 'X') => 3 + 0,
                ('A', 'Y') => 1 + 3,
                ('A', 'Z') => 2 + 6,
                ('B', 'X') => 1 + 0,
                ('B', 'Y') => 2 + 3,
                ('B', 'Z') => 3 + 6,
                ('C', 'X') => 2 + 0,
                ('C', 'Y') => 3 + 3,
                ('C', 'Z') => 1 + 6
            })
            .Sum();
    }
}