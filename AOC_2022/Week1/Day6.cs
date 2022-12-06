using Advent._2022.Day;

namespace Advent._2022.Week1;

class Day6 : IDay
{
    public void Execute()
    {
        var signal = File.ReadAllText(@"Week1\input6.txt");

        Console.WriteLine(TaskA(signal));
        Console.WriteLine(TaskB(signal));
    }

    private int TaskA(string signal) => FindMarker(signal, 4);

    private int TaskB(string signal) => FindMarker(signal, 14);

    private static int FindMarker(string signal, int distinctLen)
    {
        return Enumerable
            .Range(distinctLen - 1, signal.Length)
            .First(i => signal[(i - (distinctLen - 1))..(i + 1)]
                .Distinct()
                .Count() == distinctLen) + 1;
    }
}