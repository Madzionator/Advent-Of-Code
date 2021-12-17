using System;
using System.Diagnostics;
using Advent._2021.Week1;
using Advent._2021.Week2;
using Advent._2021.Week3;

namespace Advent._2021
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Stopwatch();
            s.Start();

            Day17.Execute();

            s.Stop();
            Console.WriteLine($"\n\nTime Elapsed: {s.ElapsedMilliseconds}ms");
        }
    }
}