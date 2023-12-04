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
            for(var dx = -1; dx <= 1; dx ++)
            for (var dy = -1; dy <= 1; dy++) 
                if(!(dx == 0 && dy == 0)) 
                    CheckAndAddUnique(symbol.Key.X + dx, symbol.Key.Y + dy);
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

        for (var dx = -1; dx <= 1; dx++)
        for (var dy = -1; dy <= 1; dy++)
            if (!(dx == 0 && dy == 0))
                CalculatePartRatio(x + dx, y + dy);

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

        _ = File.ReadAllLines(@"Week1\input3.txt")
            .Select((str, y) =>
            {
                int xStart = -2;
                int val = 0;

                for (var j = 0; j < str.Length; j++)
                {
                    if (char.IsDigit(str[j]))
                    {
                        val = val * 10 + (str[j] - '0');

                        if (lastIdx == idx)
                        {
                            xStart = j;
                            idx++;
                        }

                        if (j + 1 == str.Length || !char.IsDigit(str[j + 1]))
                        {
                            for (var x = xStart; x <= j; x++)
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