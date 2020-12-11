using System;
using System.IO;

namespace Advent._2020.Week2
{
    public class Day11
    {
        public static void Execute()
        {
            var data = ReadData();

            int resultA = Task(data, 'A');
            int resultB = Task(data, 'B');

            Console.WriteLine(resultA);
            Console.WriteLine(resultB);
        }

        private static int column;
        private static int row;

        private static char[,] ReadData()
        {
            var data = File.ReadAllLines(@"Week2\input11.txt");
            column = data.Length;
            row = data[0].Length;
            char[,] cData = new char[data.Length, data[0].Length];
            for (int il = 0; il < data.Length; il++)
            {
                var changeLine = data[il];
                for (int i = 0; i < changeLine.Length; i++)
                    cData[il, i] = changeLine[i];
            }
            return cData;
        }

        private static int Task(char[,] data, char task)
        {
            char[,] clone = data.Clone() as char[,];
            while (true)
            {
                if (!Change(clone, task, '#'))
                    break;
                bool a = Change(clone, task, 'L');
            }
            int result = 0;
            foreach (var x in clone)
                if (x == '#')
                    result++;
            return result;
        }

        private static bool Change(char[,] clone, char task, char newChar)
        {
            char[,] temp = clone.Clone() as char[,];
            bool sthChange = false;
            for (int y = 0; y < column; y++)
            {
                for (int x = 0; x < row; x++)
                {
                    if (temp[y, x] == '.' || temp[y, x] == newChar)
                        continue;
                    if (task == 'A')
                        if (CountNeighbourhoodA(temp, x, y))
                        {
                            clone[y, x] = newChar;
                            sthChange = true;
                        }
                    if (task == 'B')
                        if (CountNeighbourhoodB(temp, x, y))
                        {
                            clone[y, x] = newChar;
                            sthChange = true;
                        }
                }
            }
            return sthChange;
        }

        private static bool CountNeighbourhoodA(char[,] temp, int x, int y)
        {
            int sum = 0;
            for (int i = x - 1; i <= x + 1; i++)
            {
                if (i < 0 || i >= row)
                    continue;
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (j < 0 || j >= column || (i == x && j == y))
                        continue;

                    if (temp[j, i] == '#')
                        sum++;
                }
            }

            if (temp[y, x] == 'L' && sum == 0)
                return true;
            if (temp[y, x] == '#' && sum >= 4)
                return true;
            return false;
        }

        private static bool CountNeighbourhoodB(char[,] temp, int x, int y)
        {
            int seeCorupted(int dx, int dy)
            {
                int ix = x + dx;
                int iy = y + dy;
                while (0 <= ix && ix < row && 0 <= iy && iy < column)
                {
                    if (temp[iy, ix] == '#')
                        return 1;
                    if (temp[iy, ix] == 'L')
                        return 0;

                    ix += dx;
                    iy += dy;
                }
                return 0;
            }

            int sum = seeCorupted(0, -1) + seeCorupted(1, -1) + seeCorupted(1, 0) + seeCorupted(1, 1)
                + seeCorupted(0, 1) + seeCorupted(-1, 1) + seeCorupted(-1, 0) + seeCorupted(-1, -1);

            if (temp[y, x] == 'L' && sum == 0)
                return true;
            if (temp[y, x] == '#' && sum >= 5)
                return true;
            return false;
        }
    }
}