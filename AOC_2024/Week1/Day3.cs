using System.Text.RegularExpressions;

namespace AdventOfCode2024.Week1;

class Day3 : Day
{
    private const string MulPattern = @"mul\((\d{1,3}),(\d{1,3})\)";

    public override (object resultA, object resultB) Execute()
    {
        var input = string.Join("", InputLines);
        return (TaskA(input), TaskB(input));
    }

    int TaskA(string input) 
        => Regex.Matches(input, MulPattern)
            .Select(match => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value))
            .Sum();

    int TaskB(string input) 
        => input.Split("do")
            .Where(x => !x.StartsWith("n't()"))
            .Select(TaskA)
            .Sum();
}