using Advent._2022.Day;

namespace Advent._2022.Week2;

class Day10 : IDay
{
    public void Execute()
    {
        var instructions = new Queue<int>(
            File.ReadLines(@"Week2\input10.txt")
                .SelectMany(ParseLine));

        Console.WriteLine(Task(instructions));
    }

    IEnumerable<int> ParseLine(string line)
    {
        yield return 0;
        if (line[0] == 'a')
            yield return int.Parse(line[5..]);
    }

    private int Task(Queue<int> instructions)
    {
        var X = 1;
        var result = 0;

        for (var cycle = 1; instructions.Count > 0; cycle++)
        {
            if ((cycle - 20) % 40 == 0)
                result += cycle * X;

            Draw(cycle%40);

            X += instructions.Dequeue();
        }

        Console.WriteLine();
        return result;

        void Draw(int spriteEnd)
        {
            Console.Write(spriteEnd - 2 <= X && X <= spriteEnd ? '#' : ' ');

            if (spriteEnd == 0)
                Console.WriteLine();
        }
    }
}