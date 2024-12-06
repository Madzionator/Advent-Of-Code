using System.Diagnostics;

namespace AdventOfCode2024.Helpers;

[DebuggerDisplay("Y: {Y}, X: {X}")]
public readonly record struct Vector2
{
    public readonly int Y;
    public readonly int X;

    public Vector2(int y, int x)
    {
        Y = y;
        X = x;
    }

    public static implicit operator Vector2((int y, int x) pair) => new Vector2(pair.y, pair.x);
    public static Vector2 operator +(Vector2 a, Vector2 b) => (a.Y + b.Y, a.X + b.X);
    public static Vector2 operator -(Vector2 a, Vector2 b) => (a.Y - b.Y, a.X - b.X);
    public static Vector2 operator *(Vector2 a, int c) => (a.Y * c, a.X * c);

    public override int GetHashCode()
    {
        unchecked
        {
            return (2137 * X) ^ Y;
        }
    }
}

public static class Vector2Extensions
{
    public static Vector2 Move(this Vector2 vector, Direction2 dir, int steps = 1)
    {
        return vector + dir.Vector2 * steps;
    }
}