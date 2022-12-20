using Advent._2022.Day;

namespace Advent._2022.Week3;

class Day20 : IDay
{
    public void Execute()
    {
        var input = File.ReadAllLines(@"Week3\input20.txt").Select(long.Parse);

        var listA = input.Select((l, i) => new Number(i, l)).ToList();
        var listB = input.Select((l, i) => new Number(i, l * 811589153L)).ToList();

        Console.WriteLine($"A: {Task(listA)}");
        Console.WriteLine($"B: {Task(listB, 10)}");
    }

    record Number(int Idx, long Val);

    private long Task(List<Number> list, int mixes = 1)
    {
        var len = list.Count;

        for (var m = 0; m < mixes; m++)
        for (var idx = 0; idx < len; idx++)
        {
            var number = list.First(x => x.Idx == idx);
            var oldI = list.IndexOf(number);
            list.RemoveAt(oldI);

            var newI = (int)((oldI + number.Val) % (len - 1));
            if (newI < 0)
            { 
                newI = len - 1 + newI;
            }

            list.Insert(newI, number);
        }

        var _0I = list.IndexOf(list.First(x => x.Val == 0));

        return list[(_0I + 1000) % len].Val
               + list[(_0I + 2000) % len].Val
               + list[(_0I + 3000) % len].Val;
    }
}