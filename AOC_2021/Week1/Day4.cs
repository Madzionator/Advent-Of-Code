using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;

namespace Advent._2021.Week1
{
    public class Day4
    {
        public static void Execute()
        {
            var file = File.ReadAllLines(@"Week1\input4.txt");
            var drawnNumbers = file[0].Split(',').Select(int.Parse).ToArray();
            var boards = new List<Bingo>();
            for (int i = 2; i < file.Length; i += 6)
            {
                var board = new Bingo(file[i..(i+5)]);
                boards.Add(board);
            }

            Console.WriteLine(TaskA(boards, drawnNumbers));
            Console.WriteLine(TaskB(boards, drawnNumbers));
        }

        public static int TaskA(List<Bingo> boards, int[] drawnNumbers)
        {
            foreach (int drawn in drawnNumbers)
            {
                foreach (Bingo board in boards)
                {
                    board.Mark(drawn);
                    if (board.IsBingo())
                        return board.SumUnmarked() * drawn;
                }
            }

            return -1;
        }

        public static int TaskB(List<Bingo> boards, int[] drawnNumbers)
        {
            int boardsLeft = boards.Count;
            foreach (int drawn in drawnNumbers)
                foreach (Bingo board in boards)
                {
                    board.Mark(drawn);

                    if (board.IsBingo() && !board.HasBingo)
                    {
                        board.HasBingo = true;
                        boardsLeft--;
                    }

                    if(boardsLeft == 0)
                        return board.SumUnmarked() * drawn;
                }

            return -1;
        }

        public class Bingo
        {
            private const int ZERO = 2137;
            public int[,] board = new int[5, 5];
            public bool HasBingo = false;
            public Bingo(string[] strValues)
            {
                for (int i = 0; i < 5; i++)
                {
                    var values = strValues[i].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    for (int j = 0; j < 5; j++)
                    {
                        board[i, j] = int.Parse(values[j]);
                        if (board[i, j] == 0)
                            board[i, j] = ZERO;
                    }
                }
            }

            public void Mark(int number)
            {
                if (number == 0)
                    number = ZERO;

                for (int i = 0; i < 5; i++)
                    for (int j = 0; j < 5; j++)
                        if (board[i, j] == number)
                        {
                            board[i, j] *= (-1);
                            return;
                        }
            }

            public bool IsBingo()
            {
                for (int i = 0; i < 5; i++)
                {
                    int bingoY = 0, bingoX = 0;
                    for (var j = 0; j < 5; j++)
                    {
                        bingoY += board[i, j] < 0 ? 1 : 0;
                        bingoX += board[j, i] < 0 ? 1 : 0;
                    }
                    if (bingoY == 5 || bingoX == 5)
                        return true;
                }

                return false;
            }

            public int SumUnmarked() => (from int val in board where val >= 0 where val != ZERO select val).Sum();
        }
    }
}
