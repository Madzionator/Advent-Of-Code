using Advent._2022.Day;

namespace Advent._2022.Week1;

class Day2 : IDay
{
    public void Execute()
    {
        var rounds = File.ReadAllLines(@"Week1\input2.txt")
            .Select(x => ((Shape)x[0] - 'A' + 1, (Shape)x[2] - 'X' + 1))
            .ToList();

        Console.WriteLine(TaskA(rounds));
        Console.WriteLine(TaskB(rounds.Select(x => (x.Item1, (Goal)x.Item2))));
    }

    private int TaskA(List<(Shape, Shape)> rounds)
    {
        var res = rounds.Select(round => (int)round.Item2 + round switch
        {
            (Shape.Rock, Shape.Rock) => Result.Draw,
            (Shape.Rock, Shape.Paper) => Result.Win,
            (Shape.Rock, Shape.Scissors) => Result.Loss,
            (Shape.Paper, Shape.Rock) => Result.Loss,
            (Shape.Paper, Shape.Paper) => Result.Draw,
            (Shape.Paper, Shape.Scissors) => Result.Win,
            (Shape.Scissors, Shape.Rock) => Result.Win,
            (Shape.Scissors, Shape.Paper) => Result.Loss,
            (Shape.Scissors, Shape.Scissors) => Result.Draw,
        });

        return res.Cast<int>().Sum();
    }

    private int TaskB(IEnumerable<(Shape, Goal)> rounds)
    {
        var res = rounds.Select(round => round switch
        {
            (Shape.Rock, Goal.Loss) => (Shape.Scissors, Result.Loss),
            (Shape.Rock, Goal.Draw) => (Shape.Rock, Result.Draw),
            (Shape.Rock, Goal.Win) => (Shape.Paper, Result.Win),
            (Shape.Paper, Goal.Loss) => (Shape.Rock, Result.Loss),
            (Shape.Paper, Goal.Draw) => (Shape.Paper, Result.Draw),
            (Shape.Paper, Goal.Win) => (Shape.Scissors, Result.Win),
            (Shape.Scissors, Goal.Loss) => (Shape.Paper, Result.Loss),
            (Shape.Scissors, Goal.Draw) => (Shape.Scissors, Result.Draw),
            (Shape.Scissors, Goal.Win) => (Shape.Rock, Result.Win)
        });

        return res.Select(x => (int)x.Item1 + (int)x.Item2).Sum();
    }

    enum Shape
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3
    }

    enum Goal
    {
        Loss = 1,
        Draw = 2,
        Win = 3
    }

    enum Result
    {
        Loss = 0,
        Draw = 3,
        Win = 6
    }
}