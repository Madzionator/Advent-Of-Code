using System;
using System.Diagnostics;
using Advent._2020.Week1;
using Advent._2020.Week2;

namespace Advent._2020
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Stopwatch();
            s.Start();

            Day8.Execute();

            s.Stop();
            Console.WriteLine($"\n\nTime Elapsed: {s.ElapsedMilliseconds}ms");
        }
    }
}