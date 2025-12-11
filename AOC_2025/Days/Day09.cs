using AdventOfCode2025.Helpers;

namespace AdventOfCode2025.Days;

[AocData("example09_1.txt", 50L, 24L)]
[AocData("input09.txt", 4750297200L, 1578115935L)]
public class Day09 : Day
{
    public override (object? PartA, object? PartB) Execute(string[] inputLines)
    {
        var redTiles = inputLines.Select(line =>
        {
            var splits = line.Split(',');
            return new Vector2(int.Parse(splits[1]), int.Parse(splits[0]));
        }).ToList();

        var greenLineSegments = GetGreenLineSegments(redTiles);
        
        var resultA = 0L;
        var resultB = 0L;

        for (var i = 0; i < redTiles.Count - 1; i++)
        for (var j = i + 1; j < redTiles.Count; j++)
        {
            var d = 1L * (Math.Abs(redTiles[i].X - redTiles[j].X) + 1) * (Math.Abs(redTiles[i].Y - redTiles[j].Y) + 1);
            if (d > resultA)
            {
                resultA = d;
            }

            if (d > resultB && !IntersectWithAnySegment(redTiles[i], redTiles[j], greenLineSegments))
            {
                resultB = d;
            }
        }

        return (resultA, resultB);
    }

    private List<(Vector2 A, Vector2 B)> GetGreenLineSegments(List<Vector2> redTiles)
    {
        var segments = new List<(Vector2, Vector2)>();
        var p0 = redTiles.MinBy(v => v.Y * 100 + v.X);

        var p1 = p0;
        var p2 = redTiles.Where(v => v.Y == p1.Y && v != p0)
            .MinBy(v => Math.Abs(p1.X - v.X) + Math.Abs(p1.Y - v.Y));

        var previouslyCommonX = p1.X == p2.X;
        segments.Add((p1, p2));

        while (p2 != p0)
        {
            var nextP = previouslyCommonX 
                ? redTiles.Where(v => v.Y == p2.Y && v != p1 && v != p2).MinBy(v => Math.Abs(p2.X - v.X)) 
                : redTiles.Where(v => v.X == p2.X && v != p1 && v != p2).MinBy(v => Math.Abs(p2.Y - v.Y));

            p1 = p2;
            p2 = nextP;
            segments.Add((p1, p2));

            previouslyCommonX = !previouslyCommonX;
        }

        return segments;
    }

    private bool IntersectWithAnySegment(Vector2 cornerA, Vector2 cornerC, List<(Vector2 A, Vector2 B)> greenLineSegments)
    {
        var minY = Math.Min(cornerA.Y, cornerC.Y);
        var maxY = Math.Max(cornerA.Y, cornerC.Y);
        var minX = Math.Min(cornerA.X, cornerC.X);
        var maxX = Math.Max(cornerA.X, cornerC.X);

        foreach ((Vector2 A, Vector2 B) segment in greenLineSegments)
        {
            if (segment.A.Y == segment.B.Y)
            {
                var y = segment.A.Y;
                var aX = Math.Min(segment.A.X, segment.B.X);
                var bX = Math.Max(segment.A.X, segment.B.X);

                if (minY < y && y < maxY && 
                    ((aX < minX && bX > minX) || 
                     (aX < maxX && bX > maxX) || 
                     (aX >= minX && bX <= maxX)))
                {
                    return true;
                }
            }
            else
            {
                var x = segment.A.X;
                var aY = Math.Min(segment.A.Y, segment.B.Y);
                var bY = Math.Max(segment.A.Y, segment.B.Y);

                if (minX < x && x < maxX &&
                    ((aY < minY && bY > minY) ||
                     (aY < maxY && bY > maxY) ||
                     (aY >= minY && bY <= maxY)))
                {
                    return true;
                }
            }
        }

        return false;
    }

}