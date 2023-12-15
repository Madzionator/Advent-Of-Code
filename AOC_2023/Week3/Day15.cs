using Advent._2023.Day;

namespace AdventOfCode2023.Week3;

class Day15 : IDay
{
    public void Execute()
    {
        var input = File.ReadAllText(@"Week3\input15.txt").Split(',');

        Console.WriteLine($"A: {TaskA(input)}");
        Console.WriteLine($"B: {TaskB(input)}");
    }

    int TaskA(string[] input) => input.Select(HashAlgorithm).Sum();

    int HashAlgorithm(string str)
    {
        var current = 0;
        foreach (var c in str)
        {
            current += c;
            current *= 17;
            current %= 256;
        }

        return current;
    }

    int TaskB(string[] input)
    {
        var boxes = CreateBoxes();

        foreach (var instruction in input)
        {
            var x = instruction.Split(new []{'=', '-'}, StringSplitOptions.RemoveEmptyEntries);

            var box = boxes[HashAlgorithm(x[0])];
            var idx = box.Lenses.FindIndex(b => b.Label == x[0]);

            if (x.Length > 1) // '='
            {
                if (idx >= 0)
                    box.Lenses[idx] = (x[0], int.Parse(x[1]));
                else
                    box.Lenses.Add((x[0], int.Parse(x[1])));
            }
            else if (idx >= 0) // '-'
            {
                box.Lenses.RemoveAt(idx);
            }
        }

        var result = 0;
        foreach (var box in boxes)
            for (var i = 0; i < box.Lenses.Count; i++)
                result += (box.Idx + 1) * (i + 1) * box.Lenses[i].FocalLength;

        return result;
    }

    record Box(int Idx)
    {
        public readonly List<(string Label, int FocalLength)> Lenses = new();
    }

    Box[] CreateBoxes()
    {
        var list = new List<Box>();
        for(var i = 0; i < 256; i++)
            list.Add(new Box(i));

        return list.ToArray();
    }
}