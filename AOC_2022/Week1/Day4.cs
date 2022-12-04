using Advent._2022.Day;

namespace Advent._2022.Week1;

class Day4 : IDay
{
    public void Execute()
    {
        var input = File.ReadAllLines(@"Week1\input4.txt")
            .Select(x => x
                .Split(',', '-')
                .Select(int.Parse)
                .ToArray());

        Console.WriteLine(TaskA(input));
        Console.WriteLine(TaskB(input));
    }

    private int TaskA(IEnumerable<int[]> sections) => 
        sections.Count(s => (s[0] >= s[2] && s[1] <= s[3]) || (s[0] <= s[2] && s[1] >= s[3]));

    private int TaskB(IEnumerable<int[]> sections) => 
        sections.Count(s => (s[0] <= s[2] && s[1] >= s[2]) || (s[0] > s[2] && s[0] <= s[3]));
}