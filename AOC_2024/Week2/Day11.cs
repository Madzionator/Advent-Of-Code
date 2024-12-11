namespace AdventOfCode2024.Week2;

internal class Day11 : Day
{
    private Dictionary<long, long> _rocks;

    public override (object resultA, object resultB) Execute()
    {
        _rocks = InputLines[0]
            .Split(' ')
            .Select(long.Parse)
            .ToDictionary(x => x, x => 1L); //rock, count

        Task(0, 25);
        var resultA = _rocks.Sum(x => x.Value);

        Task(25, 75);
        var resultB = _rocks.Sum(x => x.Value);

        return (resultA, resultB);
    }

    void Task (int blinked, int stopBlinkAt)
    {
        for (var blink = blinked + 1; blink <= stopBlinkAt; blink++)
        {
            var changedRocks = new Dictionary<long, long>();
            foreach (var (number, count) in _rocks)
            {
                if (number == 0)
                {
                    changedRocks.AddOrSet(1, count);
                    continue;
                }

                var digits = number < 10 ? 1 : (int)Math.Floor(Math.Log10(number) + 1);
                if (digits % 2 == 0)
                {
                    var div = (int)Math.Pow(10, digits / 2);

                    changedRocks.AddOrSet(number / div, count);
                    changedRocks.AddOrSet(number % div, count);
                    continue;
                }

                changedRocks.AddOrSet(number*2024, count);
            }

            _rocks = changedRocks;
        }
    }
}