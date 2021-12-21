using System;
using System.IO;
using System.Linq;

namespace Advent._2021.Week3
{
    class Day21
    {
        public static void Execute()
        {
            var file = File.ReadAllLines(@"Week3\input21.txt");
            var startPositions = file.Select(x => int.Parse(x.Split(": ")[^1])).ToArray();

            Console.WriteLine(TaskA(startPositions[0], startPositions[1]));
            Console.WriteLine(TaskB(startPositions[0], startPositions[1])); //wait about 3s
        }

        public class Player
        {
            public int position;
            public int score = 0;
        }

        public static int TaskA(int p1, int p2)
        {
            var pl1 = new Player() { position = p1 };
            var pl2 = new Player() { position = p2 };

            var die = 0;
            var rolls = 0;

            while (true)
            {
                MakeTurn(pl1);
                if (pl1.score >= 1000)
                    return pl2.score * rolls;

                MakeTurn(pl2);
                if (pl2.score >= 1000)
                    return pl1.score * rolls;

                void MakeTurn(Player p)
                {
                    p.position += Roll();
                    p.position = (p.position - 1) % 10 + 1;
                    p.score += p.position;
                }

                int Roll()
                {
                    var value = (3*die + 6);
                    die = (die+3)%100;
                    rolls += 3;

                    return value;
                }
            }
        }

        public record P(bool turnP1, int score1, int score2, int pos1, int pos2);

        public static (int, int)[] rolls = {(3, 1), (4, 3), (5, 6), (6, 7), (7, 6), (8, 3), (9, 1)};

        public static long TaskB(int p1, int p2)
        {
            var first = new P(true, 0, 0, p1, p2);
            var result = CalculateUniverse(first);
            return Math.Max(result.Item1, result.Item2);
        }

        public static (long, long) CalculateUniverse(P p)
        {
            if (p.score1 >= 21)
                return(1, 0);

            if (p.score2 >= 21)
                return(0, 1);

            long win1 = 0;
            long win2 = 0;

            foreach (var (val, number) in rolls)
            {
                if (p.turnP1)
                {
                    var p1 = (p.pos1 + val - 1) % 10 + 1;
                    var s1 = p.score1 + p1;
                    var newP = new P(!p.turnP1, s1, p.score2, p1, p.pos2);
                    var x = CalculateUniverse(newP);
                    win1 += x.Item1 * number;
                    win2 += x.Item2 * number;
                }
                else
                {
                    var p2 = (p.pos2 + val - 1) % 10 + 1;
                    var s2 = p.score2 + p2;
                    var newP = new P(!p.turnP1, p.score1, s2, p.pos1, p2);
                    var x = CalculateUniverse(newP);
                    win1 += x.Item1 * number;
                    win2 += x.Item2 * number;
                }
            }

            return (win1, win2);
        }
    }
}