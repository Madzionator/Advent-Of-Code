using Advent._2022.Day;

namespace Advent._2022.Week3;

class Day15 : IDay
{
    public void Execute()
    {
        var areas = File.ReadAllLines(@"Week3\input15.txt")
            .Select(x => x.Split(' '))
            .Select(x => new Area(int.Parse(x[2][2..^1]), int.Parse(x[3][2..^1]), int.Parse(x[8][2..^1]), int.Parse(x[9][2..])))
            .ToList();

        bool isAnExample = areas.Count <= 14;

        Console.WriteLine($"A: {TaskA(areas, isAnExample)}");   //takes few sec
        Console.WriteLine($"B: {TaskB(areas, isAnExample)}");
    }

    record Area(int Sx, int Sy, int Bx, int By)
    {
        public int Manhattan { get; } = Math.Abs(Sx - Bx) + Math.Abs(Sy - By);

        public bool Contains(int x, int y)
            => Math.Abs(Sx - x) + Math.Abs(Sy - y) <= Manhattan;

        public int CommonXWidth(int x, int y)
            => (Manhattan + Sx - Math.Abs(Sy - y) - x);
    };

    private int TaskA(List<Area> areas, bool isAnExample)
    {
        var y = isAnExample ? 10 : 2000000;
        var counter = 0;

        var minX = areas.Min(x => x.Sx - x.Manhattan);
        var maxX = areas.Max(x => x.Sx + x.Manhattan);

        for (var x = minX; x <= maxX; x++)
        {
            if (areas.Any(d => (d.By == y && d.Bx == x)))
                continue;

            if (areas.Any(area => area.Contains(x, y)))
                counter++;
        }

        return counter;
    }

    private long TaskB(List<Area> areas, bool isAnExample)
    {
        var max = isAnExample ? 20 : 4000000;

        for (var y = 0; y <= max; y++)
            for (var x = 0; x <= max; x++)
            {
                var area = areas.FirstOrDefault(a => a.Contains(x, y));

                if (area is not null)
                    x += area.CommonXWidth(x, y);
                else
                    return 4000000L * x + y;
            }

        return 0;
    }
}