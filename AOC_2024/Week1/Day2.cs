namespace AdventOfCode2024.Week1;

class Day2 : Day
{
    public override (object, object) Execute()
    {
        var records = InputLines
            .Select(line => line.Split(' ').Select(int.Parse).ToArray())
            .ToList();

        return (TaskA(records), TaskB(records));
    }

    int TaskA(List<int[]> records) => records.Count(IsSafe);

    int TaskB(List<int[]> records)
    {
        int count = 0;
        foreach (var record in records)
        {
            if (IsSafe(record))
            {
                count++;
                continue;
            }

            var variants = record
                .Select((num, i) => record
                    .Where((x, index) => index != i)
                    .ToArray());

            if (variants.Any(IsSafe))
            {
                count++;
            }
        }

        return count;
    }

    private bool IsSafe(int[] record)
    {
        var direction = 0;

        for (int i = 0; i < record.Length - 1; i++)
        {
            var difference = record[i] - record[i + 1];

            if (difference == 0 || Math.Abs(difference) > 3)
            {
                return false;
            }

            if (i == 0)
            {
                direction = difference > 0 ? 1 : -1;
            }
            else if (difference * direction < 0)
            {
                return false;
            }
        }

        return true;
    }
}