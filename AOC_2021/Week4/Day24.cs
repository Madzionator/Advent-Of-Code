using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2021.Week1
{
    class Day24
    {
        public static void Execute()
        {
            var instructions = File.ReadAllLines(@"Week4\input24.txt").Select(x => x.Split(' ')).ToList();

            //Todo: Console.WriteLine(TaskA(instructions)); // find the largest model number
            Console.WriteLine(TaskB(instructions)); // find the smallest model number
        }

        public static char[] TaskA(List<string[]> instructions)
        {
            var minBest = "11111111111111".ToCharArray();
            var min = "99999999999999".ToCharArray();

            for (var repeat = 0; repeat <= 10; repeat++)
            {
                var minZ = long.MaxValue;
                for (var i = 0; i <= 13; i++)
                {
                    var minZIdx = 0;
                    min = minBest.ToArray();
                  

                    for (var digit = 1; digit <= 9; digit++)
                    {
                        min[i] = (char)(digit + '0');
                        var result = RunMonad(instructions, min);
                        if (result.Item2 < minZ && min[i] > minBest[i])
                        {
                            minZIdx = digit;
                            minZ = result.Item2;
                            minBest = min.ToArray();
                            Console.WriteLine(minZ);
                        }
                    }

                    min[i] = (char)(minZIdx + '0');
                }
            }

            return min;
        }

        public static char[] TaskB(List<string[]> instructions)
        {
            //Doesn't work properly yet 😅
            var min = "11111111111111".ToCharArray();

            for (var repeat = 0; repeat <= 13; repeat++)
                for (var i = 0; i <= 13; i++)
                {
                    var minZIdx = 0;
                    var minZ = long.MaxValue;
                    for (var digit = 1; digit <= 9; digit++)
                    {
                        min[i] = (char)(digit + '0');
                        var result = RunMonad(instructions, min);
                        if (result.Item2 < minZ)
                        {
                            minZIdx = digit;
                            minZ = result.Item2;
                        }
                    }
                    min[i] = (char)(minZIdx + '0');
                }

            return min;
        }

        public static (bool, long) RunMonad(List<string[]> instructions, char[] inp)
        {
            var inputIdx = 0;
            int i, j;
            var ALU = new long[] { 0, 0, 0, 0 }; // w x y z


            foreach (var instr in instructions)
            {
                switch (instr[0])
                {
                    case "inp":
                        ALU[VariableIdx(instr[1])] = int.Parse(inp[inputIdx].ToString());
                        inputIdx++;
                        break;
                    case "add":
                        i = VariableIdx(instr[1]);
                        j = VariableIdx(instr[2]);
                        ALU[i] += j >= 0 ? ALU[j] : int.Parse(instr[2]);
                        break;
                    case "mul":
                        i = VariableIdx(instr[1]);
                        j = VariableIdx(instr[2]);
                        ALU[i] *= j >= 0 ? ALU[j] : int.Parse(instr[2]);
                        break;
                    case "div":
                        i = VariableIdx(instr[1]);
                        j = VariableIdx(instr[2]);
                        ALU[i] /= j >= 0 ? ALU[j] : int.Parse(instr[2]);
                        break;
                    case "mod":
                        i = VariableIdx(instr[1]);
                        j = VariableIdx(instr[2]);
                        ALU[i] %= j >= 0 ? ALU[j] : int.Parse(instr[2]);
                        break;
                    case "eql":
                        i = VariableIdx(instr[1]);
                        j = VariableIdx(instr[2]);
                        ALU[i] = j >= 0 ? ALU[i] == ALU[j] ? 1 : 0 : ALU[i] == int.Parse(instr[2]) ? 1 : 0;
                        break;
                }
            }

            int VariableIdx(string str)
            {
                return str switch
                {
                    "w" => 0,
                    "x" => 1,
                    "y" => 2,
                    "z" => 3,
                    _ => -1
                };
            }

            return (ALU[3] == 0, ALU[3]);
        }
    }
}
