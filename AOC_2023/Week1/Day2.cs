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

    record RgbSet()
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

        public bool IsValid() => R <= 12 && G <= 13 && B <= 14;
    }

    record Game(int Id)
    {
        public List<RgbSet> Sets { get; set; } = new();
    }

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
                var game = new Game(i + 1);

                foreach (var sets in line.Split(": ")[1].Split("; "))
                {
                    var colors = sets.Replace(", ", " ").Split(" ");
                    var rgb = new RgbSet();

                    for (var j = 1; j < colors.Length; j += 2)
                    {
                        switch (colors[j])
                        {
                            case "red":
                                rgb.R = Int32.Parse(colors[j - 1]);
                                break;
                            case "green":
                                rgb.G = Int32.Parse(colors[j - 1]);
                                break;
                            case "blue":
                                rgb.B = Int32.Parse(colors[j - 1]);
                                break;
                        }
                    }

                    game.Sets.Add(rgb);
                }

                return game;
            })
            .ToList();
}