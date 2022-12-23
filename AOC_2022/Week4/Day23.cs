using Advent._2022.Day;

namespace Advent._2022.Week4;

class Day23 : IDay
{
    enum Direction
    {
        North = 0,
        South = 1,
        West = 2,
        East = 3
    }

    record Elf(int X, int Y)
    {
        public int X { get; set; } = X;
        public int Y { get; set; } = Y;
    }

    public void Execute()
    {
        var elves = new List<Elf>();
        var elvesCopy = new List<Elf>();

        var input = File.ReadAllLines(@"Week4\input23.txt");
        for(var y = 0; y < input.Length; y++)
        for (var x = 0; x < input[0].Length; x++)
        {
            if (input[y][x] == '#')
            {
                elves.Add(new Elf(x, y));
                elvesCopy.Add(new Elf(x, y));
            }
        }
        
        Console.WriteLine($"A: {Task(elves, 10)}");
        Console.WriteLine($"B: {Task(elvesCopy, 2000)}"); //takes about 10 min, to optymalize some day :pepe:
    }
    
    private int Task(List<Elf> elves, int rounds)
    {
        for (int round = 0; round < rounds ; round++)
        {
            //first half of round
            var propositions = new Dictionary<(int x, int y), List<Elf>>();

            foreach (var elf in elves)
            {
                if(!IsAnyNeighbor(elf.X, elf.Y, elves))
                    continue;

                for (var d = 0; d < 4; d++)
                {
                    var dir = (Direction)((d + round) % 4);
                    if (IsDirAvailability(elf.X, elf.Y, dir, elves))
                    {
                        var newPos = MoveInDirection(elf.X, elf.Y, dir);

                        if (propositions.ContainsKey(newPos))
                            propositions[newPos].Add(elf);
                        else
                            propositions.Add(newPos, new List<Elf>{elf});

                        break;
                    }
                }
            }

            // second half of round
            var toMove = propositions.Where(prop => prop.Value.Count == 1);
            if (!toMove.Any())
                return round + 1;

            foreach (var prop in toMove)
            {
                prop.Value[0].X = prop.Key.x;
                prop.Value[0].Y = prop.Key.y;
            }
        }

        var minX = elves.Min(e => e.X);
        var maxX = elves.Max(e => e.X);
        var minY = elves.Min(e => e.Y);
        var maxY = elves.Max(e => e.Y);

        return (maxX - minX + 1) * (maxY - minY + 1) - elves.Count;
    }

    private static (int x, int y) MoveInDirection(int x, int y, Direction dir) =>
        dir switch
        {
            Direction.North => (x, y - 1),
            Direction.South => (x, y + 1),
            Direction.West => (x - 1, y),
            Direction.East => (x + 1, y),
            _ => (x, y)
        };

    private static bool IsAnyNeighbor(int x, int y, List<Elf> elves)
    {
        for(var ix = -1; ix <= 1; ix++)
        for (var iy = -1; iy <= 1; iy++)
        {
            if(ix == 0 && iy == 0)
                continue;

            if(elves.Any(e => e.Y == y + iy && e.X == x + ix))
                return true;
        }

        return false;
    }

    private static bool IsDirAvailability(int x, int y, Direction dir, List<Elf> elves) =>
        dir switch
        {
            Direction.North => !elves.Any(p => p.Y == y - 1 && (p.X == x-1 || p.X == x || p.X == x+1)),
            Direction.South => !elves.Any(p => p.Y == y + 1 && (p.X == x-1 || p.X == x || p.X == x+1)),
            Direction.West => !elves.Any(p => p.X == x - 1 && (p.Y == y-1 || p.Y == y || p.Y == y+1)),
            Direction.East => !elves.Any(p => p.X == x + 1 && (p.Y == y-1 || p.Y == y || p.Y == y+1)),
            _ => false
        };
}