using Advent._2023.Day;

namespace AdventOfCode2023.Week4;

class Day22 : IDay
{
    record Brick(int X1, int Y1, int Z1, int X2, int Y2, int Z2, Rotation Rotation)
    {
        public HashSet<Brick> StandOn = new ();
        public HashSet<Brick> Support = new ();
        public HashSet<Brick> Collapse = new ();
    }

    enum Rotation
    {
        xLong,
        yLong,
        zLong
    }

    public void Execute()
    {
        var bricks = File.ReadAllLines(@"Week4\input22.txt").Select(line =>
        {
            var x = line.Split(new[] { ',', '~' }).Select(int.Parse).ToArray();
            var rot = x[0] != x[3] ? Rotation.xLong : x[1] != x[4] ? Rotation.yLong : Rotation.zLong;

            return new Brick(x[0], x[1], x[2], x[3], x[4], x[5], rot);

        }).OrderBy(b => b.Z1).ToArray();

        Console.WriteLine($"A: {TaskA(bricks)}");
        Console.WriteLine($"B: {TaskB(bricks)}");
    }

    int TaskA(Brick[] bricks)
    {
        var maxH = 0;
        var takenSpace = new Dictionary<(int x, int y, int z), Brick>();

        foreach (var brick in bricks)
        {
            var minH = maxH + 1;
            for (var z = maxH + 1; z>0; z--) //find the lowest free space
            {
                var isSpace = true;
                switch (brick.Rotation)
                {
                    case Rotation.xLong:
                        for(var i = brick.X1; i <= brick.X2; i++)
                            if (takenSpace.ContainsKey((i, brick.Y1, z)))
                                isSpace = false;
                        break;

                    case Rotation.yLong:
                        for (var i = brick.Y1; i <= brick.Y2; i++)
                            if (takenSpace.ContainsKey((brick.X1, i, z)))
                                isSpace = false;
                        break;

                    case Rotation.zLong:
                        if (takenSpace.ContainsKey((brick.X1, brick.Y1, z)))
                            isSpace = false;
                        break;
                }

                if (isSpace)
                    minH = z;
                else
                    break;
            }

            switch (brick.Rotation) // lay bricks in the place found
            {
                case Rotation.xLong:
                    for (var i = brick.X1; i <= brick.X2; i++)
                    {
                        takenSpace.Add((i, brick.Y1, minH), brick);
                        if (takenSpace.ContainsKey((i, brick.Y1, minH - 1)))
                        {
                            var supporter = takenSpace[(i, brick.Y1, minH - 1)];
                            brick.StandOn.Add(supporter);
                            supporter.Support.Add(brick);
                        }
                    }
                    minH++;
                    break;

                case Rotation.yLong:
                    for (var i = brick.Y1; i <= brick.Y2; i++)
                    {
                        takenSpace.Add((brick.X1, i, minH), brick);
                        if (takenSpace.ContainsKey((brick.X1, i, minH - 1)))
                        {
                            var supporter = takenSpace[(brick.X1, i, minH - 1)];
                            brick.StandOn.Add(supporter);
                            supporter.Support.Add(brick);
                        }
                    }
                    minH++;
                    break;

                case Rotation.zLong:
                    var x = minH;

                    if (takenSpace.ContainsKey((brick.X1, brick.Y1, minH - 1)))
                    {
                        var supporterZ = takenSpace[(brick.X1, brick.Y1, minH - 1)];
                        brick.StandOn.Add(supporterZ);
                        supporterZ.Support.Add(brick);
                    }

                    for (var i = x; i <= x + brick.Z2 - brick.Z1; i++)
                    {
                        takenSpace.Add((brick.X1, brick.Y1, i), brick);
                        minH++;
                    }
                    break;
            }

            if(minH > maxH)
                maxH = minH;
        }

        var toDisintegrate = 0;

        foreach (var brick in bricks) //find which can be disintegrated
        {
            if (brick.Support.Count == 0)
            {
                toDisintegrate++;
                continue;
            }

            var standsOnOnlyOne = false;
            foreach (var brickOnTop in brick.Support)
            {
                if (brickOnTop.StandOn.Count < 2)
                    standsOnOnlyOne = true;
            }

            if(!standsOnOnlyOne)
                toDisintegrate++;
        }

        return toDisintegrate;
    }

    int TaskB(Brick[] bricks)
    {
        foreach (var actualBrick in bricks)
        {
            var addedLast = new HashSet<Brick>();

            // add to Collapse the bricks directly above, which only stand on actual brick
            foreach (var supportedBrick in actualBrick.Support.Where(b => b.StandOn.Count < 2))
            {
                addedLast.Add(supportedBrick);
                actualBrick.Collapse.Add(supportedBrick);
            }

            while (addedLast.Count > 0)
            {
                var newOnes = new HashSet<Brick>();

                foreach (var toBeCollapsed in addedLast)
                {
                    //add to Collapse the bricks that stand only on elements that collapse with actual brick
                    foreach (var brick in toBeCollapsed.Support)
                    {
                        var k = brick.StandOn.Count;
                        foreach (var lowerBrick in brick.StandOn)
                        {
                            if (actualBrick == lowerBrick || actualBrick.Collapse.Contains(lowerBrick))
                                k--;
                        }

                        if (k == 0)
                        {
                            newOnes.Add(brick);
                            actualBrick.Collapse.Add(brick);
                        }
                    }
                }

                addedLast = newOnes;
            }
        }

        return bricks.Sum(b => b.Collapse.Count);
    }
}