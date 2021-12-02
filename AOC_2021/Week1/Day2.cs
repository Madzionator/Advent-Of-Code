using System;
using System.Collections.Generic;
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
                .ToList();

            Console.WriteLine(Task_A(commands));
            Console.WriteLine(Task_B(commands));
        }

        public static int Task_A(List<(string, int)> commands)
        {
            int horizontalPosition = 0;
            int depth = 0;

            foreach ((string direction, int value) command in commands)
            {
                switch (command.direction)
                {
                    case "forward":
                        horizontalPosition += command.value;
                        break;
                    case "up":
                        depth -= command.value;
                        break;
                    case "down":
                        depth += command.value;
                        break;
                }
            }
            return horizontalPosition * depth;
        }

        public static int Task_B(List<(string, int)> commands)
        {
            int horizontalPosition = 0, depth = 0, aim = 0;

            foreach ((string direction, int value) command in commands)
            {
                switch (command.direction)
                {
                    case "forward":
                        horizontalPosition += command.value;
                        depth += aim * command.value;
                        break;
                    case "up":
                        aim -= command.value;
                        break;
                    case "down":
                        aim += command.value;
                        break;
                }
            }
            return horizontalPosition * depth;
        }
    }
}
