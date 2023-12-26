using Advent._2023.Day;
using Microsoft.Z3;

namespace AdventOfCode2023.Week4;

class Day24 : IDay
{
    record Hailstone(long Px, long Py, long Pz, int Vx, int Vy, int Vz);
    record LineXY(Hailstone Hailstone, double A, double B, (int dx, int dy) IncrDir);

    public void Execute()
    {
        var hailstones = File.ReadAllLines(@"Week4\input24.txt").Select(line =>
        {
            var r = line.Split(new[] { '@', ',' });
            return new Hailstone(long.Parse(r[0]), long.Parse(r[1]), long.Parse(r[2]), int.Parse(r[3]), int.Parse(r[4]), int.Parse(r[5]));

        }).ToList();

        Console.WriteLine($"A: {TaskA(hailstones)}");
        Console.WriteLine($"B: {TaskB(hailstones)}");
    }

    int TaskA(List<Hailstone> hailstones)
    {
        var lines = hailstones.Select(CalculateLineXY).ToList();
        var combinations = lines.Combinations(2).Select(x => x.ToArray()).ToList();

        return hailstones.Count > 100 ? 
            combinations.Count(combination => AreTheLinesCrossing(combination[0], combination[1], 200000000000000, 400000000000000)) 
            : combinations.Count(combination => AreTheLinesCrossing(combination[0], combination[1], 7, 27)); //for example
    }
    
    long TaskB(List<Hailstone> hailstones)
    {
        using var ctx = new Context();

        // Unknowns
        RealExpr[] sp = { ctx.MkRealConst("spx"), ctx.MkRealConst("spy"), ctx.MkRealConst("spz") };
        RealExpr[] sv = { ctx.MkRealConst("svx"), ctx.MkRealConst("svy"), ctx.MkRealConst("svz") };
        RealExpr[] t = { ctx.MkRealConst("t1"), ctx.MkRealConst("t2"), ctx.MkRealConst("t3") };

        // Knowns
        RealExpr[,] knows = new RealExpr[3, 6];
        for (var hail = 0; hail < 3; hail++)
        {
            knows[hail, 0] = ctx.MkReal(hailstones[hail].Px);
            knows[hail, 1] = ctx.MkReal(hailstones[hail].Py);
            knows[hail, 2] = ctx.MkReal(hailstones[hail].Pz);
            knows[hail, 3] = ctx.MkReal(hailstones[hail].Vx);
            knows[hail, 4] = ctx.MkReal(hailstones[hail].Vy);
            knows[hail, 5] = ctx.MkReal(hailstones[hail].Vz);
        }

        var solver = ctx.MkSolver();

        // Equations: spx + ti * svx == xi + ti * vxi ; etc for y, z
        for (var hail = 0; hail < 3; hail++)
        for (var param = 0; param < 3; param++)
        {
            solver.Add(ctx.MkEq(
                ctx.MkAdd(sp[param], ctx.MkMul(t[hail], sv[param])),
                ctx.MkAdd(knows[hail, param], ctx.MkMul(t[hail], knows[hail, param + 3]))));
        }

        if (solver.Check() == Status.SATISFIABLE)
        {
            var model = solver.Model;

            var epx = model.Evaluate(sp[0]);
            var epy = model.Evaluate(sp[1]);
            var epz = model.Evaluate(sp[2]);

            if (epx is RatNum px && epy is RatNum py && epz is RatNum pz)
            {
                return px.Numerator.Int64 + py.Numerator.Int64 + pz.Numerator.Int64;
            }
        }

        throw new Exception("Wrong input");
    }

    LineXY CalculateLineXY(Hailstone h)
    {
        var a = 1.0 * h.Vy / h.Vx;
        var b = h.Py - a * h.Px;

        return new LineXY(h, a, b, (h.Vx > 0 ? 1 : -1, h.Vy > 0 ? 1 : -1));
    }

    bool AreTheLinesCrossing(LineXY first, LineXY second, long rangeMin, long rangeMax)
    {
        if (first.A == second.A) //parallel
            return false;

        var x = (second.B - first.B) / (first.A - second.A);
        var y = first.A * x + first.B;

        if (x < rangeMin || x > rangeMax || y < rangeMin || y > rangeMax)
            return false;

        if ((first.IncrDir.dx == 1 && x < first.Hailstone.Px)
            || (first.IncrDir.dx == -1 && x > first.Hailstone.Px)
            || (first.IncrDir.dy == 1 && y < first.Hailstone.Py)
            || (first.IncrDir.dy == -1 && y > first.Hailstone.Py))
            return false;

        if ((second.IncrDir.dx == 1 && x < second.Hailstone.Px)
            || (second.IncrDir.dx == -1 && x > second.Hailstone.Px)
            || (second.IncrDir.dy == 1 && y < second.Hailstone.Py)
            || (second.IncrDir.dy == -1 && y > second.Hailstone.Py))
            return false;

        return true;
    }
}