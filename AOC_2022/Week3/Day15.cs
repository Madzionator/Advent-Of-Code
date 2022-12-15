using Advent._2022.Day;

namespace Advent._2022.Week3;

class Day15 : IDay
{
    public void Execute()
    {
        var input = File.ReadAllLines(@"Week3\input15.txt")
            .Select(x => x.Split(' '))
            .Select(x => new Area(int.Parse(x[2][2..^1]), int.Parse(x[3][2..^1]), int.Parse(x[8][2..^1]), int.Parse(x[9][2..])))
            .ToList();

        bool isAnExample = input.Count <= 14;

        Console.WriteLine($"A: {TaskA(input, isAnExample)}");   //takes few sec
        Console.WriteLine($"B: {TaskB(input, isAnExample)}");
    }

    record Area(int Sx, int Sy, int Bx, int By)
    {
        public int Manhattan { get; } = Math.Abs(Sx - Bx) + Math.Abs(Sy - By);

        public bool Contains(int x, int y)
            => Math.Abs(Sx - x) + Math.Abs(Sy - y) <= Manhattan;

        public (int width, bool contained) CommonXWidth(int x, int y)
            => !Contains(x, y) ? (0, false) : (Manhattan + Sx - Math.Abs(Sy - y) - x, true);
    };

    private int TaskA(List<Area> input, bool isAnExample)
    {
        var y = isAnExample ? 10 : 2000000;
        var counter = 0;

        var minXArea = input.MinBy(x => x.Sx - x.Manhattan);
        var maxXArea = input.MaxBy(x => x.Sx + x.Manhattan);
        var minX = minXArea.Sx - minXArea.Manhattan;
        var maxX = maxXArea.Sx + maxXArea.Manhattan;

        for (var x = minX; x <= maxX; x++)
        {
            if (input.Any(d => (d.By == y && d.Bx == x) || (d.Sy == y && d.Sx == x)))
                continue;

            if (input.Any(area => area.Contains(x, y)))
            {
                counter++;
            }
        }

        return counter;
    }

    private long TaskB(List<Area> input, bool isAnExample)
    {
        var max = isAnExample ? 20 : 4000000;

        for (var y = 0; y <= max; y++)
        {
            for (var x = 0; x <= max; x++)
            {
                var any = false;
                foreach (var a in input.Select(area => area.CommonXWidth(x, y)).Where(d => d.contained))
                {
                    x += a.width;
                    any = true;
                    break;
                }

                if (!any)
                    return 4000000L * x + y;
            }
        }

        return 0;
    }
}