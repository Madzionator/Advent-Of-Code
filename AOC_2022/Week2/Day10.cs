using Advent._2022.Day;

namespace Advent._2022.Week2;

class Day10 : IDay
{
    public void Execute()
    {
        var input = File.ReadAllLines(@"Week2\input10.txt")
            .Select(x => x.Split(' '))
            .Select(x => x[0] == "noop" ? (1, 0) : (2, int.Parse(x[1])))
            .ToList();

        Console.WriteLine(Task(input));
    }

    private int Task(List<(int Cl, int Val)> input)
    {
        int X = 1;
        var instr = (-1, 0, 0); //instr nr, step, toAdd
        int result = 0;

        for (var clock = 1;; clock++)
        {
            if (instr.Item2 == 0)
            {
                instr.Item1++;
                if (instr.Item1 >= input.Count)
                    break;

                instr.Item2 = input[instr.Item1].Cl;
                instr.Item3 = input[instr.Item1].Val;
            }

            if(clock%40-2 <= X && X <= clock%40)
                Console.Write('#');
            else
                Console.Write(' ');

            if (clock%40 == 0)
                Console.Write(Environment.NewLine);

            if ((clock - 20) % 40 == 0)
                result += clock * X;

            if (instr.Item2 == 1)
                X += instr.Item3;

            instr.Item2--;
        }

        Console.Write(Environment.NewLine);
        return result;
    }
}