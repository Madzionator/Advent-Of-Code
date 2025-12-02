namespace AdventOfCode2025.Days;

[AocData("example2_1.txt", 1227775554L, 4174379265L)]
[AocData("input2.txt", 54641809925, 73694270688)]
public class Day2 : Day
{
    private static readonly Dictionary<int, int[]> ValidPatternLengthsByNumberLength = new()
    {
        { 1, [] },
        { 2, [1] },
        { 3, [1] },
        { 4, [2, 1] },
        { 5, [1] },
        { 6, [3, 1, 2] },
        { 7, [1] },
        { 8, [4, 1, 2] },
        { 9, [1, 3] },
        { 10, [5, 1, 2] },
        { 11, [1] },
        { 12, [6, 1, 2, 3, 4] }
    };

    public override (object? PartA, object? PartB) Execute(string[] inputLines)
    {
        var input = inputLines.First().Split(',').Select(x =>
        {
            var range = x.Split('-');
            return (long.Parse(range[0]), long.Parse(range[1]));
        }).ToList();

        return Task(input);
    }

    private (long, long) Task(List<(long From, long To)> input)
    {
        var invalidIdSumA = 0L;
        var invalidIdSumB = 0L;

        foreach (var range in input)
        {
            for (var number = range.From; number <= range.To; number++)
            {
                var numberStr = number.ToString();
                foreach (var patternLength in ValidPatternLengthsByNumberLength[numberStr.Length])
                {
                    if (OccursAtLeastTwoTimes(numberStr, numberStr[..patternLength]))
                    {
                        if (numberStr.Length == patternLength * 2)
                        {
                            invalidIdSumA += number;
                        }

                        invalidIdSumB += number;
                        break;
                    }
                }
            }
        }

        return (invalidIdSumA, invalidIdSumB);
    }

    private static bool OccursAtLeastTwoTimes(string number, string pattern)
    {
        var index = 0;

        while (index < number.Length)
        {
            if (number[index..(pattern.Length + index)] != pattern)
            {
                return false;
            }

            index += pattern.Length;
        }

        return true;
    }
}