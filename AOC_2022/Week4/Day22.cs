using Advent._2022.Day;

namespace Advent._2022.Week4;

class Day22 : IDay
{
    record CubeSide(int MinX, int MinY)
    {
        public HashSet<(int X, int Y)> Walls { get; } = new();
    }

    public void Execute()
    {
        #region read and process data

        var input = File.ReadAllText(@"Week4\input22.txt")
            .Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries));

        var map = input.First();
        var sideWidth = map[0].Length / 3;
        var isExample = sideWidth == 4;

        var instructions = input.Last()[0].Replace("R", " R;").Replace("L", " L;")
            .Split(' ')
            .Select(x =>
            {
                var y = x.Split(';');
                return y.Length < 2 ? ('c', int.Parse(y[0])) : (y[0][0], int.Parse(y[1]));
            })
            .ToArray();

        var cubeSides = new List<CubeSide>();

        for (var yj = 0; yj < map.Length / sideWidth; yj++)
        for (var xi = 0; xi < 4; xi++)
        {
            if (map[yj * sideWidth].Length <= xi * sideWidth || map[yj * sideWidth][xi * sideWidth] == ' ')
                continue;

            var cubeS = new CubeSide(xi * sideWidth + 1, yj * sideWidth + 1);
            cubeSides.Add(cubeS);

            for (var x = 0; x < sideWidth; x++)
            for (var y = 0; y < sideWidth; y++)
            {
                if (map[yj * sideWidth + y][xi * sideWidth + x] == '#')
                {
                    cubeS.Walls.Add((x, y));
                }
            }
        }

        #endregion

        Console.WriteLine($"A: {Task(cubeSides, instructions, sideWidth, isExample)}");
        Console.WriteLine($"B: {Task(cubeSides, instructions, sideWidth, isExample, true)}");
    }

    private int Task(List<CubeSide> cubeSides, (char rotate, int steps)[] instructions, int sideWidth, bool isExample, bool isTaskB = false)
    {
        var currentSide = 0;
        var curPosX = 0;
        var curPosY = 0;
        var dirX = 1;
        var dirY = 0;

        foreach (var instruction in instructions)
        {
            (dirX, dirY) = Rotate(dirX, dirY, instruction.rotate);
            for (var step = 0; step < instruction.steps; step++)
            {
                var newPos = NewPosition(dirX, dirY, curPosX, curPosY, currentSide, sideWidth, isExample, isTaskB);

                if (cubeSides[newPos.Item1].Walls.Contains((newPos.x, newPos.y)))
                    break;

                (currentSide, curPosX, curPosY, dirX, dirY) = newPos;
            }
        }

        var facing = (dirX, dirY) == (1, 0) ? 0 : (dirX, dirY) == (0, 1) ? 1 : (dirX, dirY) == (-1, 0) ? 2 : 3;

        return (cubeSides[currentSide].MinY + curPosY)*1000 + (cubeSides[currentSide].MinX + curPosX)*4 + facing;
    }

    (int side, int x, int y, int dirx, int diry) NewPosition(int dirX, int dirY, int curX, int curY, int curSide, int sideWidth, bool isExample, bool isTaskB)
    {
        var x = curX + dirX;
        var y = curY + dirY;

        if (x >= sideWidth) 
            return ChangeCubeSide('r', dirX, dirY, x, y, sideWidth, curSide, isExample, isTaskB);
        if (x < 0) 
            return ChangeCubeSide('l', dirX, dirY, x, y, sideWidth, curSide, isExample, isTaskB);
        if (y >= sideWidth) 
            return ChangeCubeSide('d', dirX, dirY, x, y, sideWidth, curSide, isExample, isTaskB);
        if (y < 0) 
            return ChangeCubeSide('u', dirX, dirY, x, y, sideWidth, curSide, isExample, isTaskB);

        return (curSide, x, y, dirX, dirY);
    }

    (int newDirX, int newDirY) Rotate(int dirX, int dirY, char dir)
    {
        return dir switch
        {
            'L' => (dirX, dirY) switch
            {
                (1, 0) => (0, -1), // right -> up
                (0, -1) => (-1, 0), // up -> left
                (-1, 0) => (0, 1), // left -> down
                (0, 1) => (1, 0), // down -> right
            },
            'R' => (dirX, dirY) switch
            {
                (1, 0) => (0, 1), // right -> down
                (0, 1) => (-1, 0), // down -> left
                (-1, 0) => (0, -1), // left -> up
                (0, -1) => (1, 0), // up -> right
            },
            _ => (dirX, dirY)
        };
    }

    (int newCubeSide, int newX, int newY, int newDirX, int newDirY) ChangeCubeSide(char dir, int dirX, int dirY, int x, int y, int sideWidth, int posCube, bool isExample, bool isTaskB)
    {
        switch (isExample, isTaskB)
        {
            case (true, false): //Task A - example
                return (dir, posCube) switch
                {
                    ('r', 0) => (0, 0, y, dirX, dirY),
                    ('d', 0) => (3, x, 0, dirX, dirY),
                    ('l', 0) => (0, sideWidth - 1, y, dirX, dirY),
                    ('u', 0) => (4, x, sideWidth - 1, dirX, dirY),

                    ('r', 1) => (2, 0, y, dirX, dirY),
                    ('d', 1) => (1, x, 0, dirX, dirY),
                    ('l', 1) => (3, sideWidth - 1, y, dirX, dirY),
                    ('u', 1) => (1, x, sideWidth - 1, dirX, dirY),

                    ('r', 2) => (3, 0, y, dirX, dirY),
                    ('d', 2) => (2, x, 0, dirX, dirY),
                    ('l', 2) => (1, sideWidth - 1, y, dirX, dirY),
                    ('u', 2) => (2, x, sideWidth - 1, dirX, dirY),

                    ('r', 3) => (1, 0, y, dirX, dirY),
                    ('d', 3) => (4, x, 0, dirX, dirY),
                    ('l', 3) => (2, sideWidth - 1, y, dirX, dirY),
                    ('u', 3) => (0, x, sideWidth - 1, dirX, dirY),

                    ('r', 4) => (5, 0, y, dirX, dirY),
                    ('d', 4) => (0, x, 0, dirX, dirY),
                    ('l', 4) => (5, sideWidth - 1, y, dirX, dirY),
                    ('u', 4) => (3, x, sideWidth - 1, dirX, dirY),

                    ('r', 5) => (4, 0, y, dirX, dirY),
                    ('d', 5) => (5, x, 0, dirX, dirY),
                    ('l', 5) => (4, sideWidth - 1, y, dirX, dirY),
                    ('u', 5) => (5, x, sideWidth - 1, dirX, dirY),
                };

            case (false, false): //Task A - real input
                return (dir, posCube) switch
                {
                    ('r', 0) => (1, 0, y, dirX, dirY),
                    ('d', 0) => (2, x, 0, dirX, dirY),
                    ('l', 0) => (1, sideWidth - 1, y, dirX, dirY),
                    ('u', 0) => (4, x, sideWidth - 1, dirX, dirY),

                    ('r', 1) => (0, 0, y, dirX, dirY),
                    ('d', 1) => (1, x, 0, dirX, dirY),
                    ('l', 1) => (0, sideWidth - 1, y, dirX, dirY),
                    ('u', 1) => (1, x, sideWidth - 1, dirX, dirY),

                    ('r', 2) => (2, 0, y, dirX, dirY),
                    ('d', 2) => (4, x, 0, dirX, dirY),
                    ('l', 2) => (2, sideWidth - 1, y, dirX, dirY),
                    ('u', 2) => (0, x, sideWidth - 1, dirX, dirY),

                    ('r', 3) => (4, 0, y, dirX, dirY),
                    ('d', 3) => (5, x, 0, dirX, dirY),
                    ('l', 3) => (4, sideWidth - 1, y, dirX, dirY),
                    ('u', 3) => (5, x, sideWidth - 1, dirX, dirY),

                    ('r', 4) => (3, 0, y, dirX, dirY),
                    ('d', 4) => (0, x, 0, dirX, dirY),
                    ('l', 4) => (3, sideWidth - 1, y, dirX, dirY),
                    ('u', 4) => (2, x, sideWidth - 1, dirX, dirY),

                    ('r', 5) => (5, 0, y, dirX, dirY),
                    ('d', 5) => (3, x, 0, dirX, dirY),
                    ('l', 5) => (5, sideWidth - 1, y, dirX, dirY),
                    ('u', 5) => (3, x, sideWidth - 1, dirX, dirY),
                };

            case (true, true): //Task B - example
                return (dir, posCube) switch
                {
                    ('r', 0) => (5, sideWidth - 1, sideWidth - 1 - y, -1, 0),
                    ('d', 0) => (3, x, 0, dirX, dirY),
                    ('l', 0) => (2, y, 0, 0, 1),
                    ('u', 0) => (1, sideWidth - 1 - x, 0, 0, 1),

                    ('r', 1) => (2, 0, y, dirX, dirY),
                    ('d', 1) => (4, sideWidth - 1 - x, sideWidth - 1, 0, -1),
                    ('l', 1) => (5, sideWidth - 1 - y, sideWidth - 1, 0, -1),
                    ('u', 1) => (0, sideWidth - 1 - x, 0, 0, 1),

                    ('r', 2) => (3, 0, y, dirX, dirY),
                    ('d', 2) => (4, 0, sideWidth - 1 - x, 1, 0),
                    ('l', 2) => (1, sideWidth - 1, y, dirX, dirY),
                    ('u', 2) => (0, 0, x, 1, 0),

                    ('r', 3) => (5, sideWidth - 1 - y, 0, 0, 1),
                    ('d', 3) => (4, x, 0, dirX, dirY),
                    ('l', 3) => (2, sideWidth - 1, y, dirX, dirY),
                    ('u', 3) => (0, x, sideWidth - 1, dirX, dirY),

                    ('r', 4) => (5, 0, y, dirX, dirY),
                    ('d', 4) => (1, sideWidth - 1 - x, sideWidth - 1, 0, -1),
                    ('l', 4) => (2, sideWidth - 1 - y, sideWidth - 1, 0, -1),
                    ('u', 4) => (3, x, sideWidth - 1, dirX, dirY),

                    ('r', 5) => (0, sideWidth - 1, sideWidth - 1 - y, -1, 0),
                    ('d', 5) => (1, 0, sideWidth - 1 - x, 1, 0),
                    ('l', 5) => (4, sideWidth - 1, y, dirX, dirY),
                    ('u', 5) => (3, sideWidth - 1, sideWidth - 1 - x, -1, 0),
                };

            case (false, true): //Task B - real input
                return (dir, posCube) switch
                {
                    ('r', 0) => (1, 0, y, dirX, dirY),
                    ('d', 0) => (2, x, 0, dirX, dirY),
                    ('l', 0) => (3, 0, sideWidth - 1 - y, 1, 0),
                    ('u', 0) => (5, 0, x, 1, 0),

                    ('r', 1) => (4, sideWidth - 1, sideWidth - 1 - y, -1, 0),
                    ('d', 1) => (2, sideWidth - 1, x, -1, 0),
                    ('l', 1) => (0, sideWidth - 1, y, dirX, dirY),
                    ('u', 1) => (5, x, sideWidth - 1, 0, -1),

                    ('r', 2) => (1, y, sideWidth - 1, 0, -1),
                    ('d', 2) => (4, x, 0, dirX, dirY),
                    ('l', 2) => (3, y, 0, 0, 1),
                    ('u', 2) => (0, x, sideWidth - 1, dirX, dirY),

                    ('r', 3) => (4, 0, y, dirX, dirY),
                    ('d', 3) => (5, x, 0, dirX, dirY),
                    ('l', 3) => (0, 0, sideWidth - 1 - y, 1, 0),
                    ('u', 3) => (2, 0, x, 1, 0),

                    ('r', 4) => (1, sideWidth - 1, sideWidth - 1 - y, -1, 0),
                    ('d', 4) => (5, sideWidth - 1, x, -1, 0),
                    ('l', 4) => (3, sideWidth - 1, y, dirX, dirY),
                    ('u', 4) => (2, x, sideWidth - 1, dirX, dirY),

                    ('r', 5) => (4, y, sideWidth - 1, 0, -1),
                    ('d', 5) => (1, x, 0, 0, 1),
                    ('l', 5) => (0, y, 0, 0, 1),
                    ('u', 5) => (3, x, sideWidth - 1, dirX, dirY),
                };
        }
    }
}