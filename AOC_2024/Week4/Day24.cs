namespace AdventOfCode2024.Week4;

internal class Day24 : Day
{
    private record Gate(string Num1, Operation Operation, string Num2, string Result);
    private enum Operation { AND, XOR, OR }

    private Gate[] _gates;

    public override (object resultA, object resultB) Execute()
    {
        var emptyLineIdx = Array.FindIndex(InputLines, string.IsNullOrWhiteSpace);
        var wires = InputLines.Take(emptyLineIdx).Select(line =>
        {
            var split = line.Split(": ");
            return (split[0], split[1] == "1");
        }).ToDictionary(x => x.Item1, x => x.Item2);

        _gates = InputLines.Skip(emptyLineIdx + 1).Select(line =>
        {
            var split = line.Split(' ');
            var op = split[1] == "AND" ? Operation.AND : split[1] == "OR" ? Operation.OR : Operation.XOR;
            return new Gate(split[0], op, split[2], split[4]);
        }).ToArray();

        return (TaskA(wires), TaskB());
    }

    long TaskA(Dictionary<string, bool> wires)
    {
        var queue = new Queue<Gate>();

        foreach (var gate in _gates)
        {
            queue.Enqueue(gate);
        }

        while (queue.TryDequeue(out var gate))
        {
            if (!wires.ContainsKey(gate.Num1) || !wires.ContainsKey(gate.Num2))
            {
                queue.Enqueue(gate);
                continue;
            }

            var value = gate.Operation switch
            {
                Operation.AND => wires[gate.Num1] && wires[gate.Num2],
                Operation.OR => wires[gate.Num1] || wires[gate.Num2],
                Operation.XOR => wires[gate.Num1] != wires[gate.Num2],
            };
            wires.Add(gate.Result, value);
        }

        var orderedZ = wires
            .Where(x => x.Key.StartsWith('z'))
            .OrderByDescending(x => x.Key)
            .Select(x => x.Value ? 1 : 0)
            .ToList();

        return Convert.ToInt64(string.Join("", orderedZ), 2);
    }

    string TaskB()
    {
        var lastZ = _gates.Where(g => g.Result.StartsWith('z')).MaxBy(x => x.Result);
        var invalidGates = _gates.Where(g => IsInvalidGate(g, lastZ));
        
        return string.Join(',', invalidGates.Select(x => x.Result).Order());
    }

    bool IsInvalidGate(Gate gate, Gate lastZ)
    {
        if (IsZ(gate.Result) && gate.Result != lastZ.Result)
        {
            return gate.Operation != Operation.XOR;
        }

        if (!IsZ(gate.Result) && !IsXY(gate.Num1) && !IsXY(gate.Num2))
        {
            return gate.Operation == Operation.XOR;
        }

        if (!IsXY(gate.Num1) || !IsXY(gate.Num2))
            return false;

        if (gate.Num1.EndsWith("00") && gate.Num2.EndsWith("00"))
            return false;

        return !_gates.Any(other
            => other != gate
               && (other.Num1 == gate.Result || other.Num2 == gate.Result)
               && other.Operation == (gate.Operation == Operation.XOR ? Operation.XOR : Operation.OR));

    }

    static bool IsXY(string wire) => wire.StartsWith('x') || wire.StartsWith('y');
    static bool IsZ(string wire) => wire.StartsWith("z");
}