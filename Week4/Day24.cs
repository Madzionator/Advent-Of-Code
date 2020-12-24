using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2020.Week4
{
    public class Day24
    {
        public static void Execute()
        {
            var data = File.ReadAllLines(@"Week4\input24.txt")
                .Select(line => line.Replace("nw", "nl ").Replace("sw", "sl ").Replace("ne", "nr ").Replace("se", "sr ").Replace("w", "w ").Replace("e", "e ")
                .Split(" ", StringSplitOptions.RemoveEmptyEntries))
                .ToList();

            var partResult = TaskA(data);
            int resultA = partResult.Item1;
            int resultB = TaskB(partResult.Item2);

            Console.WriteLine(resultA);
            Console.WriteLine(resultB);
        }

        private static (int, Dictionary<(int, int), int>) TaskA(List<string[]> data)
        {
            var tiles = new Dictionary<(int, int), int>(); // -1 white, 1 - black
            tiles.Add((0, 0), -1);
            foreach (var instruction in data)
            {
                (int x, int y) tile = (0, 0);
                foreach (var move in instruction)
                {
                    switch (move)
                    {
                        case "nr":
                            tile.y += 1;
                            break;
                        case "e":
                            tile.x += 1;
                            break;
                        case "sr":
                            tile = (tile.x + 1, tile.y - 1);
                            break;
                        case "sl":
                            tile.y -= 1;
                            break;
                        case "w":
                            tile.x -= 1;
                            break;
                        case "nl":
                            tile = (tile.x - 1, tile.y + 1);
                            break;
                    }
                }
                if (!tiles.ContainsKey(tile))
                    tiles.Add(tile, 1);
                else
                    tiles[tile] *= -1;
            }

            int result = (tiles.Where(color => color.Value == 1).ToArray()).Length;
            return (result, tiles);
        }

        private static int TaskB(Dictionary<(int, int), int> tiles)
        {
            var operations = new (int opX, int opY)[6] { (0, 1), (1, 0), (1, -1), (0, -1), (-1, 0), (-1, 1) };
            var newTiles = new Dictionary<(int, int), int>();

            for (int day = 1; day <= 100; day++)
            {
                newTiles.Clear();
                foreach (var tile in tiles.Where(x => x.Value == 1))
                    TryAdd(tile.Key, 1);

                foreach (var tile in tiles.Where(x => x.Value == 1))
                    foreach (var op in operations)
                        TryAdd((tile.Key.Item1 + op.opX, tile.Key.Item2 + op.opY), -1);

                tiles = new Dictionary<(int, int), int>(newTiles);
                foreach (var tile in tiles)
                {
                    int black = CountBlack(tile.Key);
                    if (tile.Value == 1 && (black == 0 || black > 2))
                        newTiles[tile.Key] = -1;
                    else if (tile.Value == -1 && black == 2)
                        newTiles[tile.Key] = 1;
                }

                tiles = new Dictionary<(int, int), int>(newTiles);
            }

            int result = (tiles.Where(color => color.Value == 1).ToArray()).Length;
            return result;

            void TryAdd((int, int) t, int color)
            {
                if (!newTiles.ContainsKey(t))
                    newTiles.Add(t, color);
            }

            int CountBlack((int x, int y) t)
            {
                int black = 0;
                foreach (var op in operations)
                    if (tiles.ContainsKey((t.x + op.opX, t.y + op.opY)) && tiles[(t.x + op.opX, t.y + op.opY)] == 1)
                        black++;
                return black;
            }
        }
    }
}