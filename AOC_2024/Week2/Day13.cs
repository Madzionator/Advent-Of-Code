using System.Text.RegularExpressions;

namespace AdventOfCode2024.Week2;

internal class Day13 : Day
{
    private record Machine(int Ax, int Ay, int Bx, int By, long PriceX, long PriceY);
    private const string Pattern = @"X.(\d+), Y.(\d+)";

    private Machine[] _machines;

    public override (object resultA, object resultB) Execute()
    {
        _machines = InputLines.Chunk(4).Select(chunk =>
        {
            var a = LineToNumbers(chunk[0]);
            var b = LineToNumbers(chunk[1]);
            var price = LineToNumbers(chunk[2]);

            return new Machine(a.X, a.Y, b.X, b.Y, price.X, price.Y);

        }).ToArray();
        
        return (TaskA(), TaskB());
    }

    long TaskA() => _machines.Sum(CalculateTokens);

    long TaskB() => 
        _machines.Select(x => x with
        {
            PriceX = x.PriceX + 10000000000000,
            PriceY = x.PriceY + 10000000000000
        })
        .Sum(CalculateTokens);

    long CalculateTokens(Machine machine)
    {
        var determinant = machine.Ax * machine.By - machine.Bx * machine.Ay;
        var determinantA = machine.PriceX * machine.By - machine.Bx * machine.PriceY;
        var determinantB = machine.Ax * machine.PriceY - machine.PriceX * machine.Ay;

        var a = 1.0 * determinantA / determinant;
        var b = 1.0 * determinantB / determinant;

        if (a % 1 != 0 || b % 1 != 0)
        {
            return 0;
        }

        return (long)(a * 3 + b);
    }

    (int X, int Y) LineToNumbers(string line)
    {
        var match = Regex.Match(line.Split(": ")[1], Pattern);
        return (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
    }
}