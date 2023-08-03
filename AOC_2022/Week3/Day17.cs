using Advent._2022.Day;

namespace Advent._2022.Week3;

class Day17 : IDay
{
    public void Execute()
    {
        var areas = File.ReadAllText(@"Week3\input17.txt").ToCharArray();
        
        Console.WriteLine($"A: {TaskA(areas)}");
        Console.WriteLine($"B: {TaskB(areas)}");
    }

    private int TaskA(char[] input)
    {
        var maxHaight = -1;
        var tetris = new HashSet<(int, int)>();
        int inputIdx = 0;

        for (var i = 0; i < 2022; i++)
        {
            var shape = new Shape(2, maxHaight + 4, i % 5);

            while (true)
            {
                if (inputIdx >= input.Length)
                    inputIdx %= input.Length;

                shape.TryGoSideways(tetris, input[inputIdx]);
                inputIdx++;
                
                if (TryStop(shape))
                    break;

                shape.GoDown(tetris);
            }
        }

        return maxHaight + 1;

        bool TryStop(Shape shape)
        {
            if (!shape.isStopped(tetris)) 
                return false;

            foreach (var p in shape.Points)
            {
                tetris.Add((p.X, p.Y));
            }

            maxHaight = tetris.Max(x => x.Item2);

            return true;
        }
    }

    private long TaskB(char[] input)
    {
        var maxHeight = -1;
        var tetris = new HashSet<(int X, int Y)>();
        int inputIdx = 0;

        string bigPattern = "";
        string smallPattern = "";

        long needed = 1000000000000L;
        long figures = 0;

        for (long i = 0; figures<needed; figures++)
        {
            var shape = new Shape(2, maxHeight + 4, (int)(figures % 5));

            while (true)
            {
                if (inputIdx >= input.Length)
                    inputIdx %= input.Length;

                shape.TryGoSideways(tetris, input[inputIdx]);
                inputIdx++;

                if (TryStop(shape))
                    break;

                shape.GoDown(tetris);
            }

            if (figures == 20005)
            {
                var pattern = bigPattern[1000..1500];
                var searchingarea = bigPattern[1500..];

                var idx = searchingarea.IndexOf(pattern);
            }


        }

        return maxHeight + 1;

        bool TryStop(Shape shape)
        {
            if (!shape.isStopped(tetris))
                return false;

            foreach (var p in shape.Points)
            {
                tetris.Add((p.X, p.Y));
            }

            maxHeight = tetris.Max(x => x.X);
            bigPattern += shape.DeltaX;

            return true;
        }
    }


    private long TaskBOld(char[] input)
    {
        var maxHeight = -1;
        var tetris = new HashSet<(int, int)>();
        int inputIdx = 0;

        string pattern = "";
        List<string> patterns = new();

        long figures = 0;
        long needed = 1000000000000L;

        string bestPattern = null;
        long bestPattern1Index = -1;
        long bestPattern2Index = -1;
        long bestPattern1Height = -1;
        long bestPattern2Height = -1;
        long additionalHeight = 0;


        for (int i = 0; figures < needed; i++, figures++)
        {
            var shape = new Shape(2, maxHeight + 4, i % 5);

            while (true)
            {
                if (inputIdx >= input.Length)
                    inputIdx %= input.Length;

                shape.TryGoSideways(tetris, input[inputIdx]);
                inputIdx++;

                if (TryStop(shape))
                {
                    pattern += string.Join("" ,shape.Points.Select(c => c.X));
                    break;
                }

                shape.GoDown(tetris);
            }

            if (i % 5 == 4)
            {
                var p = pattern;
                patterns.Add(pattern);
                pattern = "";

                if (i > 6500 && bestPattern == null)
                {
                    var pat = patterns.GroupBy(x => x).MaxBy(x => x.Count()).Key;
                    if (patterns.Count > 2 && p == pat)
                    {
                        bestPattern = pat;
                        bestPattern1Index = i;
                        bestPattern1Height = maxHeight;
                    }
                }

                if (bestPattern == p && i != bestPattern1Index)
                {
                    bestPattern2Index = i;
                    bestPattern2Height = maxHeight;

                    var mod = bestPattern2Index - bestPattern1Index;
                    var heightDifference = bestPattern2Height - bestPattern1Height;

                    var sequencesLeft = (needed - i) / mod;
                    figures += sequencesLeft * mod;
                    additionalHeight = heightDifference * sequencesLeft;
                }
            }
        }

        return maxHeight + 1 + additionalHeight;

        bool TryStop(Shape shape)
        {
            if (!shape.isStopped(tetris))
                return false;

            foreach (var p in shape.Points)
            {
                tetris.Add((p.X, p.Y));
            }

            maxHeight = tetris.Max(x => x.Item2);

            return true;
        }
    }

    private class Shape
    {
        public (int X, int Y)[] Points { get; } = new (int, int)[5];
        public int DeltaX { get; set; }

        public Shape(int x, int y, int figureNum)
        {
            // 0)           1)  .1.      2)  ..4     3)  3     4)  12
            //     0123         024          ..3         2         03
            //                  .3.          012         1
            //                                           0

            switch (figureNum)
            {
                case 0:
                    for (var i = 0; i < 4; i++)
                        Points[i] = (x + i, y);
                    Points[4] = Points[3];
                    break;
                case 1:
                    Points[0] = (x, y + 1);
                    Points[1] = (x+1, y + 2);
                    Points[2] = (x+1, y + 1);
                    Points[3] = (x+1, y);
                    Points[4] = (x+2, y+1);
                    break;
                case 2:
                    Points[0] = (x, y);
                    Points[1] = (x+1, y);
                    Points[2] = (x+2, y);
                    Points[3] = (x+2, y+1);
                    Points[4] = (x+2, y+2);
                    break;
                case 3:
                    for (var i = 0; i < 4; i++)
                        Points[i] = (x, y+i);
                    Points[4] = Points[3];
                    break;
                case 4:
                    Points[0] = (x, y);
                    Points[1] = (x, y + 1);
                    Points[2] = (x + 1, y + 1);
                    Points[3] = (x + 1, y);
                    Points[4] = Points[3];
                    break;
            }

        }

        public void TryGoSideways(HashSet<(int, int)> tetris, char direction)
        {
            if (direction == '<' && Points[0].X == 0) //left
            {
                return;
            }

            if (direction == '>' && Points[4].X == 6) //right
            {
                return;
            }

            int dir = direction == '<' ? -1 : 1;

            for (var i = 0; i < 5; i++)
            {
                if (tetris.Contains((Points[i].X + dir, Points[i].Y)))
                    return;
            }

            for (var i = 0; i < 5; i++)
            {
                Points[i] = (Points[i].X + dir, Points[i].Y);
            }

            DeltaX += dir;
        }

        public void GoDown(HashSet<(int, int)> tetris)
        {
            for (var i = 0; i < 5; i++)
            {
                Points[i] = (Points[i].X, Points[i].Y - 1);
            }
        }

        public bool isStopped(HashSet<(int, int)> tetris)
        {
            for (var i = 0; i < 5; i++)
            {
                if (Points[i].Y == 0 || tetris.Contains((Points[i].X, Points[i].Y - 1)))
                    return true;
            }

            return false;
        }
    }
}