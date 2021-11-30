using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2020.Week3
{
    public class Tile
    {
        public Tile(string data)
        {
            var splitData = data.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            Name = int.Parse(splitData[0].Substring(5, 4));
            Len = splitData[1].Length;
            var newArray = new char[Len, Len];
            for (int i = 1; i < splitData.Length; i++)
                for (int j = 0; j < Len; j++)
                    newArray[i - 1, j] = splitData[i][j];
            ThisTile = newArray;
        }

        public int Name { get; }
        public char[,] ThisTile { get; set; }

        public int Len;

        public void TurnRight()
        {
            var temp = new char[Len, Len];
            for (int x = 0; x < Len; x++)
                for (int y = 0; y < Len; y++)
                    temp[x, Len - 1 - y] = ThisTile[y, x];
            ThisTile = temp;
        }

        public void MirrorImage()
        {
            var temp = new char[Len, Len];
            for (int x = 0; x < Len; x++)
                for (int y = 0; y < Len; y++)
                    temp[y, Len - 1 - x] = ThisTile[y, x];
            ThisTile = temp;
        }

        public string GetSide(int sideNr) // 1 U / 2 R / -1 D / -2 L 
        {
            switch (sideNr)
            {
                case 1:
                    string side = "";
                    for (int i = 0; i < Len; i++)
                        side += ThisTile[0, i];
                    return side;
                case -1:
                    side = "";
                    for (int i = 0; i < Len; i++)
                        side += ThisTile[Len - 1, i];
                    return side;
                case 2:
                    side = "";
                    for (int i = 0; i < Len; i++)
                        side += ThisTile[i, Len - 1];
                    return side;
                case -2:
                    side = "";
                    for (int i = 0; i < Len; i++)
                        side += ThisTile[i, 0];
                    return side;
                default:
                    return "";
            }
        }

        public char GetPoint(int Y, int X)
        {
            return ThisTile[Y, X];
        }

        public void RemoveBorders()
        {
            var temp = new char[Len-2, Len-2];
            for (int x = 1; x < Len-1; x++)
                for (int y = 1; y < Len-1; y++)
                    temp[y-1, x-1] = ThisTile[y, x];
            ThisTile = temp;
            Len -= 2;
        }
    }

    public class Day20
    {
        public static void Execute()
        {
            var tiles = File.ReadAllText(@"Week3\input20.txt").Split("\r\n\r\n").Select(x => new Tile(x)).ToList();

            (long, int) results = Task(tiles);

            long resultA = results.Item1;
            int resultB = results.Item2;

            Console.WriteLine(resultA);
            Console.WriteLine(resultB);
        }

        private static (long, int) Task(List<Tile> tiles)
        {
            // Task A //
            int gridSize = (int)Math.Sqrt(tiles.Count);
            var grid = new Dictionary<(int, int), Tile>();

            var firstCornerTile = tiles[FindCorner(tiles)];
            tiles.Remove(firstCornerTile);
            TryRotateCorner(firstCornerTile, tiles);
            grid.Add((0, 0), firstCornerTile);

            for (int y = 1; y < gridSize; y++)  // make 1th column in grid
            {
                var nTile = FindNeighborOnThatSide(tiles, grid[(y - 1, 0)], 1);
                grid.Add((y, 0), nTile);
                tiles.Remove(nTile);
            }

            for (int y = 0; y < gridSize; y++) // make rest of the grid
                for (int x = 1; x < gridSize; x++)
                {
                    var nTile = FindNeighborOnThatSide(tiles, grid[(y, x - 1)], 2);
                    grid.Add((y, x), nTile);
                    tiles.Remove(nTile);
                }

            long resultA = (long)grid[(0, 0)].Name * grid[(0, gridSize - 1)].Name * grid[(gridSize - 1, 0)].Name * grid[(gridSize - 1, gridSize - 1)].Name;

            // Task B //

            foreach (var tile in grid.Values)
                tile.RemoveBorders();

            int tileWidth = grid[(0, 0)].Len;

            var map = new char[gridSize * tileWidth, gridSize * tileWidth];
            foreach(var gridPos in grid)   // create full map from tiles
            {
                int pY = tileWidth * gridPos.Key.Item1;
                int pX = tileWidth * gridPos.Key.Item2;

                for (int y = 0; y < tileWidth; y++)
                    for (int x = 0; x < tileWidth; x++)
                        map[pY + tileWidth - y - 1, pX + x] = gridPos.Value.GetPoint(y, x);
            }

            for (int j = 0; j < 8; j++) // try all rotations
            {
                int monsters = 0;
                for (int y = 0; y < gridSize * tileWidth - 2; y++)
                    for (int x = 0; x < gridSize*tileWidth - 19; x++)
                    {

                        if (HaveMonster(map, (y, x)))
                            monsters++;
                    }

                if (monsters > 0)
                    break;

                TurnRightMap();
                if (j == 3 || j == 7)
                    MirrorImageMap();
            }

            // 💦🐍 RESULT VISUALISATION 🐍💦
            //VisualisationResult(map, gridSize * tileWidth);
               
            int resultB = 0;
            for (int y = 0; y < gridSize * tileWidth; y++)
                for (int x = 0; x < gridSize * tileWidth; x++)
                    if (map[y, x] == '#')
                        resultB++;
           return (resultA, resultB);


            void TurnRightMap()
            {
                var temp = new char[gridSize*tileWidth, gridSize*tileWidth];
                for (int x = 0; x < gridSize*tileWidth; x++)
                    for (int y = 0; y < gridSize*tileWidth; y++)
                        temp[x, gridSize*tileWidth - 1 - y] = map[y, x];
                map = temp;
            }

            void MirrorImageMap()
            {
                var temp = new char[gridSize*tileWidth, gridSize*tileWidth];
                for (int x = 0; x < gridSize * tileWidth; x++)
                    for (int y = 0; y < gridSize * tileWidth; y++)
                        temp[y, gridSize * tileWidth - 1 - x] = map[y, x];
                map = temp;
            }
        }

        static void VisualisationResult(char[,] map, int width)
        {
            for (int i = width - 1; i >= 0; i--)
            {
                for (int j = 0; j < width; j++)
                    if (map[i, j] == 'O')
                        Console.Write("O");
                    else
                        Console.Write(" ");
                Console.WriteLine();
            }
        }

        static bool HaveMonster(char[,] map, (int y, int x)poz)
        {
            var monster = new (int, int)[] { (0, 1), (0,4), (0,7), (0, 10), (0, 13), (0, 16), (1, 0), (1,5), (1,6), (1, 11), (1, 12), (1, 17), (1, 18), (1, 19), (2, 18) };

            foreach (var m in monster)
                if(map[poz.y + m.Item1, poz.x + m.Item2] != '#')
                    return false;

            foreach (var m in monster)
                map[poz.y + m.Item1, poz.x + m.Item2] = 'O';

            return true;
        }

        static int FindCorner(List<Tile> tiles)
        {
            var sideDir = new int[] { 1, 2, -1, -2 };
            for (var tileIdx = 0; tileIdx < tiles.Count; tileIdx++)
            {
                int neighbors = 0;
                for (int s = 0; s < 4; s++)
                {
                    if (IsNeighborOnThatSide(tiles, tiles[tileIdx], sideDir[s]))
                        neighbors++;
                }
                if (neighbors == 2)
                    return tileIdx;
            }
            return -1;
        }

        static void TryRotateCorner(Tile tile, List<Tile> tiles)
        {
            bool D = IsNeighborOnThatSide(tiles, tile, -1);
            bool L = IsNeighborOnThatSide(tiles, tile, -2);
            int rotate = 0;
            if (D && L)
                rotate = 2;
            else if (D && !L)
                rotate = 3;
            else if (!D && L)
                rotate = 1;

            for (int r = 0; r < rotate; r++)
                tile.TurnRight();
        }

        static bool IsNeighborOnThatSide(List<Tile> tiles, Tile tile, int sideDir)
        {
            bool have = false;
            string side = tile.GetSide(sideDir);
            foreach (var nTile in tiles)
            {
                if (nTile == tile)
                    continue;
                for (int j = 0; j < 8; j++)
                {
                    if (side == nTile.GetSide(sideDir * -1))
                        have = true;

                    nTile.TurnRight();
                    if (j == 3 || j == 7)
                        nTile.MirrorImage();

                    if (have)
                        return true;
                }
            }
            return false;
        }

        static Tile FindNeighborOnThatSide(List<Tile> tiles, Tile tile, int sideDir)
        {
            string side = tile.GetSide(sideDir);
            foreach (var nTile in tiles)
            {
                if (nTile == tile)
                    continue;
                for (int j = 0; j < 8; j++)
                {
                    if (side == nTile.GetSide(sideDir * -1))
                        return nTile;

                    nTile.TurnRight();
                    if (j == 3 || j == 7)
                        nTile.MirrorImage();
                }
            }
            return null;
        }

    }
}