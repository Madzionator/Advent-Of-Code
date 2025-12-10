using AdventOfCode2025.Helpers;

namespace AdventOfCode2025.Days;

[AocData("example04_1.txt", 13, 43)]
[AocData("input04.txt", 1349, 8277)]
public class Day04 : Day
{
    public override (object? PartA, object? PartB) Execute(string[] inputLines)
    {
        var input = new HashSet<Vector2>();

        for (var y = 0; y < inputLines.Length; y++)
        for (var x = 0; x < inputLines[y].Length; x++)
            if (inputLines[y][x] == '@')
            {
                input.Add((y, x));
            }

        return Task(input);
    }

    private (int, int) Task(HashSet<Vector2> input)
    {
        var removed = RemoveAccessibleRolls(input);

        var answerPartA = removed;
        var answerPartB = removed;

        do
        {
            removed = RemoveAccessibleRolls(input);
            answerPartB += removed;

        } while (removed > 0);

        return (answerPartA, answerPartB);
    }

    private int RemoveAccessibleRolls(HashSet<Vector2> input)
    {
        var rollsToRemove = new HashSet<Vector2>();
        var directions = Direction2.SidesAndCorners;

        foreach (var rollOfPaper in input.Where(rollOfPaper => directions.Count(dir => input.Contains(rollOfPaper.Move(dir))) < 4))
        {
            rollsToRemove.Add(rollOfPaper);
        }

        foreach (var rollOfPaper in rollsToRemove)
        {
            input.Remove(rollOfPaper);
        }

        return rollsToRemove.Count;
    }
}