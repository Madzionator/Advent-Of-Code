using Advent._2023.Day;

class Day5 : IDay
{
    private Almanac[] _almanacs;

    public void Execute()
    {
        var input = File.ReadAllText(@"Week1\input5.txt")
            .Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries).ToArray();

        var seeds = input[0].Split(": ")[1].Split(' ').Select(uint.Parse).ToArray();

        _almanacs = input[1..].Select(section =>
        {
            var lines = section.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)[1..];
            return new Almanac(lines.Select(line =>
            {
                var x = line.Split(' ');
                return new Transformation(uint.Parse(x[1]), uint.Parse(x[0]), uint.Parse(x[2]));
            }).OrderBy(x => x.Source).ToArray());

        }).ToArray();

        Console.WriteLine($"A: {TaskA(seeds)}");
        Console.WriteLine($"B: {TaskB(seeds)}");
    }

    record Transformation(uint Source, uint Destination, uint Length);

    record Almanac(Transformation[] Transformations)
    {
        public uint Transform(uint source)
        {
            foreach (var tr in Transformations)
            {
                if (tr.Source <= source && source < tr.Source + tr.Length)
                {
                    return tr.Destination - tr.Source + source;
                }
            }

            return source;
        }
    }

    uint TaskA(uint[] seeds)
    {
        uint minLoc = uint.MaxValue;

        foreach (uint seed in seeds)
        {
            uint loc = seed;
            for (var i = 0; i < 7; i++)
                loc = _almanacs[i].Transform(loc);

            if(loc < minLoc)
                minLoc = loc;
        }
        
        return minLoc;
    }

    uint TaskB(uint[] seeds)
    {
        uint minLoc = uint.MaxValue;

        Parallel.For(0, seeds.Length/2, j =>
        {
            Parallel.For(seeds[j+j], seeds[j+j] + seeds[j+j+1], p1 =>
            {
                uint loc = (uint)p1;
                for (var i = 0; i < 7; i++)
                    loc = _almanacs[i].Transform(loc);

                if (loc < minLoc)
                    minLoc = loc;

            });

        });

        return minLoc;
    }
}