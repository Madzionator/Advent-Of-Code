using Advent._2023.Day;

namespace AdventOfCode2023.Week1;

class Day1 : IDay
{
    public void Execute()
    {
        var input = File.ReadAllText(@"Week1\input1.txt")
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        Console.WriteLine($"A: {Task(input)}");
        Console.WriteLine($"B: {Task(input.Select(ConvertDigits))}");
    }

    int Task(IEnumerable<string> input) =>
        input.Select(GetOnlyDigits)
            .Where(x => x.Length > 0)
            .Select(x => (x.First() - '0') * 10 + x.Last() - '0')
            .Sum();

    static string GetOnlyDigits(string str) => str.Where(char.IsDigit).Aggregate("", (current, c) => current + c);

    readonly Dictionary<string, string> _numberDict = new()
    {
        { "one", "1" }, { "two", "2" }, { "three", "3" }, { "four", "4" },
        { "five", "5" }, { "six", "6" }, { "seven", "7" }, { "eight", "8" },
        { "nine", "9" }
    };

    string ConvertDigits(string str)
    {
        while (true)
        {
            var lowestIndex = 1000;
            var numberToReplace = "";

            foreach (var number in _numberDict.Keys)
            {
                var index = str.IndexOf(number);
                if (index >= 0 && index < lowestIndex)
                {
                    lowestIndex = index;
                    numberToReplace = number;
                }
            }

            if (lowestIndex == 1000)
                 return str;

            str = str.ReplaceAt(lowestIndex, _numberDict[numberToReplace]);
        }
    }
}