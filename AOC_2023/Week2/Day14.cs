using Advent._2023.Day;

namespace AdventOfCode2023.Week2;

class Day14 : IDay
{
    public void Execute()
    {
        var reflectorDish = File.ReadAllLines(@"Week2\input14.txt").ToCharMatrix();

        Console.WriteLine($"A: {TaskA(reflectorDish.CopyMatrix())}");
        Console.WriteLine($"B: {TaskB(reflectorDish)}");
    }

    int TaskA(char[,] reflectorDish)
    {
        TiltNorth(reflectorDish);
        return CountTotalLoad(reflectorDish);
    }

    int TaskB(char[,] reflectorDish)
    {
        var numberOfHistoryRecords = 300;
        var history = new List<char[,]>();
        
        for (var i = 1; i <= numberOfHistoryRecords; i++)
        { 
            SpinCycle(reflectorDish);
            history.Add(reflectorDish.CopyMatrix());
        }

        var searchedFor = history.Last();
        var nextI = -1;

        for (var i = numberOfHistoryRecords - 2; i >= 0; i--)
            if (IsEqual(searchedFor, history[i]))
            {
                var d = numberOfHistoryRecords - 1 - i;
                if (IsEqual(searchedFor, history[i - d]))
                {
                    var toSkip = (1_000_000_000 - numberOfHistoryRecords) / d * d;

                    nextI = toSkip + numberOfHistoryRecords + 1;
                    break;
                }
            }

        for (var i = nextI; i <= 1_000_000_000; i++)
            SpinCycle(reflectorDish);

        return CountTotalLoad(reflectorDish);
    }

    int CountTotalLoad(char[,] reflectorDish)
    {
        var totalLoad = 0;
        var rowQuantity = reflectorDish.GetLength(0);

        for (var x = 0; x < reflectorDish.GetLength(1); x++)
        for (var y = 0; y < rowQuantity; y++)
            if (reflectorDish[y, x] == 'O') 
                totalLoad += (rowQuantity - y);

        return totalLoad;
    }

    void SpinCycle(char[,] reflectorDish)
    {
        TiltNorth(reflectorDish);
        TiltWest(reflectorDish);
        TiltSouth(reflectorDish);
        TiltEast(reflectorDish);
    }

    void TiltNorth(char[,] reflectorDish)
    {
        for (var x = 0; x < reflectorDish.GetLength(1); x++)
            for (var y = 0; y < reflectorDish.GetLength(0); y++)
                if (reflectorDish[y, x] == 'O')
                {
                    var emptyY = -1;
                    var isSomeEmpty = false;
                    for (var y2 = y - 1; y2 >= -1; y2--)
                        if (y2 == -1 || reflectorDish[y2, x] != '.')
                        {
                            emptyY = y2 + 1;
                            isSomeEmpty = true;
                            break;
                        }

                    if (isSomeEmpty && emptyY != y)
                    {
                        reflectorDish[y, x] = '.';
                        reflectorDish[emptyY, x] = 'O';
                    }
                }
    }

    void TiltWest(char[,] reflectorDish)
    {
        for (var y = 0; y < reflectorDish.GetLength(0); y++)
            for (var x = 0; x < reflectorDish.GetLength(1); x++)
                if (reflectorDish[y, x] == 'O')
                {
                    var emptyX = -1;
                    var isSomeEmpty = false;
                    for (var x2 = x - 1; x2 >= -1; x2--)
                        if (x2 == -1 || reflectorDish[y, x2] != '.')
                        {
                            emptyX = x2 + 1;
                            isSomeEmpty = true;
                            break;
                        }

                    if (isSomeEmpty && emptyX != x)
                    {
                        reflectorDish[y, x] = '.';
                        reflectorDish[y, emptyX] = 'O';
                    }
                }
    }

    void TiltEast(char[,] reflectorDish)
    {
        for (var y = 0; y < reflectorDish.GetLength(0); y++)
            for (var x = reflectorDish.GetLength(1) - 1; x >= 0; x--)
                if (reflectorDish[y, x] == 'O')
                {
                    var emptyX = -1;
                    var isSomeEmpty = false;
                    for (var x2 = x + 1; x2 <= reflectorDish.GetLength(1); x2++)
                        if (x2 == reflectorDish.GetLength(1) || reflectorDish[y, x2] != '.')
                        {
                            emptyX = x2 - 1;
                            isSomeEmpty = true;
                            break;
                        }

                    if (isSomeEmpty && emptyX != x)
                    {
                        reflectorDish[y, x] = '.';
                        reflectorDish[y, emptyX] = 'O';
                    }
                }
    }

    void TiltSouth(char[,] reflectorDish)
    {
        for (var x = 0; x < reflectorDish.GetLength(1); x++)
            for (var y = reflectorDish.GetLength(0) - 1; y >= 0; y--)
                if (reflectorDish[y, x] == 'O')
                {
                    var emptyY = -1;
                    var isSomeEmpty = false;
                    for (var y2 = y + 1; y2 <= reflectorDish.GetLength(0); y2++)
                        if (y2 == reflectorDish.GetLength(0) || reflectorDish[y2, x] != '.')
                        {
                            emptyY = y2 - 1;
                            isSomeEmpty = true;
                            break;
                        }

                    if (isSomeEmpty && emptyY != y)
                    {
                        reflectorDish[y, x] = '.';
                        reflectorDish[emptyY, x] = 'O';
                    }
                }
    }

    bool IsEqual(char[,] tab1, char[,] tab2)
    {
        for (var y = 0; y < tab1.GetLength(0); y++)
        for (var x = 0; x < tab2.GetLength(1); x++)
            if (tab1[y, x] != tab2[y, x])
                return false;

        return true;
    }
}