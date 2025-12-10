namespace AdventOfCode2025.Days;

[AocData("example01_1.txt", 3, 6)]
[AocData("input01.txt", 1040, 6027)]
public class Day01 : Day
{
    public override (object? PartA, object? PartB) Execute(string[] inputLines)
    {
        var input = inputLines
            .Where(x => x.Length > 0)
            .Select(line => line[0] == 'L'
                ? -int.Parse(line[1..])
                : int.Parse(line[1..]))
            .ToList();

        return (TaskA(input), TaskB(input));
    }

    private int TaskA(List<int> input)
    {
        var atZero = 0;
        var currentPosition = 50;

        foreach (var rotate in input) 
        {
            currentPosition = (currentPosition + 100 + rotate) % 100;
            if (currentPosition == 0)
            { 
                atZero++;
            }
        }

        return atZero;
    }

    private int TaskB(List<int> input)
    {
        var atZero = 0;
        var currentPosition = 50;

        foreach (var rotate in input)
        {
            var fullCycles = Math.Abs(rotate) / 100;
            var remainder = rotate > 0 
                ? rotate - fullCycles * 100 
                : rotate + fullCycles * 100;

            var previouslyZero = currentPosition == 0;

            atZero += fullCycles;
            currentPosition += remainder;

            if (!previouslyZero && currentPosition is > 99 or <= 0)
            {
                atZero++;
            }

            currentPosition = (currentPosition + 100) % 100;
        }

        return atZero;
    }
}