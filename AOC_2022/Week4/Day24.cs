using Advent._2022.Day;

namespace Advent._2022.Week4;

class Day24 : IDay
{
    private static int LimitX { get; set; }
    private static int LimitY { get; set; }

    record Blizzard(int DirX, int DirY, int X, int Y)
    {
        public int X { get; private set; } = X;
        public int Y { get; private set; } = Y;

        public void Move()
        {
            X += DirX;
            Y += DirY;
            if (X >= LimitX) X = 1;
            else if (X <= 0) X = LimitX - 1;
            else if (Y >= LimitY) Y = 1;
            else if (Y <= 0) Y = LimitY - 1;
        }
    }

    public void Execute()
    {
        var input = File.ReadAllLines(@"Week4\input24.txt");

        var blizzards = new Dictionary<(int x, int y), List<Blizzard>>();
        var blizzardsCopy = new Dictionary<(int x, int y), List<Blizzard>>();

        for(var y = 0; y < input.Length; y++)
        for(var x = 0; x < input[0].Length; x++)
            if (input[y][x] != '#' && input[y][x] != '.')
            {
                var dir = (input[y][x]) switch
                {
                    '>' => (1, 0),
                    '<' => (-1, 0),
                    '^' => (0, -1),
                    'v' => (0, 1),
                    _ => (0, 0)
                };

                blizzards.Add((x, y), new List<Blizzard>{new (dir.Item1, dir.Item2, x, y)});
                blizzardsCopy.Add((x, y), new List<Blizzard>{new (dir.Item1, dir.Item2, x, y)});
            }

        LimitX = input[0].Length - 1;
        LimitY = input.Length - 1;

        var targetA = new[] { (LimitX - 1, LimitY) };
        var targetB = new[] { (LimitX - 1, LimitY), (1, 0), (LimitX - 1, LimitY) };

        Console.WriteLine($"A: {Task(blizzardsCopy, targetA)}");
        Console.WriteLine($"B: {Task(blizzards, targetB)}");
    }
    
    private long Task(Dictionary<(int x, int y), List<Blizzard>> input, (int, int)[] targets)
    {
        var targetIdx = 0;

        var blizzards = input;
        var myPositions = new HashSet<(int x, int y)> { (1, 0) };

        for (var minute = 1; ; minute++)
        {
            var blizzardsCopy = new Dictionary<(int x, int y), List<Blizzard>>();
            foreach (var blizz in blizzards.SelectMany(loc => loc.Value))
            {
                blizz.Move();

                if (blizzardsCopy.ContainsKey((blizz.X, blizz.Y)))
                    blizzardsCopy[(blizz.X, blizz.Y)].Add(blizz);
                else
                    blizzardsCopy.Add((blizz.X, blizz.Y), new List<Blizzard> { blizz });
            }

            blizzards = blizzardsCopy;

            var myPositionsCopy = new HashSet<(int x, int y)> { };
            foreach (var loc in myPositions)
            {
                var isTargetAchieved = false;
                foreach (var pos in GetNearestPos(loc).Where(x => !blizzards.ContainsKey(x)))
                {
                    if (pos == (targets[targetIdx]))
                    {
                        if (targetIdx == targets.Length - 1)
                            return minute;

                        myPositionsCopy = new HashSet<(int x, int y)> {pos};
                        targetIdx++;
                        isTargetAchieved = true;
                        break;
                    }

                    if (IsValid(pos))
                        myPositionsCopy.Add(pos);
                }

                if (isTargetAchieved) 
                    break;
            }

            myPositions = myPositionsCopy;
        }
    }

    IEnumerable<(int, int)> GetNearestPos((int x, int y)pos)
    {
        yield return (pos.x, pos.y);
        yield return (pos.x, pos.y - 1);
        yield return (pos.x, pos.y + 1);
        yield return (pos.x - 1, pos.y);
        yield return (pos.x + 1, pos.y);
    }

    bool IsValid((int x, int y) pos) =>
        (pos.x > 0 && pos.x < LimitX && pos.y > 0 && pos.y < LimitY)
        || (pos.x == 1 && pos.y == 0) || (pos.x == LimitX - 1 && pos.y == LimitY);
}