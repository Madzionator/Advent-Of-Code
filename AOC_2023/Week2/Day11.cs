using Advent._2023.Day;
namespace AdventOfCode2023.Week2;

class Day11 : IDay
{
    record Galaxy
    {
        public int Y { get; set; }
        public int X { get; set; }
    }
    
    private List<Galaxy> _galaxies;

    public void Execute()
    {
        ReadInput();

        Console.WriteLine($"A: {Task(1)}");
        Console.WriteLine($"B: {Task( 1000000 - 1)}");
    }

    long Task(int expandLen) =>
        ExpandUniverse(expandLen)
            .Combinations(2)
            .Select(pair =>
            {
                var l = pair.ToArray();
                return Math.Abs(l[0].X - l[1].X) + Math.Abs(l[0].Y - l[1].Y);

            }).Sum(distance => (long)distance);

    void ReadInput()
    {
        _galaxies = File.ReadAllLines(@"Week2\input11.txt")
            .SelectMany((line, y) =>
            {
                var list = new List<Galaxy>();
                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#')
                    {
                        list.Add(new Galaxy{ Y = y, X = x });
                    }
                }

                return list;
            }).ToList();
    }

    Galaxy[] ExpandUniverse(int d)
    {
        var galaxies = _galaxies.Select(g => new Galaxy{ X = g.X, Y = g.Y }).ToArray();

        var maxX = galaxies.Max(g => g.X);
        for (var x = maxX - 1; x >= 0; x--)
            if (galaxies.All(g => g.X != x))
                foreach (var g2 in galaxies.Where(g2 => g2.X > x))
                    g2.X += d;

        var maxY = galaxies.Max(g => g.Y);
        for (var y = maxY - 1; y >= 0; y--)
            if (galaxies.All(g => g.Y != y))
                foreach (var g2 in galaxies.Where(g2 => g2.Y > y))
                    g2.Y += d;

        return galaxies;
    }
}