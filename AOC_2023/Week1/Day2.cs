using Advent._2023.Day;

namespace AdventOfCode2023.Week1;

class Day2 : IDay
{
    public void Execute()
    {
        var games = GameParse(File.ReadAllText(@"Week1\input2.txt"));

        Console.WriteLine($"A: {TaskA(games)}");
        Console.WriteLine($"B: {TaskB(games)}");
    }

    record RgbSet(int R, int G,int B)
    {
        public bool IsValid() => R <= 12 && G <= 13 && B <= 14;
    }

    record Game(int Id, List<RgbSet> Sets);

    int TaskA(List<Game> games) => games
        .Where(game => game.Sets
            .All(x => x.IsValid()))
        .Select(x => x.Id)
        .Sum();

    int TaskB(List<Game> games) =>
        (from game in games
            let maxR = game.Sets.Max(x => x.R)
            let maxG = game.Sets.Max(x => x.G)
            let maxB = game.Sets.Max(x => x.B)
            select maxR * maxG * maxB)
        .Sum();

    List<Game> GameParse(string input) =>
        input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select((line, i) =>
            {
                var game = new Game(i + 1, new List<RgbSet>());

                foreach (var sets in line.Split(": ")[1].Split("; "))
                {
                    var colors = sets.Replace(", ", " ").Split(" ");
                    int red = 0, green = 0, blue = 0;

                    for (var j = 1; j < colors.Length; j += 2)
                    {
                        switch (colors[j][0])
                        {
                            case 'r':
                                red = int.Parse(colors[j - 1]);
                                break;
                            case 'g':
                                green = int.Parse(colors[j - 1]);
                                break;
                            case 'b':
                                blue = int.Parse(colors[j - 1]);
                                break;
                        }
                    }

                    game.Sets.Add(new RgbSet(red, green, blue));
                }

                return game;
            })
            .ToList();
}