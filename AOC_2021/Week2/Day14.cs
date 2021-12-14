using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent.Helpers.Extensions;

namespace Advent._2021.Week2
{
    class Day14
    {
        public static void Execute()
        {
            var file = File.ReadAllLines(@"Week2\input14.txt");
            var firstTemplate = file[0];

            var productionRules = file[2..]
                .Select(x => x.Split(" -> "))
                .ToDictionary(x => x[0], x => x[1][0]);

            Console.WriteLine(Task(productionRules, firstTemplate, 10));
            Console.WriteLine(Task(productionRules, firstTemplate, 40));
        }

        public static long Task(Dictionary<string, char> productionRules, string template, int steps)
        {
            var elements = new Dictionary<char, long>();
            foreach (var c in template)
                elements.AddOrSet(c, 1);

            var created = new Dictionary<string, long>();
            for (var i = 1; i < template.Length; i++)
                created.AddOrSet(template[(i - 1)..(i + 1)], 1);

            for (var i = 0; i < steps; i++)
            {
                var copy = new Dictionary<string, long>(created);

                foreach (var (key, val) in copy)
                {
                    var produced = productionRules[key];
                    created[key] -= val;

                    created.AddOrSet("" + key[0] + produced, val);
                    created.AddOrSet("" + produced + key[1], val);
                    elements.AddOrSet(produced, val);
                }
            }

            return elements.Max(x => x.Value) - elements.Min(x => x.Value);
        }
    }
}
