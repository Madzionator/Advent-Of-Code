using Advent._2023.Day;

namespace AdventOfCode2023.Week3;

class Day18 : IDay
{
    record Dig(char Direction, int Meters);

    public void Execute()
    {
        var input = File.ReadAllLines(@"Week3\input18.txt");
        
        var inputA = input.Select(line =>
        {
            var x = line.Split(' ');
            return new Dig(x[0][0], int.Parse(x[1]));
        }).ToList();

        var inputB = input.Select(line =>
        {
            var x = line.Split('(')[1][..^1];

            var val = int.Parse(x[1..^1], System.Globalization.NumberStyles.HexNumber);
            var dir = x[^1] switch
            {
                '0' => 'R',
                '1' => 'D',
                '2' => 'L',
                '3' => 'U'
            };

            return new Dig(dir, val);
        }).ToList();

        Console.WriteLine($"A: {Task(inputA)}");
        Console.WriteLine($"B: {Task(inputB)}");
    }

    long Task(List<Dig> digs)
    {
        var vertices = GetVertices(digs);

        var boundary =  CalculateTrenchLength(digs);
        var inside = CalculateTrenchArea(vertices);

        // Pick's theorem
        return inside + boundary / 2 + 1;
    }

    long CalculateTrenchLength(List<Dig> digs) => digs.Sum(d => d.Meters);

    long CalculateTrenchArea(List<(int Y, int X)> vertices)
    {
        // Shoelace formula
        var s1 = 0L;
        var s2 = 0L;

        for (var i = 0; i < vertices.Count - 1; i++)
        {
            s1 += 1L * vertices[i].X * vertices[i + 1].Y;
            s2 += 1L * vertices[i + 1].X * vertices[i].Y;
        }

        s1 += 1L * vertices[^1].X * vertices[0].Y;
        s2 += 1L * vertices[0].X * vertices[^1].Y;

        return Math.Abs(s1 - s2) / 2;
    }

    List<(int Y, int X)> GetVertices(List<Dig> input)
    {
        var vertices = new List<(int Y, int X)>();
        int y = 0, x = 0;

        foreach (var row in input)
        {
            switch (row.Direction)
            {
                case 'R':
                    x += row.Meters;
                    break;
                case 'L':
                    x -= row.Meters;
                    break;
                case 'U':
                    y -= row.Meters;
                    break;
                case 'D':
                    y += row.Meters;
                    break;
            }

            vertices.Add((y, x));
        }

        return vertices;
    }
}