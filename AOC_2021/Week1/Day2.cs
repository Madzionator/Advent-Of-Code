using System;
using System.IO;
using System.Linq;

namespace Advent._2021.Week1
{
    class Day2
    {
        public static void Execute()
        {
            var commands = File.ReadAllLines(@"Week1\input2.txt")
                 .Select(x =>
                 {
                     var values = x.Split(" ");
                     return (values[0], int.Parse(values[1]));
                 })
                 .ToArray();

            Console.WriteLine(TaskA(commands));
            Console.WriteLine(TaskB(commands));
        }

        public static int TaskA((string, int)[] commands)
        {
            int horizontalPosition = 0, depth = 0;

            foreach ((string direction, int value) in commands)
            {
                switch (direction)
                {
                    case "forward":
                        horizontalPosition += value;
                        break;
                    case "up":
                        depth -= value;
                        break;
                    case "down":
                        depth += value;
                        break;
                }
            }

            return horizontalPosition * depth;
        }

        public static int TaskB((string, int)[] commands)
        {
            int horizontalPosition = 0, depth = 0, aim = 0;

            foreach ((string direction, int value) in commands)
            {
                switch (direction)
                {
                    case "forward":
                        horizontalPosition += value;
                        depth += aim * value;
                        break;
                    case "up":
                        aim -= value;
                        break;
                    case "down":
                        aim += value;
                        break;
                }
            }

            return horizontalPosition * depth;
        }
    }
}
