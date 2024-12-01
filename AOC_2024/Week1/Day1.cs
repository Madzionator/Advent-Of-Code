﻿namespace AdventOfCode2024.Week1;

class Day1 : IDay
{
    public void Execute()
    {
        var input = File.ReadAllText(@"Week1\input1.txt")
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Split("  ").Select(int.Parse).ToArray())
            .Select(x => (L: x[0], R: x[1]))
            .ToList();

        Console.WriteLine($"A: {TaskA(input)}");
        Console.WriteLine($"B: {TaskB(input)}");
    }

    long TaskA(List<(int L, int R)> input)
    {
        return input.Select(x => x.L).Order()
            .Zip(input.Select(x => x.R).Order(), 
                (l, r) => Math.Abs(l - r))
            .Sum();
    }

    int TaskB(List<(int L, int R)> input)
    {
        var leftCount = input.Select(x => x.L).CountBy(x => x).ToDictionary();
        var rightCount = input.Select(x => x.R).CountBy(x => x).ToDictionary();

        return leftCount.Aggregate(0, (sum, left) 
            => sum + left.Key * left.Value * rightCount.GetValueOrDefault(left.Key, 0));
    }
}