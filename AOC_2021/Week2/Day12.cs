using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2021.Week2
{
    class Day12
    {
        private static Dictionary<string, List<string>> paths;
        private static int fullPathCounter;

        public static void Execute()
        {
            var connections = File.ReadAllLines(@"Week2\input12.txt").Select(x => x.Split("-")).ToArray();
            paths = new Dictionary<string, List<string>>();

            foreach (var path in connections)
                for (int p = 0; p < 2; p++)
                {
                    if (!paths.ContainsKey(path[p]))
                        paths[path[p]] = new();

                    paths[path[p]].Add(path[(p+1)%2]);
                }

            foreach (var (k, v) in paths)
                v.Remove("start");

            Console.WriteLine(Task(false));
            Console.WriteLine(Task(true));
        }

        public static int Task(bool allowTwice)
        {
            fullPathCounter = 0;
            CountFullPath("start", new List<string>(), !allowTwice);
            return fullPathCounter;
        }

        private static void CountFullPath(string now, List<string> exceptions, bool wasTwice)
        {
            if (now == "end")
            {
                fullPathCounter++;
                return;
            }

            var currentExceptions = new List<string>(exceptions);
            if (now[0] >= 'a' && now[0] <= 'z')
                currentExceptions.Add(now);

            if (paths.ContainsKey(now))
                foreach (var path in paths[now])
                {
                    if (!currentExceptions.Contains(path))
                        CountFullPath(path, currentExceptions, wasTwice);
                    else if (!wasTwice)
                        CountFullPath(path, currentExceptions, true);
                }
        }
    }
}
