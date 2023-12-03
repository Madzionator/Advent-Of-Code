using Advent._2023.Day;

namespace AdventOfCode2023.Week1;

class Day3 : IDay
{
    record Number(int Idx, int Value);

    Dictionary<(int X, int Y), Number> _numbers = new ();
    Dictionary<(int X, int Y), char> _symbols = new ();

    public void Execute()
    {
        ReadInput();

        Console.WriteLine($"A: {TaskA()}");
        Console.WriteLine($"B: {TaskB()}");
    }
    
    int TaskA()
    {
        var partNumbers = new Dictionary<int, int>();

        foreach (var symbol in _symbols)
        {
            int x = symbol.Key.X;
            int y = symbol.Key.Y;

            CheckAndAddUnique(x - 1, y - 1);
            CheckAndAddUnique(x, y - 1);
            CheckAndAddUnique(x + 1, y - 1);

            CheckAndAddUnique(x - 1, y);
            CheckAndAddUnique(x + 1, y);

            CheckAndAddUnique(x - 1, y + 1);
            CheckAndAddUnique(x, y + 1);
            CheckAndAddUnique(x + 1, y + 1);
        }

        return partNumbers.Sum(x => x.Value);

        void CheckAndAddUnique(int x, int y)
        {
            if (_numbers.ContainsKey((x, y)))
                partNumbers.TryAdd(_numbers[(x, y)].Idx, _numbers[(x, y)].Value);
        }
    }

    int TaskB()
    {
        return _symbols
            .Where(symbol => symbol.Value == '*')
            .Sum(symbol => CheckGearRatioB(symbol.Key.X, symbol.Key.Y));
    }

    int CheckGearRatioB(int x, int y)
    {
        var nextToGearQuantity = 0;
        var gearRatio = 1;
        var alreadyIncluded = new List<Number>();

        CalculatePartRatio(x - 1, y - 1);
        CalculatePartRatio(x, y - 1);
        CalculatePartRatio(x + 1, y - 1);

        CalculatePartRatio(x - 1, y);
        CalculatePartRatio(x + 1, y);

        CalculatePartRatio(x - 1, y + 1);
        CalculatePartRatio(x, y + 1);
        CalculatePartRatio(x + 1, y + 1);

        return nextToGearQuantity == 2 ? gearRatio : 0;

        void CalculatePartRatio(int X, int Y)
        {
            if (!_numbers.ContainsKey((X, Y)))
                return;

            var num = _numbers[(X, Y)];
            if (!alreadyIncluded.Contains(num))
            {
                alreadyIncluded.Add(num);
                nextToGearQuantity++;
                gearRatio *= num.Value;
            }
        }
    }

    void ReadInput()
    {
        int idx = -1, lastIdx = -1;

        _ = File.ReadAllText(@"Week1\input3.txt")
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select((str, y) =>
            {
                int xb = -2, xe = -2, val = 0;

                for (var j = 0; j < str.Length; j++)
                {
                    if (char.IsDigit(str[j]))
                    {
                        val = val * 10 + (str[j] - '0');

                        if (lastIdx == idx)
                        {
                            xb = j;
                            idx++;
                        }

                        if (j + 1 == str.Length || !char.IsDigit(str[j + 1]))
                        {
                            xe = j;
                            for (var x = xb; x <= xe; x++)
                                _numbers.Add((x, y), new Number(idx, val));
                            val = 0;
                            lastIdx++;
                        }
                    }
                    else if (str[j] != '.')
                        _symbols.Add((j, y), str[j]);
                }

                return str;
            })
            .ToList();
    }
}