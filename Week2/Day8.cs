using System;
using System.Collections.Generic;
using System.IO;

namespace Advent._2020.Week2
{
    public class Day8
    {
        public static void Execute()
        {
            var lines = File.ReadAllLines(@"Week2\input8.txt");
            var commandList = new List<(string, int)>();
            foreach (var line in lines)
            {
                var command = line.Split(' ');
                commandList.Add((command[0], int.Parse(command[1])));
            }
            var commandArray = commandList.ToArray();

            int resultA = Task(commandArray).Item1;
            int resultB = TaskB(commandArray);

            Console.WriteLine(resultA);
            Console.WriteLine(resultB);
        }

        private static (int, bool) Task((string, int)[] commandArray)
        {
            var array = new bool[commandArray.Length];
            int result = 0;
            int i = 0;

            while (i < commandArray.Length && array[i] == false)
            {
                array[i] = true;
                switch (commandArray[i].Item1)
                {
                    case "acc":
                        result += commandArray[i].Item2;
                        break;
                    case "jmp":
                        i += commandArray[i].Item2 - 1;
                        break;
                }
                i++;
            }

            return (result, (i >= commandArray.Length));
        }

        private static int TaskB((string, int)[] commandArray)
        {
            int result = 0;
            for(int i = 0; i<commandArray.Length; i++)
            {
                if(commandArray[i].Item1 == "nop")
                { 
                    commandArray[i].Item1 = "jmp";
                    var (res, finished) = Task(commandArray);
                    if (finished)
                    {
                        result = res;
                        break;
                    }
                    else
                        commandArray[i].Item1 = "nop";
                }
                else if(commandArray[i].Item1 == "jmp")
                {
                    commandArray[i].Item1 = "nop";
                    var (res, finished) = Task(commandArray);
                    if (finished)
                    {
                        result = res;
                        break;
                    }
                    else
                        commandArray[i].Item1 = "jmp";
                }
            }

            return result;
        }
    }
}
