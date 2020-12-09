using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2020.Week2
{
    public class Day8
    {
        public static void Execute()
        {
            var commands = File.ReadAllLines(@"Week2\input8.txt")
               .Select(lines =>
               {
                   var a = lines.Split(' ');
                   return (a[0], int.Parse(a[1]));
               })
               .ToArray();

            int resultA = Task(commands).Item1;
            int resultB = TaskB(commands);

            Console.WriteLine(resultA);
            Console.WriteLine(resultB);
        }

        private static (int, bool) Task((string cmd, int arg)[] commands)
        {
            var used = new bool[commands.Length];
            int accumulator = 0;
            int i = 0;

            while (i < commands.Length && !used[i])
            {
                used[i] = true;
                switch (commands[i].cmd)
                {
                    case "acc":
                        accumulator += commands[i].arg;
                        break;
                    case "jmp":
                        i += commands[i].arg - 1;
                        break;
                }
                i++;
            }

            return (accumulator, i >= commands.Length);
        }

        private static int TaskB((string cmd, int arg)[] commands)
        {
            for (int i = 0; i < commands.Length; i++)
                switch (commands[i].cmd)
                {
                    case "nop":
                        commands[i].cmd = "jmp";
                        var (accumulator, finished) = Task(commands);
                        if (finished)
                            return accumulator;
                        commands[i].cmd = "nop";
                        break;

                    case "jmp":
                        commands[i].cmd = "nop";
                        (accumulator, finished) = Task(commands);
                        if (finished)
                            return accumulator;
                        commands[i].cmd = "jmp";
                        break;
                }
            return 0;
        }
    }
}
