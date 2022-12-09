using Advent._2022.Day;

namespace Advent._2022.Week2;

class Day9 : IDay
{
    record Instruction(char Direction, int Length);

    public void Execute()
    {
        var instructions = File.ReadAllLines(@"Week2\input9.txt")
            .Select(x => x.Split(' '))
            .Select(x => new Instruction(x[0][0], int.Parse(x[1])))
            .ToList();

        Console.WriteLine(TaskA(instructions));
        Console.WriteLine(TaskB(instructions));
    }

    private int TaskA(List<Instruction> instructions)
    {
        var visited = new HashSet<(int, int)>{ (0, 0) };
        (int, int) head = (0, 0), tail = (0, 0), temp;
        int xIncr = 0, yIncr = 0;

        foreach (var instr in instructions)
        {
            switch (instr.Direction)
            {
                case 'U': xIncr = 0; yIncr = 1; break;
                case 'D': xIncr = 0; yIncr = -1; break;
                case 'L': xIncr = -1; yIncr = 0; break;
                case 'R': xIncr = 1; yIncr = 0; break;
            }

            for (int i = 0; i < instr.Length; i++)
            {
                temp = head;
                head.Item1 += xIncr;
                head.Item2 += yIncr;

                if (Math.Abs(head.Item2 - tail.Item2) > 1 || Math.Abs(head.Item1 - tail.Item1) > 1)
                    tail = temp;

                visited.Add(tail);
            }
        }

        return visited.Count;
    }
    
    private int TaskB(List<Instruction> instructions)
    {
        var visited = new HashSet<(int X, int Y)>{ (0, 0) };
        var rope = new (int X, int Y)[10];

        int xIncr = 0, yIncr = 0;

        foreach (var instr in instructions)
        {
            switch (instr.Direction)
            {
                case 'U': xIncr = 0; yIncr = 1; break;
                case 'D': xIncr = 0; yIncr = -1; break;
                case 'L': xIncr = -1; yIncr = 0; break;
                case 'R': xIncr = 1; yIncr = 0; break;
            }

            for (var i = 0; i < instr.Length; i++)
            {
                rope[0].X += xIncr;
                rope[0].Y += yIncr;

                for (var knot = 1; knot < 10; knot++)
                {
                    if (Math.Abs(rope[knot-1].X - rope[knot].X) > 1 ||
                        Math.Abs(rope[knot-1].Y - rope[knot].Y) > 1)
                    {
                        rope[knot] = NewPosition(rope[knot - 1], rope[knot]);
                    }
                    else
                        break;

                    if (knot == 9)
                        visited.Add(rope[9]);
                }
            }
        }
        
        return visited.Count;

        (int, int) NewPosition((int X, int Y) H, (int X, int Y) T)
        {
            var dx = Math.Abs(H.X - T.X);
            var dy = Math.Abs(H.Y - T.Y);

            if (dx == 2 && dy == 2) // 3x3
                return ((H.X + T.X) / 2, (H.Y + T.Y) / 2);

            if (dx == 2 && dy == 0 || dx == 0 && dy == 2) // 3x1 v 1x3
                return ((H.X + T.X) / 2, (H.Y + T.Y) / 2);

            if (dy == 1) //2x3
                return ((H.X + T.X) / 2, H.Y);

            //dx == 1 3x2
            return (H.X, (H.Y + T.Y) / 2);
        }
    }
}