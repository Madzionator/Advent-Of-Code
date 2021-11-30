using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2020.Week4
{
    public class Day22
    {
        public static void Execute()
        {
            Queue<int> player1 = new Queue<int>();
            Queue<int> player2 = new Queue<int>();
            var data = File.ReadAllLines(@"Week4\input22.txt").ToList();
            int pl2Index = data.IndexOf("Player 2:");
            for (int i = 1; i < pl2Index - 1; i++)
                player1.Enqueue(int.Parse(data[i]));

            for (int i = pl2Index + 1; i < data.Count; i++)
                player2.Enqueue(int.Parse(data[i]));

            int resultB = GameB(player1.ToList(), player2.ToList(), 1);
            int resultA = GameA(player1, player2);

            Console.WriteLine(resultA);
            Console.WriteLine(resultB);
        }

        private static int GameA(Queue<int> player1, Queue<int> player2)
        {
            void PlayRound(Queue<int> pl1, Queue<int> pl2)
            {
                pl1.Enqueue(pl1.Peek());
                pl1.Enqueue(pl2.Peek());
                pl1.Dequeue();
                pl2.Dequeue();
            }

            while (player1.Count > 0 && player2.Count > 0)
                if (player1.Peek() > player2.Peek())
                    PlayRound(player1, player2);
                else
                    PlayRound(player2, player1);

            return player1.Count > 0 ? CountResult(player1.ToList()) : CountResult(player2.ToList());
        }

        private static int CountResult(List<int> pl)
        {
            int result = 0;
            for (int i = pl.Count; i > 0; i--)
                result += i * pl[pl.Count - i];
            return result;
        }

        private static int GameB(List<int> player1, List<int> player2, int game)
        {
            HashSet<string> previous = new HashSet<string>();

            while (player1.Count > 0 && player2.Count > 0)
            {
                string decks = $"{string.Join(" ", player1)}|{string.Join(" ", player2)}";
                if (previous.Contains(decks))
                    return 1;
                else
                    previous.Add(decks);
                if (player1.Count > player1[0] && player2.Count > player2[0])
                {
                    if (GameB(player1.GetRange(1, player1[0]), player2.GetRange(1, player2[0]), game + 1) == 1)
                        PlayRound(player1, player2);
                    else
                        PlayRound(player2, player1);
                }
                else
                {
                    if (player1[0] > player2[0])
                        PlayRound(player1, player2);
                    else
                        PlayRound(player2, player1);
                }
            }

            if (game > 1)
                return player1.Count > 0 ? 1 : 2;
            else
                return player1.Count > 0 ? CountResult(player1) : CountResult(player2);

            void PlayRound(List<int> pl1, List<int> pl2)
            {
                pl1.Add(pl1[0]);
                pl1.Add(pl2[0]);
                pl1.RemoveAt(0);
                pl2.RemoveAt(0);
            }
        }
    }
}