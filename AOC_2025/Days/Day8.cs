using Xunit.Internal;

namespace AdventOfCode2025.Days;

[AocData("example8_1.txt", 40, 25272L)]
[AocData("input8.txt", 122636, 9271575747L)]
public class Day8 : Day
{
    private record struct Point(int X, int Y, int Z);
    private record struct Connection(Point A, Point B, long SquaredLength);

    public override (object? PartA, object? PartB) Execute(string[] inputLines)
    {
        var junctionBoxes = inputLines.Select(line =>
        {
            var splits =  line.Split(',').Select(int.Parse).ToArray();
            return new Point(splits[0], splits[1], splits[2]);
        }).ToArray();

        var possibleConnections = GetPossibleConnections(junctionBoxes);

        return ConnectJunctionBoxes(possibleConnections, junctionBoxes.Length);
    }

    List<Connection> GetPossibleConnections(Point[] junctionBoxes)
    {
        var possibleConnections = new List<Connection>();

        for (var i = 0; i < junctionBoxes.Length - 1; i++)
        {
            var pointA = junctionBoxes[i];
            for (var j = i + 1; j < junctionBoxes.Length; j++)
            {
                var pointB = junctionBoxes[j];
                var l = Math.Pow(pointA.X - pointB.X, 2) + Math.Pow(pointA.Y - pointB.Y, 2) + Math.Pow(pointA.Z - pointB.Z, 2);
                possibleConnections.Add(new Connection(pointA, pointB, (long)l));
            }
        }

        return possibleConnections.OrderBy(c => c.SquaredLength).ToList();
    }

    (int, long) ConnectJunctionBoxes(List<Connection> connections, int boxQuantity)
    {
        var circuits = new List<HashSet<Point>>();

        var toLinkInA = boxQuantity < 25 ? 10 : 1000;
        var linked = 0;

        var resultA = 0;
        var resultB = 0L;

        foreach (var connection in connections)
        {
            var circuitWithA = circuits.Select((c, i) => (Points: c, Index: i)).FirstOrDefault(c => c.Points.Contains(connection.A));
            var circuitWithB = circuits.Select((c, i) => (Points: c, Index: i)).FirstOrDefault(c => c.Points.Contains(connection.B));

            if (circuitWithA.Points is null && circuitWithB.Points is null)
            {
                circuits.Add(new HashSet<Point> { connection.A, connection.B });
            }
            else if (circuitWithA.Points is not null && circuitWithB.Points is not null)
            {
                if (circuitWithA.Index != circuitWithB.Index)
                {
                    circuits[circuitWithA.Index].AddRange(circuitWithB.Points);
                    circuits.RemoveAt(circuitWithB.Index);
                }
            }
            else if (circuitWithA.Points is not null)
            {
                circuits[circuitWithA.Index].Add(connection.B);
            }
            else // circuit2.c is not null
            {
                circuits[circuitWithB.Index].Add(connection.A);
            }

            linked++;

            if (linked == toLinkInA)
            {
                var partA = circuits.Select(c => c.Count).OrderDescending().Take(3).ToArray();
                resultA = partA[0] * partA[1] * partA[2];
            }

            if (circuits.Count == 1 && circuits[0].Count == boxQuantity)
            {
                resultB = 1L * connection.A.X * connection.B.X;
                break;
            }
        }

        return (resultA, resultB);
    }
}