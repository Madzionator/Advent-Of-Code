global using Advent.Helpers.Extensions;
global using Advent.Helpers.Methods;

using System.Diagnostics;
using Humanizer;

namespace AdventOfCode2024;

public abstract class Day
{
    public string[] InputLines = null!;

    public void Run(int day)
    {
        InputLines = File.ReadAllLines(GetInputPath(day));

        if (InputLines.Length == 0)
        {
            throw new Exception("Input file is empty");
        }

#if DEBUG
        BasicTimeMeasurement();
#else
        TimeMeasurement();
#endif
    }

    public abstract (object resultA, object resultB) Execute();

    private string GetInputPath(int day, bool isExample = false)
    {
        var week = (day - 1) / 7 + 1;
        var fileName = isExample ? $"example{day}.txt" : $"input{day}.txt";
        return Path.Combine($"Week{week}", fileName);
    }

    private void BasicTimeMeasurement()
    {
        var s = Stopwatch.StartNew();
        var results = Execute();
        s.Stop();

        Console.WriteLine($"A: {results.resultA}");
        Console.WriteLine($"B: {results.resultB}");

        Console.WriteLine($"\n\nTime Elapsed: {s.Elapsed.Humanize(precision: 2)}");
    }

    private void TimeMeasurement()
    {
        // warm up
        var results = Execute();
        for (int i = 0; i < 4; i++) 
        {
            Execute();
        }

        // measurement
        var repeats = 10;
        var s = Stopwatch.StartNew();
        for (int i = 0; i < repeats; i++)
        {
            Execute();
        }
        s.Stop();

        Console.WriteLine($"A: {results.resultA}");
        Console.WriteLine($"B: {results.resultB}");

        var mean = s.Elapsed / repeats;
        Console.WriteLine($"\n\nMean elapsed time: {mean.Humanize(precision: 4)}");
    }
}
