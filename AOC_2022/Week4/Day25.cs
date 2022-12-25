using Advent._2022.Day;

namespace Advent._2022.Week4;

class Day25 : IDay
{
    public void Execute()
    {
        var input = File.ReadAllLines(@"Week4\input25.txt");

        Console.WriteLine($"A: {TaskA(input)}");
    }

    private string TaskA(string[] input) => Dec2SnafuConvert(input.Select(Snafu2DecConvert).Sum());

    private long Snafu2DecConvert(string snafu)
    {
        long result = 0;
        var j = 0;

        for (var i = snafu.Length - 1; i >= 0; i--, j++)
        {
            result += (long)Math.Pow(5, j) * (snafu[i]) switch
            {
                '0' => 0,
                '1' => 1,
                '2' => 2,
                '-' => -1,
                '=' => -2
            };
        }

        return result;
    }

    private string Dec2SnafuConvert(long dec)
    {
        var result = "";
        while (dec != 0)
        {
            var (digit, value) = (dec % 5) switch
            {
                0 => ('0', 0),
                1 => ('1', 1),
                2 => ('2', 2),
                3 => ('=', -2),
                4 => ('-', -1),
            };

            result = $"{digit}{result}";
            dec = (dec - value) / 5;
        }

        return result;
    }
}