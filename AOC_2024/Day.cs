using System.Diagnostics;

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

        var s = Stopwatch.StartNew();
        var results = Execute();
        s.Stop();
        
        Console.WriteLine($"A: {results.resultA}");
        Console.WriteLine($"B: {results.resultB}");

        Console.WriteLine($"\n\nTime Elapsed: {s.ElapsedMilliseconds}ms");
    }

    public abstract (object resultA, object resultB) Execute();

    private string GetInputPath(int day, bool isExample = false)
    {
        var week = (day - 1) / 7 + 1;
        var fileName = isExample ? $"example{day}.txt" : $"input{day}.txt";
        return Path.Combine($"Week{week}", fileName);
    }
}
