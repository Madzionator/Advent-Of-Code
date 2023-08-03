using System.Collections;
using Advent._2022.Day;

namespace Advent._2022.Week3;

class Day19 : IDay
{
    public record Blueprint(int Idx, int OreRCInOre, int ClayRCInOre, int ObsRCInOre, int ObsRCInClay, int GeodRCInOre, int GeodRCInObs); //RCIn - robot cost in

    public void Execute()
    {
        var input = File.ReadAllLines(@"Week3\input19.txt")
            .Select(x => x.ExtractNumbers())
            .Select(x => new Blueprint(x[0], x[1], x[2], x[3], x[4], x[5], x[6]))
            .ToArray();

        Console.WriteLine($"A: {TaskB(input)}");
        //Console.WriteLine($"B: {TaskA(input)}");
    }

    private int TaskA(Blueprint[] blueprints)
    {
        List<int> results = new();
        int localBest = 0;

        foreach (var blue in blueprints)
        {
            var amountOfrobots = new[] { 1, 0, 0, 0 }; //0 - oreCollector, 1 - clayCollector, 2 - obsidianCollector, 3 - geodeCollector
            var amountOfResources = new[] { 0, 0, 0, 0 }; // 0 - ore, 1 - clay, 2 - obsidian, 3 - geode

            Production(amountOfrobots, amountOfResources, 0, blue);
            results.Add(localBest * blue.Idx);
            localBest = 0;
        }

        return results.Sum();

        void Production(int[] robots, int[] resources, int time, Blueprint blue)
        {
            if (resources[3] > localBest)
                localBest = resources[3];

            if (time >= 24)
                return;

            for (var i = 0; i < 5; i++)
            {
                var newRes = resources;
                if (i < 4)
                {
                    (newRes, bool canProduce) = TryProduceRobot(robots, newRes, i, blue);
                    if (!canProduce)
                        continue;
                }

                newRes = ProduceResources(robots, newRes);
                if (i == 4)
                {
                    Production(robots, newRes, time + 1, blue);
                }
                else
                {
                    var newRob = robots.ToArray();
                    newRob[i] += 1;
                    Production(newRob, newRes, time + 1, blue);
                }

            }
        }

        int[] ProduceResources(int[] rob, int[] res)
        {
            var newRes = res.ToArray();
            for (int i = 0; i < 4; i++)
            {
                newRes[i] += rob[i];
            }

            return newRes;
        }

        (int[], bool) TryProduceRobot(int[] rob, int[] res, int robNum, Blueprint blueprint)
        {
            var resCopy = res.ToArray();
            switch (robNum)
            {
                case 0:
                    if (res[0] < blueprint.OreRCInOre)
                        break;
                    resCopy[0] -= blueprint.OreRCInOre;
                    return (resCopy, true);

                case 1:
                    if (res[0] < blueprint.ClayRCInOre)
                        break;
                    resCopy[0] -= blueprint.ClayRCInOre;
                    return (resCopy, true);

                case 2:
                    if (res[0] < blueprint.ObsRCInOre || res[1] < blueprint.ObsRCInClay)
                        break;
                    resCopy[0] -= blueprint.ObsRCInOre;
                    resCopy[1] -= blueprint.ObsRCInClay;
                    return (resCopy, true);

                case 3:
                    if (res[0] < blueprint.GeodRCInOre || res[2] < blueprint.GeodRCInObs)
                        break;
                    resCopy[0] -= blueprint.GeodRCInOre;
                    resCopy[2] -= blueprint.GeodRCInObs;
                    return (resCopy, true);
            }

            return (res, false);
        }
    }

    private long TaskB(Blueprint[] blueprints)
    {
        List<int> results = new();

        foreach (var blue in blueprints.Take(3))
        {
            var amountOfrobots = new[] { 1, 0, 0, 0 }; //0 - oreCollector, 1 - clayCollector, 2 - obsidianCollector, 3 - geodeCollector
            var amountOfResources = new[] { 0, 0, 0, 0 }; // 0 - ore, 1 - clay, 2 - obsidian, 3 - geode

            int localBest = Enumerable.Range(0, 10000000).AsParallel().Select(x =>
                Production(amountOfrobots.ToArray(), amountOfResources.ToArray(), 32, blue, new Random(x + 1000000))).Max();

            results.Add(localBest);
        }

        return results.Multiply();
    }

    int Production(int[] robots, int[] resources, int time, Blueprint blueprint, Random r)
    {
        for (int j = 0; j < time; j++)
        {
            var xx = r.Next(0, 100);
            if (xx < 95)
            {
                if (resources[0] >= blueprint.GeodRCInOre && resources[2] >= blueprint.GeodRCInObs)
                {
                    var x = r.Next(0, 100);
                    if (x < 90)
                    {
                        ProduceRobot(robots, resources, 3, blueprint);
                        ProduceResources(robots, resources);
                        robots[3]++;
                        continue;
                    }
                }

                if (resources[0] >= blueprint.ObsRCInOre && resources[1] >= blueprint.ObsRCInClay)
                {
                    var x = r.Next(0, 100);
                    if (x < 50)
                    {
                        ProduceRobot(robots, resources, 2, blueprint);
                        ProduceResources(robots, resources);
                        robots[2]++;
                        continue;
                    }
                }

                if (resources[0] >= blueprint.ClayRCInOre)
                {
                    var x = r.Next(0, 100);
                    if (x < 30)
                    {
                        ProduceRobot(robots, resources, 1, blueprint);
                        ProduceResources(robots, resources);
                        robots[1]++;
                        continue;
                    }
                }

                if (resources[0] >= blueprint.OreRCInOre)
                {
                    //var x = r.Next(0, 100);
                    //if (x < 40)
                    {
                        ProduceRobot(robots, resources, 0, blueprint);
                        ProduceResources(robots, resources);
                        robots[0]++;
                        continue;
                    }
                }
            }

            ProduceResources(robots, resources);
        }

        return resources[3];
    }

    void ProduceResources(int[] rob, int[] res)
    {
        for (int i = 0; i < 4; i++)
        {
            res[i] += rob[i];
        }
    }

    void ProduceRobot(int[] rob, int[] res, int robNum, Blueprint blueprint)
    {
        switch (robNum)
        {
            case 0:
                res[0] -= blueprint.OreRCInOre;
                return;
            case 1:
                res[0] -= blueprint.ClayRCInOre;
                return;
            case 2:
                res[0] -= blueprint.ObsRCInOre;
                res[1] -= blueprint.ObsRCInClay;
                return;
            case 3:
                res[0] -= blueprint.GeodRCInOre;
                res[2] -= blueprint.GeodRCInObs;
                return;
        }
    }
}