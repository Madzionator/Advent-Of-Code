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
            var commandArray = File.ReadAllLines(@"Week2\input8.txt")
               .Select(lines =>
               {
                   var a = lines.Split(' ');
                   return (a[0], int.Parse(a[1]));
               })
               .ToArray();

            int resultA = Task(commandArray).Item1;
            int resultB = TaskB(commandArray);

            Console.WriteLine(resultA);
            Console.WriteLine(resultB);
        }

        private static (int, bool) Task((string, int)[] commandArray)
        {
            var array = new bool[commandArray.Length];
            int accumulator = 0;
            int i = 0;

            while (i < commandArray.Length && array[i] == false)
            {
                array[i] = true;
                switch (commandArray[i].Item1)
                {
                    case "acc":
                        accumulator += commandArray[i].Item2;
                        break;
                    case "jmp":
                        i += commandArray[i].Item2 - 1;
                        break;
                }
                i++;
            }

            return (accumulator, (i >= commandArray.Length));
        }

        private static int TaskB((string, int)[] commandArray)
        {
            for (int i = 0; i < commandArray.Length; i++)
                switch (commandArray[i].Item1)
                {
                    case "nop":
                        commandArray[i].Item1 = "jmp";
                        var (result, finished) = Task(commandArray);
                        if (finished)
                            return result;
                        else
                            commandArray[i].Item1 = "nop";
                        break;
                    case "jmp":
                        commandArray[i].Item1 = "nop";
                        (result, finished) = Task(commandArray);
                        if (finished)
                            return result;
                        else
                            commandArray[i].Item1 = "jmp";
                        break;
                }
            return 0;
        }
    }
}
