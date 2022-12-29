using Advent._2022.Day;

namespace Advent._2022.Week3;

class Day16 : IDay
{
    private Dictionary<string, Valve> _valves { get; set; }
    private Dictionary<(string From, string To), int> _paths { get; set; }

    record Valve(string Name, int Pressure)
    {
        public List<Valve> Neighbors { get; } = new();
        public int Idx { get; set; }

        public void UpdateNeighborInfo(string descrip, int idx, Dictionary<string, Valve> valves)
        {
            Idx = idx;
            var neighbors = descrip.Replace(" ", "").Split(',');
            foreach (var n in neighbors)
            {
                Neighbors.Add(valves[n]);
            }
        }
    }

    public void Execute()
    {
        var input = File.ReadAllLines(@"Week3\input16.txt");
        _valves = input
            .Select(x => new Valve(x[6..8], int.Parse(x[24] == ';' ? x[23].ToString() : x[23..25])))
            .ToDictionary(x=> x.Name, x => x);

        for (var i = 0; i < input.Length; i++)
        {
            var val = _valves[input[i][6..8]];
            val.UpdateNeighborInfo(input[i][49..], i, _valves);
        }

        _paths = GetDistances(_valves);

        Console.WriteLine($"A: {TaskA()}");
        Console.WriteLine($"B: {TaskB()}"); //takes 1.3 min
    }

    private int TaskA() => Explore(_valves["AA"], _valves.Where(x=>x.Value.Pressure > 0).Select(x => x.Key).ToList(), 0, 0).pressure;

    private int TaskB()
    {
        var valvesToOpen = _valves.Where(x => x.Value.Pressure > 0).Select(x => x.Key).ToList();

        var max = 0;
        var combinations = valvesToOpen.Combinations(valvesToOpen.Count/2).Select(x => x.ToList());

        foreach (var combination in combinations)
        {
            var toOpen = valvesToOpen.Where(x => !combination.Contains(x)).ToList();
            var res = Explore(_valves["AA"], combination, 4, 0);

            toOpen.AddRange(res.noOpened);
            var resEleph = Explore(_valves["AA"], toOpen, 4, 0);
            max = Math.Max(max, res.pressure + resEleph.pressure);
        }

        return max;
    }

    (int pressure, List<string> noOpened) Explore(Valve curVal, List<string> openable, int time, int totalPress)
    {
        var newRes = (totalPress, openable);

        if (time > 30)
            return newRes;

        foreach (var valveName in openable)
        {
            var nextTime = time + _paths[(curVal.Name, valveName)] + 1;
            var nextTotalPress = totalPress + _valves[valveName].Pressure * (30 - nextTime);

            var x = Explore(_valves[valveName], openable.Where(x => x != valveName).ToList(), nextTime, nextTotalPress);
            newRes = x.Item1 > newRes.totalPress ? x : newRes;
        }

        return newRes;
    }

    private Dictionary<(string, string), int> GetDistances(Dictionary<string, Valve> valves)
    {
        var distances = new Dictionary<(string From, string To), int>();

        Valve from;
        Valve to;

        for (var i = 0; i < valves.Count; i++)
        {
            for (var j = i + 1; j < valves.Count; j++)
            {
                from = valves.ElementAt(i).Value;
                to = valves.ElementAt(j).Value;

                if((from.Pressure <= 0 && from.Name != "AA") || (to.Pressure <= 0 && to.Name != "AA"))
                    continue;

                FindGoal(from, to, 0, new bool[valves.Count], new List<Valve>() { from });
            }
        }

        var x = distances.Count;
        for (var i = 0; i < x; i++)
        {
            var e = distances.ElementAt(i);
            distances.Add((e.Key.To, e.Key.From), distances[e.Key]);
        }

        return distances;

        void FindGoal(Valve src, Valve dest, int cost, bool[] visited, List<Valve> path)
        {
            visited[src.Idx] = true;
            if (src == dest)
            {
                if (distances.TryGetValue((from.Name, to.Name), out int val))
                {
                    if (val > cost)
                        distances[(from.Name, to.Name)] = cost;
                }
                else
                {
                    distances.Add((from.Name, to.Name), cost);
                }

                return;
            }

            foreach (var n in src.Neighbors.Where(n => !visited[n.Idx]))
            {
                FindGoal(n, dest, cost + 1, visited.ToArray(), path.Append(n).ToList());
            }
        }
    }
}