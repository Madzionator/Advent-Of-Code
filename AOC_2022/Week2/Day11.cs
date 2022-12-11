using Advent._2022.Day;

namespace Advent._2022.Week2;

class Day11 : IDay
{
    public void Execute()
    {
        var input = File.ReadAllLines(@"Week2\input11.txt")
            .Chunk(7)
            .Select(c => new Monkey(c));

        var mod = input.Aggregate(1, (mod, monkey) => mod * monkey.TestMod);

        Console.WriteLine(Task(input.ToArray(), 20));
        Console.WriteLine(Task(input.ToArray(), 10000, mod));
    }

    private long Task(Monkey[] monkeys, int rounds, int mod = 1)
    {
        for (var round = 0; round < rounds; round++)
            foreach (var monkey in monkeys)
                while (monkey.Items.TryDequeue(out var item))
                {
                    item = monkey.ItemNewValue(item, mod);

                    var m = item % monkey.TestMod == 0 ? monkey.MonkeyIfTrue : monkey.MonkeyIfFalse;
                    monkeys[m].Items.Enqueue(item);
                    monkey.Inspected++;
                }

        return monkeys
            .Select(x => x.Inspected)
            .OrderDescending()
            .Take(2)
            .Multiply();
    }

    class Monkey
    {
        public Queue<long> Items = new();
        public char Op;
        public int? OpVal;
        public int TestMod;
        public int MonkeyIfTrue;
        public int MonkeyIfFalse;
        public long Inspected = 0;

        public Monkey(string[] data)
        {
            Items = new Queue<long>(data[1][18..].Split(", ").Select(long.Parse));
            Op = data[2][23];
            OpVal = data[2][25] == 'o' ? null : int.Parse(data[2][25..]);
            TestMod = int.Parse(data[3][21..]);
            MonkeyIfTrue = int.Parse(data[4][^1].ToString());
            MonkeyIfFalse = int.Parse(data[5][^1].ToString());
        }

        public long ItemNewValue(long old, long mod = 1)
        {
            var res = Op switch
            {
                '+' => old + (OpVal ?? old),
                '*' => old * (OpVal ?? old)
            };

            return mod == 1 ? res / 3 : res % mod;
        }
    }
}