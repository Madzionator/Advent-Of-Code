namespace AdventOfCode2023.Week2;

using Advent._2023.Day;


class Day9 : IDay
{
    public void Execute()
    {
        var histories = File.ReadAllLines(@"Week2\input9.txt")
            .Select(line => line.Split(' ').Select(int.Parse).ToArray()).ToArray();

        var res = Task(histories);

        Console.WriteLine($"A: {res.Item1}");
        Console.WriteLine($"B: {res.Item2}");
    }

    
    (int, int) Task(int[][] histories)
    {
        int sumA = 0, sumB = 0;
        foreach (var history in histories)
        {
            var rows = ArrangeSequences(history);

            for (var depth = rows.Count - 2; depth >= 0; depth--)
            {
                rows[depth].Add(rows[depth + 1][^1] + rows[depth][^1]);
                rows[depth].Insert(0, rows[depth][0] - rows[depth + 1][0]);
            }

            sumA += rows[0][^1];
            sumB += rows[0][0];
        }

        return (sumA, sumB);
    }

    List<List<int>> ArrangeSequences(int[] history)
    {
        var rows = new List<List<int>> { history.ToList() };

        for (var depth = 1; depth < history.Length - 1; depth++)
        {
            var row = new List<int>();
            for (var i = 0; i < rows[depth - 1].Count - 1; i++)
            {
                var x = rows[depth - 1][i + 1] - rows[depth - 1][i];
                row.Add(x);
            }

            rows.Add(row);
            if (row.All(x => x == 0))
            {
                row.Add(0);
                return rows;
            }
        }

        throw new Exception("Impossible");
    }
}