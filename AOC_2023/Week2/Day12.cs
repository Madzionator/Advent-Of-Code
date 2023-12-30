using Advent._2023.Day;

namespace AdventOfCode2023.Week2;

class Day12 : IDay
{
    record HotSpring(string Nonogram, int[] Lengths);
    Dictionary<string, long> _cache = new ();

    public void Execute()
    {
        var inputA = File.ReadAllLines(@"Week2\input12.txt").Select(line =>
        {
            var x = line.Split(' ');
            return new HotSpring(x[0], x[1].Split(',').Select(int.Parse).ToArray());

        }).ToArray();

        var inputB = ExtendSpringForPartB(inputA);

        Console.WriteLine($"A: {Task(inputA)}");
        Console.WriteLine($"B: {Task(inputB)}");
    }

    long Task(HotSpring[] springs) => springs
        .Select(spring => CalculateAndCacheResult(spring.Nonogram, spring.Lengths))
        .Sum();

    HotSpring[] ExtendSpringForPartB(HotSpring[] input) =>
        (from hotSpring in input 
            let expandedNonogram = string.Join("?", Enumerable.Repeat(new string(hotSpring.Nonogram), 5)) 
            let expandedLengths = Enumerable.Repeat(hotSpring.Lengths, 5).SelectMany(x => x).ToArray() 
            select new HotSpring(expandedNonogram, expandedLengths)).ToArray();

    // --- inspired by u/yfilipov on reddit ---
    long CalculateAndCacheResult(string nonogram, int[] lengths)
    {
        var cacheKey = $"{nonogram},{string.Join(',', lengths)}";

        if (_cache.TryGetValue(cacheKey, out var res))
            return res;

        res = CountArrangements(nonogram, lengths);
        _cache[cacheKey] = res;

        return res;
    }

    long CountArrangements(string nonogram, int[] lengths)
    {
        while (true)
        {
            if (lengths.Length == 0)
                return nonogram.Contains('#') ? 0 : 1;

            if (string.IsNullOrEmpty(nonogram))
                return 0;

            if (nonogram.StartsWith('.'))
            {
                nonogram = nonogram.Trim('.');
                continue;
            }

            if (nonogram.StartsWith('?'))
            {
                return CalculateAndCacheResult("." + nonogram[1..], lengths) +
                    CalculateAndCacheResult("#" + nonogram[1..], lengths);
            }

            // starts with '#' :

            if (lengths.Length == 0)
                return 0;

            if (nonogram.Length < lengths[0])
                return 0;

            if (nonogram[..lengths[0]].Contains('.'))
                return 0;

            if (lengths.Length > 1)
            {
                if (nonogram.Length < lengths[0] + 1 || nonogram[lengths[0]] == '#')
                {
                    return 0;
                }

                nonogram = nonogram[(lengths[0] + 1)..];
                lengths = lengths[1..];
                continue;
            }

            nonogram = nonogram[lengths[0]..];
            lengths = lengths[1..];
        }
    }
}