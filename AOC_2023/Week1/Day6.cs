using Advent._2023.Day;

namespace AdventOfCode2023.Week1;

class Day6 : IDay
{
    record Race(long Time, long Distance);

    public void Execute()
    {
        var input = File.ReadAllLines(@"Week1\input6.txt")
            .Select(line => line.Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .ToArray();

        var races = input[0].Select((num, i) => new Race(long.Parse(num), long.Parse(input[1][i]))).ToArray();

        Console.WriteLine($"A: {TaskA(races)}");
        Console.WriteLine($"B: {TaskB(races)}");
    }

    int TaskA(Race[] races) => races.Aggregate(1, (current, race) => current * NumberOfWaysToWin(race));

    static int NumberOfWaysToWin(Race race)
    {
        var deltaSqrt = Math.Sqrt(race.Time * race.Time - 4 * race.Distance);

        var x1 = (race.Time - deltaSqrt) / 2;
        var x2 = (race.Time + deltaSqrt) / 2;

        var firstWinning = x1 % 1 == 0 ? x1 + 1 : Math.Ceiling(x1);
        var lastWinning = x2 % 1 == 0 ? x2 - 1 : Math.Floor(x2);

        return (int)(lastWinning - firstWinning + 1);
    }

    int TaskB(Race[] races)
    {
        var time = string.Join("", races.Select(race => race.Time));
        var dist = string.Join("", races.Select(race => race.Distance));

        var bigRace = new Race(long.Parse(time), long.Parse(dist));
        return NumberOfWaysToWin(bigRace);
    }
}