using Advent._2023.Day;

class Day7 : IDay
{
 
    public void Execute()
    {
        var hands = File.ReadAllLines(@"Week1\input7.txt")
            .Select(line => new Hand(line.Split(' ')))
            .ToList();
            
        Console.WriteLine($"A: {Task(hands, true)}");
        Console.WriteLine($"B: {Task(hands, false)}");
    }

    int Task(List<Hand> hands, bool isTaskA)
    {
        hands.Sort((f, s) => CompareHands(f, s, isTaskA));

        return hands.Select((h, i) => h.Bid * (i + 1)).Sum();
    }

    class Hand
    {
        public char[] Cards { get; }
        public int Bid { get; }
        public Type AType { get; }
        public Type BType { get; }

        public Hand(string[] x)
        {
            Cards = x[0].ToCharArray();
            Bid = int.Parse(x[1]);
            (AType, BType) = CheckHandTypes(Cards);
        }
    }

    enum Type
    {
        FiveOf = 7,
        FourOf = 6,
        FullHouse = 5,
        ThreeOf = 4,
        TwoPair = 3,
        OnePair = 2,
        High = 1,
    }

    int CompareHands(Hand first, Hand second, bool isTaskA)
    {
        switch (isTaskA)
        {
            case true when first.AType != second.AType:
                return first.AType > second.AType ? 1 : -1;

            case false when first.BType != second.BType:
                return first.BType > second.BType ? 1 : -1;
        }

        var cardValues = new Dictionary<char, int>()
        {
            { 'A', 14 }, { 'K', 13 }, { 'Q', 12 }, { 'J', isTaskA ? 11 : 1 }, { 'T', 10 }, { '9', 9 }, { '8', 8 },
            { '7', 7 }, { '6', 6 }, { '5', 5 }, { '4', 4 }, { '3', 3 }, { '2', 2 }
        };

        for (var i = 0; i < 5; i++)
        {
            if (first.Cards[i] != second.Cards[i])
                return cardValues[first.Cards[i]] > cardValues[second.Cards[i]] ? 1 : -1;
        }

        return 0;
    }


    static (Type, Type) CheckHandTypes(char[] handCards)
    {
        Type a, b;

        var cardTypes = new Dictionary<char, int>();
        foreach (var c in handCards)
            cardTypes.AddOrSet(c, 1);

        a = aType();

        var isJoker = cardTypes.ContainsKey('J');
        var amount = isJoker ? cardTypes['J'] : 0;
        if (isJoker)
            cardTypes.Remove('J');

        b = bType();

        return (a, b);

        Type aType()
        {
            var charsDesc = cardTypes.OrderByDescending(x => x.Value).ToArray();

            return charsDesc[0].Value switch
            {
                5 => Type.FiveOf,
                4 => Type.FourOf,
                3 => charsDesc[1].Value == 2 ? Type.FullHouse : Type.ThreeOf,
                2 => charsDesc[1].Value == 2 ? Type.TwoPair : Type.OnePair,
                _ => Type.High
            };
        }

        Type bType()
        {
            if (!isJoker) return a;
            if (amount == 5) return Type.FiveOf;

            var charsDesc = cardTypes.OrderByDescending(x => x.Value).ToArray();

            return (amount + charsDesc[0].Value) switch
            {
                5 => Type.FiveOf,
                4 => Type.FourOf,
                3 => charsDesc[1].Value == 2 ? Type.FullHouse : Type.ThreeOf,
                _ => Type.OnePair
            };
        }
    }
}