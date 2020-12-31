using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent._2020.Week3
{
    public class Day19
    {
        public static void Execute()
        {
            (var rules, var words) = ReadData();

            int resultA = TaskA(rules, words);
            int resultB = TaskB(rules, words);

            Console.WriteLine(resultA);
            Console.WriteLine(resultB);
        }
        private static (Dictionary<string, string>, string[]) ReadData()
        {
            var data = File.ReadAllLines(@"Week3\input19.txt").ToList();
            int nextDataCategoty = data.IndexOf("");

            var rules = new Dictionary<string, string>();
            for (int i = 0; i < nextDataCategoty; i++)
            {
                var line = data[i].Split(": ");
                string rule = "(" + line[0] + ")";
                var el = line[1].Replace("\"", "").Split(" ");
                var str = "((";
                    foreach (var x in el)
                    if (x == "|")
                        str += (")" + x + "(");
                    else
                        str += ("(" + x + ")");
                rules.Add(rule, str+"))");
            }

            var words = data.GetRange(nextDataCategoty + 1, data.Count - nextDataCategoty - 1).ToArray();

            return (rules, words);
        }

        private static int TaskA(Dictionary<string, string> rules, string[] words)
        {
            var rgx = new Regex(MakeRegularExpression(rules));
            int result = 0;
            foreach (var word in words)
                if (rgx.IsMatch(word))
                    result++;

            return result;
        }

        private static int TaskB(Dictionary<string, string> rules, string[] words)
        {
            rules["(8)"] = MakeNewRule(new[] { "(42)" }, 8);  // 5 is enough in my case
            rules["(11)"] = MakeNewRule(new[] { "(42)", "(31)" }, 8); // 4

            var rgx = new Regex(MakeRegularExpression(rules));
            int result = 0;
            foreach (var word in words)
                if (rgx.IsMatch(word))
                    result++;

            return result;
        }

        private static string MakeRegularExpression(Dictionary<string, string> rules)
        {
            var expression = rules["(0)"];
            for (var i = 0; i < rules.Count; i++)
                foreach (var m in rules)
                    expression = expression.Replace(m.Key, m.Value);

            return "^" + expression + "$";

        }

        private static string MakeNewRule(string[]newRuleElement, int N)
        {
            string newRule = "";
            for (int n = 1; n <= N; n++)
            {
                string partOfRule = "";
                foreach (var element in newRuleElement)
                    partOfRule += repeat(n, element);

                if (n == 1)
                    newRule += partOfRule;
                else
                    newRule += (")|(" + partOfRule);
            }
            return "((" + newRule + "))";

            string repeat(int n, string el)
            {
                string phrase = "";
                for (int i = 1; i <= n; i++)
                    phrase += el;
                return phrase;
            }
        }
    }
}