namespace Advent._2022.Week1;
using Advent._2022.Day;

class Day1 : IDay
{
    public void Execute()
    {
        var calories = File.ReadAllLines(@"Week1\input1.txt");

        Console.WriteLine(TaskA(calories));
        Console.WriteLine(TaskB(calories));
    }

    public int TaskA(string[] calories)
    {
        int largestSum = 0;

        int sum = 0;
        foreach(string cal in calories)
        {
            if (string.IsNullOrEmpty(cal))
            {
                if (sum > largestSum)
                    largestSum = sum;
                sum = 0;
            }
            else
                sum += int.Parse(cal);
        }

        return largestSum;
    }

    public int TaskB(string[] calories)
    {
        var allSums = new List<int>();

        int sum = 0;
        foreach(string cal in calories)
        {
            if (string.IsNullOrEmpty(cal))
            {
                allSums.Add(sum);
                sum = 0;
            }
            else
                sum += int.Parse(cal);
        }

        return allSums
            .OrderByDescending(x => x)
            .ToList()
            .Take(3)
            .Sum();
    }
}
