namespace AdventOfCode2024.Week1;

class Day4 : Day
{
    private int Max;
    private char[,] Input;

    public override (object resultA, object resultB) Execute()
    {
        Input = InputLines.ToCharMatrix();
        Max = Input.GetLength(0);

        return (TaskA(), TaskB());
    }

    int TaskA()
    {
        (int dY, int dX)[] directions = [(0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1)];
        var count = 0;

        for (var y = 0; y < Max; y++)
        for (var x = 0; x < Max; x++)
        {
            if (Input[y,x] != 'X')
                continue;

            foreach (var dir in directions)
            {
                var neighbour = GetValidNeighbour(y, x, dir.dY, dir.dX);
                if(neighbour is null || Input[neighbour.Value.y, neighbour.Value.x] != 'M')
                    continue;

                neighbour = GetValidNeighbour(neighbour.Value.y, neighbour.Value.x, dir.dY, dir.dX);
                if (neighbour is null || Input[neighbour.Value.y, neighbour.Value.x] != 'A')
                    continue;

                neighbour = GetValidNeighbour(neighbour.Value.y, neighbour.Value.x, dir.dY, dir.dX);
                if (neighbour is null || Input[neighbour.Value.y, neighbour.Value.x] != 'S')
                    continue;

                count++;
            }
        }

        return count;
    }

    int TaskB()
    {
        var count = 0;

        for (var y = 0; y < Max; y++)
        for (var x = 0; x < Max; x++)
        {
            if (Input[y, x] != 'A')
                continue;

            var n1 = GetValidNeighbour(y, x, 1, 1);;
            var n2 = GetValidNeighbour(y, x, -1, -1);
            var n3 = GetValidNeighbour(y, x, 1, -1);
            var n4 = GetValidNeighbour(y, x, -1, 1);

            if (n1 is null || n2 is null || n3 is null || n4 is null)
                continue;

            var diagonal1 = PositionToValue(n1.Value.y, n1.Value.x) + PositionToValue(n2.Value.y, n2.Value.x);
            var diagonal2 = PositionToValue(n3.Value.y, n3.Value.x) + PositionToValue(n4.Value.y, n4.Value.x);

            if (diagonal1 == 3 && diagonal2 == 3)
                count++;
        }

        return count;
    }

    (int y, int x)? GetValidNeighbour(int y, int x, int dy, int dx)
    {
        var yi = y + dy;
        var xi = x + dx;

        if (yi >= Max || yi < 0 || xi >= Max || xi < 0)
            return null;

        return (yi, xi);
    }

    int PositionToValue(int y, int x)
        => Input[y, x] switch
        {
            'M' => 1,
            'S' => 2,
            _ => 0
        };
}