using Advent._2022.Day;

namespace Advent._2022.Week2;

class Day12 : IDay
{
    public void Execute()
    {
        var heightsMap = File.ReadAllLines(@"Week2\input12.txt")
            .Select(x => x
                .Select(x => x - 'a')
                .ToArray())
            .ToArray()
            .ToMatrix();

        var S = Find(heightsMap, 'S' - 'a');
        var E = Find(heightsMap, 'E' - 'a');
        heightsMap[S.Item1, S.Item2] = 'a' - 'a';
        heightsMap[E.Item1, E.Item2] = 'z' - 'a' + 1;

        Console.WriteLine(Task(heightsMap, S));
        Console.WriteLine(Task(heightsMap, S, true));
    }

    private (int, int) Find(int[,] input, int target)
    {
        for (int i = 0; i < input.GetLength(0); i++)
        for (var j = 0; j < input.GetLength(1); j++)
            if (input[i, j] == target)
                return (i, j);

        return (-1, -1);
    }

    private int Task(int[,] heightsMap, (int y, int x) start, bool isTaskB = false)
    {
        int m = heightsMap.GetLength(0), n = heightsMap.GetLength(1);

        var visited = new bool[m, n];
        visited[start.y, start.x] = true;

        if(isTaskB)
        for (int i = 0; i < m; i++)
        for (int j = 0; j < n; j++)
        {
            if (heightsMap[i, j] == 0)
                visited[i, j] = true;
        }

        var visitedUpdated = visited.CopyMatrix();

        for(var cost = 0;; cost++)
        {
            for (var y = 0; y < m; y++)
            for (var x = 0; x < n; x++)
                if (visited[y, x])
                {
                    if (heightsMap[y, x] == 'z' - 'a' + 1)
                        return cost;

                    if (y > 0) TryVisit((y, x), (y - 1, x));
                    if (y < m - 1) TryVisit((y, x), (y + 1, x));
                    if (x > 0) TryVisit((y, x), (y, x - 1));
                    if (x < n - 1) TryVisit((y, x), (y, x + 1));
                }

            visited = visitedUpdated.CopyMatrix();
        }

        void TryVisit((int y, int x) curPos, (int y, int x) neighbor)
        {
            if (!visited[neighbor.y, neighbor.x] && heightsMap[neighbor.y, neighbor.x] - 1 <= heightsMap[curPos.y, curPos.x])
                visitedUpdated[neighbor.y, neighbor.x] = true;
        }
    }
}