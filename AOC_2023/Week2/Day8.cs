using Advent._2023.Day;

namespace AdventOfCode2023.Week2;

class Day8 : IDay
{
    private Dictionary<string, (string L, string R)> _network;
    private string _instructions;

    public void Execute()
    {
        var input = File.ReadAllLines(@"Week2\input8.txt");

        _instructions = input[0];
        _network = input[2..].ToDictionary(x => x[..3], x => (x[7..10], x[12..15]));

        Console.WriteLine($"A: {TaskA()}");
        Console.WriteLine($"B: {TaskB()}");
    }

    long TaskA() => StepsToReachNode("AAA", node => node == "ZZZ");

    long TaskB()
    {
        var partResult = _network.Where(x => x.Key[2] == 'A')
            .Select(x => StepsToReachNode(x.Key, node => node[2] == 'Z'))
            .ToArray();

        if (partResult.Length == 1) return partResult[0];

        var res = MathAlgorithms.LCM(partResult[0], partResult[1]);
        for (var i = 2; i < partResult.Length; i++)
        {
            res = MathAlgorithms.LCM(res, partResult[i]);
        }

        return res;
    }
    
    long StepsToReachNode(string startNode, Func<string, bool> endCondition)
    {
        long steps = 0;
        var lr = 0;
        var node = startNode;

        while (true)
        {
            node = _instructions[lr] == 'L' ? _network[node].L : _network[node].R;
            steps++;

            if (endCondition(node))
                return steps;

            lr = (lr + 1) % _instructions.Length;
        }
    }
}