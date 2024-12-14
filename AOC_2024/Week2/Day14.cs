using System.Diagnostics;
using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Week2;

internal class Day14 : Day
{
    [DebuggerDisplay("Y: {Position.Y}, X: {Position.X}, dY: {Velocity.Y}, dX: {Velocity.X}")]
    private record Robot(Vector2 Velocity)
    {
        public Vector2 Position { get; set; }
    }

    private int MaxX;
    private int MaxY;

    private Robot[] _robots;

    public override (object resultA, object resultB) Execute()
    {
        _robots = InputLines.Select(line =>
        {
            var split = line.Split(['=', ' ', ',']);
            var p = new Vector2(int.Parse(split[2]), int.Parse(split[1]));
            var v = new Vector2(int.Parse(split[5]), int.Parse(split[4]));

            return new Robot(v) { Position = p };

        }).ToArray();

        MaxX = _robots.Length > 100 ? 101 : 11;
        MaxY = _robots.Length > 100 ? 103 : 7;

        return (TaskA(), TaskB());
    }

    public int TaskA()
    {
        var newPositions = _robots.Select(Move100s).ToList();

        var upLeft = newPositions.Count(p => p.X < MaxX / 2 && p.Y < MaxY / 2);
        var upRight = newPositions.Count(p => p.X > MaxX / 2 && p.Y < MaxY / 2);
        var downLeft = newPositions.Count(p => p.X < MaxX / 2 && p.Y > MaxY / 2);
        var downRight = newPositions.Count(p => p.X > MaxX / 2 && p.Y > MaxY / 2);

        return upLeft * upRight * downLeft * downRight;
    }

    private Vector2 Move100s(Robot robot)
    {
        var p = robot.Position + robot.Velocity * 100;

        var newY = (p.Y % MaxY + MaxY) % MaxY;
        var newX = (p.X % MaxX + MaxX) % MaxX;

        return (newY, newX);
    }

    public int TaskB()
    {
        for (var s = 1; s < 10000; s++)
        {
            var occupiedPositions = new bool[MaxY, MaxX];

            // Move robots
            foreach (var robot in _robots)
            {
                var newPos = robot.Position + robot.Velocity;
                newPos = new Vector2((newPos.Y + MaxY) % MaxY, (newPos.X + MaxX) % MaxX);
                robot.Position = newPos;
                occupiedPositions[newPos.Y, newPos.X] = true;
            }

            // Check if there is a long vertical straight line somewhere
            for (var x = 0; x < MaxX; x++)
            {
                var maxLine = 0;

                for (var y = 0; y < MaxY; y++)
                {
                    if (occupiedPositions[y, x])
                    {
                        maxLine++;
                        if (maxLine > 8)
                        {
                            //RenderMap(occupiedPositions);
                            return s;
                        }
                    }
                    else
                    {
                        maxLine = 0;
                    }
                }
            }
        }

        return 0;
    }

    private void RenderMap(bool[,] occupiedPositions)
    {
        for (var y = 0; y < MaxY; y++)
        {
            for (var x = 0; x < MaxX; x++)
            {
                Console.Write(occupiedPositions[y, x] ? '#' : ' ');
            }

            Console.Write('\n');
        }
    }
}