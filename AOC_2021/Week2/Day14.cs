using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2021.Week2
{
    class Day14
    {
        public static void Execute()
        {
            var file = File.ReadAllLines(@"Week2\input14.txt");
            var firstTemplate = file[0];
            var productionRules = new Dictionary<string, char>();
            for (int i = 2; i < file.Length; i++)
            {
                var line = file[i].Split(" -> ");
                productionRules[line[0]] = line[1][0];
            }

            Console.WriteLine(Task(productionRules, firstTemplate, 10));
            Console.WriteLine(Task(productionRules, firstTemplate, 40));
        }

        public static long Task(Dictionary<string, char> productionRules, string firstTemplate, int steps)
        {
            var elements = new Dictionary<char, long>();
            foreach (var c in firstTemplate)
                elements[c] = elements.ContainsKey(c) ? ++elements[c] : 1;

            var created = new Dictionary<string, long>();
            for (int i = 1; i < firstTemplate.Length; i++)
                if (created.ContainsKey(firstTemplate[(i - 1)..(i + 1)]))
                    created[firstTemplate[(i - 1)..(i + 1)]]++;
                else
                    created[firstTemplate[(i - 1)..(i + 1)]] = 1;

            for (int i = 0; i < steps; i++)
            {
                var copy = new Dictionary<string, long>(created);
                foreach (var (k, v) in copy)
                {
                    var produced = productionRules[k];
                    var new1 = "" + k[0] + produced;
                    var new2 = "" + produced + k[1];
                    created[k] -= v;

                    created[new1] = created.ContainsKey(new1) ? created[new1] + v : v;
                    created[new2] = created.ContainsKey(new2) ? created[new2] + v : v;
                    elements[produced] = elements.ContainsKey(produced) ? elements[produced] + v : v;
                }
            }

            var min = elements.Min(x => x.Value);
            var max = elements.Max(x => x.Value);

            return max - min;
        }
    }
}
