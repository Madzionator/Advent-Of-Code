using Advent._2022.Day;

namespace Advent._2022.Week2;

class Day14 : IDay
{
    public void Execute()
    {
        var input = File.ReadAllLines(@"Week2\input14.txt").Select(x => x.Split(" -> ").ToArray()).ToList();
        var map = CreateMap(input);

        var borders = (map.Keys.MinBy(c => c.x).x, map.Keys.MaxBy(c => c.x).x,map.Keys.MinBy(c => c.y).y, map.Keys.MaxBy(c => c.y).y);

        Console.WriteLine(Task(map.ToDictionary(x => x.Key, x => x.Value), borders, true));
        Console.WriteLine(Task(map, borders, false));
    }

    private int Task(Dictionary<(int x, int y), char> map, (int minX, int maxX, int minY, int maxY) borders, bool isTaskA)
    {
        var sandUnit = 0;
        while (true)
        {
            sandUnit++;
            int x = 500, y = 0;

            while (true)
            {
                if (isTaskA)
                {
                    if (x < borders.minX || x > borders.maxX || y > borders.maxY)
                    {
                        // map.DrawMap(borders.minX, borders.maxX, borders.minY, borders.maxY);
                        return sandUnit - 1;
                    }
                }
                else
                {
                    if (y == borders.maxY + 1) //floor
                    {
                        map.Add((x, y), '.');
                        break;
                    }
                }

                if (!map.ContainsKey((x, y + 1))) //down
                {
                    y++;
                    continue;
                }

                if (!map.ContainsKey((x - 1, y + 1))) //down-left
                {
                    y++;
                    x--;
                    continue;
                }

                if (!map.ContainsKey((x + 1, y + 1))) //down-right
                {
                    y++;
                    x++;
                    continue;
                }

                if (y == 0 && x == 500) //top
                {
                    map.TryAdd((x, y), '.');
                    //map.DrawMap(borders.minX, borders.maxX, borders.minY, borders.maxY+2);
                    return sandUnit;
                }

                map.TryAdd((x, y), '.');  //stuck
                break;
            }
        }
    }

    private Dictionary<(int x, int y), char> CreateMap(List<string[]>input)
    {
        var map = new Dictionary<(int x, int y), char>();

        foreach (var line in input)
        {
            var rocks = line.Select(x => x.Split(',').Select(int.Parse).ToArray()).ToList();
            var start = (rocks[0][0], rocks[0][1]);
            var end = start;

            for (var i = 0; i < rocks.Count - 1; i++)
            {
                end = (rocks[i + 1][0], rocks[i + 1][1]);
                var dx = end.Item1 - start.Item1 > 0 ? 1 : end.Item1 - start.Item1 < 0 ? -1 : 0;
                var dy = end.Item2 - start.Item2 > 0 ? 1 : end.Item2 - start.Item2 < 0 ? -1 : 0;
                while (start != end)
                {
                    map.TryAdd((start.Item1, start.Item2), '#');
                    start = (start.Item1 + dx, start.Item2 + dy);
                }
            }
            map.TryAdd((start.Item1, start.Item2), '#');
        }

        return map;
    }
}