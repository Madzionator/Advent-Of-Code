using System.Diagnostics;

namespace AdventOfCode2024.Helpers;

[DebuggerDisplay("{(Direction)_directionIndex}")]
public readonly record struct Direction2
{
    private static readonly (int, int)[] DirectionValues = [ (-1, 0), (-1, 1), (0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1)];
    private readonly int _directionIndex; 

    public Direction2(Direction dir)
    {
        _directionIndex = (int)dir;
    }

    private Direction2(int index)
    {
        _directionIndex = index;
    }

    public Direction2(int dy, int dx)
    {
        var sdy = Math.Sign(dy);
        var sdx = Math.Sign(dy);

        _directionIndex = (sdy, sdx) switch
        {
            (-1, 0) => 0,
            (-1, 1) => 1,
            (0, 1) => 2,
            (1, 1) => 3,
            (1, 0) => 4,
            (1, -1) => 5,
            (0, -1) => 6,
            (-1, -1) => 7,
            _ => throw new ArgumentException()
        };
    }

    public Vector2 Vector2 => DirectionValues[_directionIndex];

    public Direction2 Rotate(Rotation rotation)
    {
        return rotation switch
        {
            Rotation.Right45 => new Direction2((_directionIndex + 1) % 8),
            Rotation.Left45 => new Direction2((_directionIndex + 7) % 8),
            Rotation.Right90 => new Direction2((_directionIndex + 2) % 8),
            Rotation.Left90 => new Direction2((_directionIndex + 6) % 8),
            Rotation.Right180 => new Direction2((_directionIndex + 4) % 8),
            _ => throw new ArgumentOutOfRangeException(nameof(Rotation))
        };
    }
    public static implicit operator Direction2(Direction dir) => new(dir);

    public static Direction2[] Sides = [Direction.Up, Direction.Right, Direction.Down, Direction.Left];
    public static Direction2[] Corners = [Direction.UpRight, Direction.DownRight, Direction.DownLeft, Direction.UpLeft];
    public static Direction2[] SidesAndCorners = [Direction.Up, Direction.UpRight, Direction.Right, Direction.DownRight, Direction.Down, Direction.DownLeft, Direction.Left, Direction.UpLeft];
}

public enum Direction
{
    Up,
    UpRight,
    Right,
    DownRight,
    Down,
    DownLeft,
    Left,
    UpLeft
}

public enum Rotation
{
    Right45,
    Left45,
    Right90,
    Left90,
    Right180,
}