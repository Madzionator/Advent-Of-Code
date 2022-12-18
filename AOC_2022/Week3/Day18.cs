using Advent._2022.Day;

namespace Advent._2022.Week3;

class Day18 : IDay
{
    record Cube(int X, int Y, int Z);

    enum CubeType
    {
        Unknown = 0,
        Lava = 1,
        Water = 2,
        Air = 3,
    }

    public void Execute()
    {
        var input = File.ReadAllLines(@"Week3\input18.txt")
            .Select(x => x.Split(','))
            .Select(x => new Cube(int.Parse(x[0]), int.Parse(x[1]), int.Parse(x[2])));

        var grid = new CubeType[24, 24, 24];
        foreach (var cube in input)
            grid[cube.X + 1, cube.Y + 1, cube.Z + 1] = CubeType.Lava; //move vec[1,1,1]

        Console.WriteLine($"A: {Task(grid)}");

        FloodFillGrid(grid);
        Console.WriteLine($"B: {Task(grid)}");
    }

    private int Task(CubeType[,,] grid)
    {
        var counter = 0;

        for (var x = 1; x < 23; x++)
        for (var y = 1; y < 23; y++)
        for (var z = 1; z < 23; z++)
            if (grid[x, y, z] == CubeType.Lava)
                counter += CountExposedSides(x, y, z);

        return counter;

        int CountExposedSides(int x, int y, int z)
        {
            var exposed = 0;

            if (grid[x, y, z - 1] == CubeType.Water || grid[x, y, z - 1] == CubeType.Unknown)
                exposed++;
            if (grid[x, y, z + 1] == CubeType.Water || grid[x, y, z + 1] == CubeType.Unknown)
                exposed++;
            if (grid[x, y - 1, z] == CubeType.Water || grid[x, y - 1, z] == CubeType.Unknown)
                exposed++;
            if (grid[x, y + 1, z] == CubeType.Water || grid[x, y + 1, z] == CubeType.Unknown)
                exposed++;
            if (grid[x - 1, y, z] == CubeType.Water || grid[x - 1, y, z] == CubeType.Unknown)
                exposed++;
            if (grid[x + 1, y, z] == CubeType.Water || grid[x + 1, y, z] == CubeType.Unknown)
                exposed++;

            return exposed;
        }
    }

    void FloodFillGrid(CubeType[,,] grid)
    {
        for (var i = 0; i < 24; i++)      //make water contours
            for (var j = 0; j < 24; j++)
            {
                grid[i, j, 0] = CubeType.Water;
                grid[i, j, 23] = CubeType.Water;

                grid[0, i, j] = CubeType.Water;
                grid[23, i, j] = CubeType.Water;

                grid[j, 0, i] = CubeType.Water;
                grid[j, 23, i] = CubeType.Water;
            }

        bool sthChanged = true;
        CubeType type;

        while (sthChanged)              //flood fill
        {
            sthChanged = false;
            for (var z = 1; z < 23; z++)
            for (int x = 1; x < 23; x++)
            for (int y = 1; y < 23; y++)
            {
                if (grid[x, y, z] != CubeType.Lava)
                {
                    if (IsInContactWithWater(x, y, z))
                        type = CubeType.Water;
                    else
                        type = CubeType.Air;

                    if (grid[x, y, z] != type)
                    {
                        grid[x, y, z] = type;
                        sthChanged = true;
                    }
                }
            }
        }

        bool IsInContactWithWater(int x, int y, int z)
        {
            if (grid[x, y, z - 1] == CubeType.Water
                || grid[x, y, z + 1] == CubeType.Water
                || grid[x, y - 1, z] == CubeType.Water
                || grid[x, y + 1, z] == CubeType.Water
                || grid[x - 1, y, z] == CubeType.Water
                || grid[x + 1, y, z] == CubeType.Water)
                return true;

            return false;
        }
    }
}