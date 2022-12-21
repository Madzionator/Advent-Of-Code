using Advent._2022.Day;

namespace Advent._2022.Week3;

class Day21 : IDay
{
    class Monkey
    {
        public string Name { get; }
        public long? Number;
        public Monkey M1 { get; set; }
        public Monkey M2 { get; set; }
        public char Operation { get; set; }

        public Monkey(string name)
        {
            Name = name;
        }

        public long YellNumber()
        {
            if (Number is not null)
                return (long)Number;

            Number = Operation switch
            {
                '+' => M1.YellNumber() + M2.YellNumber(),
                '-' => M1.YellNumber() - M2.YellNumber(),
                '*' => M1.YellNumber() * M2.YellNumber(),
                '/' => M1.YellNumber() / M2.YellNumber(),
                _ => 0
            };

            return (long)Number;
        }
    }

    public void Execute()
    {
        var input = File.ReadAllLines(@"Week3\input21.txt").Select(x => x.Replace(":", ""));
        var monkeys = ProcessInput(input);

        Console.WriteLine($"A: {TaskA(monkeys)}");
        Console.WriteLine($"B: {TaskB(monkeys)}");
    }

    Dictionary<string, Monkey> ProcessInput(IEnumerable<string> input)
    {
        var monkeys = input
            .Select(line => line[..4])
            .ToDictionary(name => name, name => new Monkey(name));

        foreach (var line in input)
        {
            var info = line.Split(' ');
            if (info.Length < 3)
            {
                monkeys[info[0]].Number = long.Parse(info[1]);
            }
            else
            {
                monkeys[info[0]].M1 = monkeys[info[1]];
                monkeys[info[0]].M2 = monkeys[info[3]];
                monkeys[info[0]].Operation = info[2][0];
            }
        }

        return monkeys;
    }

    private long TaskA(Dictionary<string, Monkey> monkeys)
    {
        monkeys["root"].YellNumber();
        return (long)monkeys["root"].Number;
    }

    private long TaskB(Dictionary<string, Monkey> monkeys)
    {
        var rootMonkey = monkeys["root"];
        var human = monkeys["humn"];

        rootMonkey.YellNumber();
        var m1 = rootMonkey.M1.Number;
        CleanMonkeyNumber(monkeys);

        human.Number = 10000000;
        rootMonkey.YellNumber();
        var monkeyWithConstResult = m1 == rootMonkey.M1.Number ? 1 : 2;
        var targetNumber = monkeyWithConstResult is 1 ? (long)rootMonkey.M1.Number : (long)rootMonkey.M2.Number;

        long start = 0;
        var end = 10000000000000;
        var incr = end/20;

        while (true)
        {
            var results = new List<(long i, long res)>();

            for (var i = start; i < end; i += incr)
            {
                CleanMonkeyNumber(monkeys);
                human.Number = i;
                rootMonkey.YellNumber();

                results.Add((i, monkeyWithConstResult is 1 ? (long)rootMonkey.M2.Number : (long)rootMonkey.M1.Number));

                if (rootMonkey.M1.Number == rootMonkey.M2.Number)
                    return i;
            }

            var nearest = results.MinBy(i => Math.Abs(targetNumber - i.res));

            start = nearest.i - incr;
            end = nearest.i + incr;
            incr /= 10;

            if (incr <= 20)
                incr = 1;
        }
    }

    void CleanMonkeyNumber(Dictionary<string, Monkey> monkeys)
    {
        foreach (var monkey in monkeys.Where(monkey => monkey.Value.Operation != '\0'))
        {
            monkey.Value.Number = null;
        }
    }
}