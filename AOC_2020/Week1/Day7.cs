using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2020.Week1
{
    public class Day7
    {
        public class Color
        {
            public Color(string new_name, (string, int)[]new_content)
            {
                Name = new_name;
                Content = new_content;
            }

            public string Name { get; }    
            public (string, int) [] Content { get; }
        }

        public static void Execute()
        {
            var objects = new List<Color>();
            var all_line = File.ReadAllLines(@"Week1\input7.txt");
            foreach (var line in all_line)
                objects.Add(CreateObject(line.Replace("bags", "bag").Replace(" contain", "").Replace(".", "").Replace(",","")));

            int result_A = Task_A(objects);
            int result_B = Task_B(objects);

            Console.WriteLine(result_A);
            Console.WriteLine(result_B);
        }

        private static int Task_A(List<Color> list)
        {
            int result = 0;
            foreach (var color in list)
                if (HaveGold(color, list))
                    result++;
            return result-1;
        }

        private static int Task_B(List<Color> list)
        {
            var obj = list.Find(color => color.Name == "shinygoldbag");
            return CountBag(obj, list);
        }

        private static int CountBag(Color color, List<Color> list)
        {
            int result = 0;
            foreach (var x in color.Content)
            {
                var obj = list.Find(color => color.Name == x.Item1);
                result += x.Item2 * (CountBag(obj, list) +1);
            }
            return result;
        }

        private static bool HaveGold(Color color, List<Color> list)
        {
            if (color.Name == "shinygoldbag")
                return true;

            foreach (var x in color.Content)
            {
                 var obj = list.Find(color => color.Name == x.Item1);
                 if(HaveGold(obj, list))
                    return true;
            }            
            return false;
        }

        private static Color CreateObject(string line)
        {
            string[] elements = line.Split(' ');

            string colorName = (elements[0] + elements[1] + elements[2]);
            var contained_list = new List <(string, int)>();
            if (elements[3] != "no")
                for(int i = 3; i<elements.Length; i+=4)
                {
                    int quantity = int.Parse(elements[i]);
                    string name = (elements[i + 1] + elements[i + 2] + elements[i + 3]);
                    contained_list.Add((name, quantity));
                }
            return new Color(colorName, contained_list.ToArray());
        }
    }
}