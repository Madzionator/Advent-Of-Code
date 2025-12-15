namespace AdventOfCode2025.Days;

[AocData("example11_1.txt", 5, null)]
[AocData("example11_2.txt", null, 2L)]
[AocData("input11.txt", 590, 319473830844560L)]
public class Day11 : Day
{
    private record struct PathNode(string DeviceLabel, bool VisitedFft, bool VisitedDac);

    public override (object? PartA, object? PartB) Execute(string[] inputLines)
    {
        var devices = inputLines.Select(x => new
        {
            label = x[..3].ToString(),
            outputs = x[5..].Split(' ')
        }).ToDictionary(x => x.label, x => x.outputs);

        var resultA = devices.ContainsKey("you") 
            ? CountPathsFromYouToOut(devices, "you") 
            : 0;

        var resultB = devices.ContainsKey("svr")
            ? CountPathsFromSvrToOut(devices, new Dictionary<PathNode, long>(), new PathNode("svr", false, false))
            : 0L;

        return (resultA, resultB);
    }

    private int CountPathsFromYouToOut(Dictionary<string, string[]> devices, string node) 
        => node == "out" 
            ? 1 
            : devices[node].Sum(output => CountPathsFromYouToOut(devices, output));

    private long CountPathsFromSvrToOut(Dictionary<string, string[]> devices, Dictionary<PathNode, long> paths, PathNode currentNode)
    {
        if (currentNode.DeviceLabel == "out")
        {
            return currentNode is { VisitedDac: true, VisitedFft: true } ? 1 : 0;
        }

        if (paths.TryGetValue(currentNode, out var step))
        {
            return step;
        }

        var nodeResult = devices[currentNode.DeviceLabel]
            .Select(output => new PathNode
            {
                DeviceLabel = output, 
                VisitedFft = currentNode.VisitedFft || output == "fft",
                VisitedDac = currentNode.VisitedDac || output == "dac"

            })
            .Select(nextNode => CountPathsFromSvrToOut(devices, paths, nextNode))
            .Sum();

        paths[currentNode] = nodeResult;
        return nodeResult;
    }
}