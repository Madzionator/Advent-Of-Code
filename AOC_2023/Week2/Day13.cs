using Advent._2023.Day;

namespace AdventOfCode2023.Week2;

class Day13 : IDay
{
    public void Execute()
    {
        var patterns = ReadInputPatterns();

        Console.WriteLine($"A: {Task(patterns, 0)}");
        Console.WriteLine($"B: {Task(patterns, 1)}");
    }

    record Pattern(string[] Rows, string[] Columns);

    int Task(List<Pattern> patterns, int smudge)
    {
        int rowSum = 0, colSum = 0;

        foreach (var pat in patterns)
        {
            colSum += FindReflectionLine(pat.Columns, smudge);
            rowSum += FindReflectionLine(pat.Rows, smudge);
        }

        return rowSum * 100 + colSum;
    }

    int CountDifferences(string str1, string str2) => str1.Where((c, idx) => c != str2[idx]).Count();

    int FindReflectionLine(string[] lines, int smudge)
    {
        for (var i = 0; i < lines.Length - 1; i++)
        {
            var d = CountDifferences(lines[i], lines[i + 1]);

            if (d <= smudge)
            {
                for (var j = 1; j <= i; j++)
                {
                    if (i + 1 + j >= lines.Length)
                        break;

                    d += CountDifferences(lines[i - j], lines[i + 1 + j]);
                    if (d > smudge)
                        break;
                }

                if (d == smudge)
                {
                    return i + 1;
                }
            }
        }

        return 0;
    }

    List<Pattern> ReadInputPatterns() =>
        File.ReadAllText(@"Week2\input13.txt")
            .Split($"{Environment.NewLine}{Environment.NewLine}")
            .Select(block =>
            {
                var x = block.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

                var c = new List<string>();
                for (var i = 0; i < x[0].Length; i++)
                {
                    c.Add(new string(x.Select(r => r[i]).ToArray()));
                }

                return new Pattern(x, c.ToArray());

            }).ToList();
}