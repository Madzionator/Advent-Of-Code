﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Advent._2019.Week1
{
    public class Day2
    {
        public static void Execute()
        {
            //task 1
            List<int> report = new List<int>();
            StreamReader file = new System.IO.StreamReader(@"Week1\input2.txt");
            string[] all_line = file.ReadToEnd().Split("\r\n");
            int result_A = Task_A(all_line);
            int result_B = Task_B(all_line);

            file.Close();
            Console.WriteLine(result_A);
            Console.WriteLine(result_B);
        }

        public static int Task_A(string[] all)
        {
            int result = 0;
            foreach (string line in all)
            {
                string[] elements = line.Split(' ');
                int counter = 0;
                string[] range = elements[0].Split('-');

                foreach (char c in elements[2])
                    if (c == elements[1][0])
                        counter++;
                if (int.Parse(range[0]) <= counter && counter <= int.Parse(range[1]))
                    result++;
            }
            return result;
        }

        public static int Task_B(string[] all)
        {
            int result = 0;
            foreach (string line in all)
            {
                string[] elements = line.Split(' ');    // position, condition, password
                string[] positions = elements[0].Split('-');
                int counter = 0;
                if (elements[2][int.Parse(positions[0]) - 1] == elements[1][0])
                    counter++;
                if (elements[2][int.Parse(positions[1]) - 1] == elements[1][0])
                    counter++;

                if (counter == 1)
                    result++;
            }
            return result;

        }

    }
}