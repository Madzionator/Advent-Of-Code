using Advent._2023.Day;

namespace AdventOfCode2023.Week1;

class Day4 : IDay
{
    public void Execute()
    {
        var cards = File.ReadAllLines(@"Week1\input4.txt").Select((line, i) =>
        {
            var x = line.Split(':', '|');
            var winning = x[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            var chosen = x[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            return new Card(i, winning, chosen);

        }).ToList();

        Console.WriteLine($"A: {TaskA(cards)}");
        Console.WriteLine($"B: {TaskB(cards)}");
    }

    record Card(int Id, int[] Winning, int[] Chosen)
    {
        public int Instances { get; set; } = 1;
    }

    int TaskA(List<Card> cards) => cards
        .Sum(card => card.Winning
            .Where(wi => card.Chosen
                .Any(ci => wi == ci))
            .Aggregate(0, (current, wi) => current == 0 ? 1 : current * 2));

    int TaskB(List<Card> cards)
    {
        foreach (var card in cards)
        {
            var corrected = card.Winning.Count(t1 => card.Chosen.Any(t => t1 == t));

            for (var i = card.Id + 1; i <= card.Id + corrected; i++)
                cards[i].Instances += card.Instances;
        }

        return cards.Sum(x => x.Instances);
    }
}