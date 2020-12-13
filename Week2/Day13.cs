using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2020.Week2
{
    public class Day13
    {
        public static void Execute()
        {
            var data = File.ReadAllLines(@"Week2\input13.txt");
            var allBuses = data[1].Split(',');
            int time = int.Parse(data[0]);
            List<int> onTimebBuses = new List<int>();
            foreach (var b in allBuses)
                if (b != "x")
                    onTimebBuses.Add(int.Parse(b));

            int resultA = TaskA(onTimebBuses, time);
            long resultB = TaskB(allBuses);

            Console.WriteLine(resultA);
            Console.WriteLine(resultB);
        }

        private static int TaskA(List<int> buses, int time)
        {
            int minDiff = 1000000;
            int minBus = -1;
            foreach (var bus in buses)
            {
                int diff;
                if ((1.0 * time / bus) % 1 == 0)
                    diff = 0;
                else
                    diff = (time / bus + 1) * bus - time;

                if (diff < minDiff)
                {
                    minDiff = diff;
                    minBus = bus;
                }
            }
            return minDiff * minBus;
        }

        private static long TaskB(string[] buses)
        {
            var equationsData = new List<(long mod, long rest)>();
            long N = 1;
            for (int i = 0; i < buses.Length; i++)
            {
                if (buses[i] == "x")
                    continue;
                int m = int.Parse(buses[i]);
                int r = m - i;
                if (r == m)
                    r = 0;
                while (r < 0)
                    r += m;
                equationsData.Add((m, r));
                N *= m;
            }

            //Chinese Remainder Theorem https://www.youtube.com/watch?v=zIFehsBHB8o
            long result = 0;
            foreach (var x in equationsData)
            {
                long ri = x.rest;
                long ni = N / x.mod;
                long a = ni % x.mod;
                long xi = 1;
                if (a > 1)
                {
                    while (xi % a != 0)
                        xi += x.mod;
                    xi /= a;
                }
                result += (ri * ni * xi);
            }
            return result % N;
        }

    }
}