using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent._2020.Week1
{
    public class Day4
    {
        public static void Execute()
        {
            var lines = File.ReadAllLines(@"Week1\input4.txt");
            var records = new List<string>();
            string record = "";
            foreach(var line in lines)
            {
                if(line == "")
                {
                    records.Add(record);
                    record = "";
                }
                else
                    record = record + " " + line;
            }
            records.Add(record);

            var (A, B) = Task(records);
            Console.WriteLine(A);
            Console.WriteLine(B);
        }

        private static (int, int) Task(List<string>records)
        {
            int resultA = 0;
            int resultB = 0;
            string[] parameters = {"byr:", "iyr:", "eyr:", "hgt:", "hcl:", "ecl:", "pid:"};
            foreach (var record in records)
            {
                int temp = 0;
                foreach (var p in parameters)
                    if (record.Contains(p))
                        temp++;
                if (temp == 7)
                {
                    resultA++;
                    if (IsCorrect(record))
                        resultB++;
                }
            }
            return (resultA, resultB);
        }

        private static bool IsCorrect(string record)
        {
            string[] elements = record.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach(string element in elements)
            {
                string parameter = element.Substring(0, 4);
                if (parameter == "cid:")
                    continue;
                string value = element.Substring(4, element.Length - 4);

                switch(parameter)
                {
                    case "byr:":
                        if (int.Parse(value) < 1920 || int.Parse(value) > 2002)
                            return false;
                        break;

                    case "iyr:":
                        if (int.Parse(value) < 2010 || int.Parse(value) > 2020)
                            return false;
                        break;

                    case "eyr:":
                        if (int.Parse(value) < 2020 || int.Parse(value) > 2030)
                            return false;
                        break;

                    case "hgt:":
                        Regex rgx = new Regex(@"^[0-9]{3}cm$");
                        if (rgx.IsMatch(value))
                        {
                           if (int.Parse(value.Substring(0, 3)) < 150 || int.Parse(value.Substring(0, 3)) > 193)
                                return false;
                        }
                        else
                        {
                            rgx = new Regex(@"^[0-9]{2}in$");
                            if(rgx.IsMatch(value))
                            {
                                if (int.Parse(value.Substring(0, 2)) < 59 || int.Parse(value.Substring(0, 2)) > 76)
                                    return false;
                            }
                            else 
                            return false;
                        }
                        break;

                    case "hcl:":
                        rgx = new Regex(@"^#[0-9a-f]{6}$");
                        if (!rgx.IsMatch(value))
                            return false;
                        break;

                    case "ecl:":
                        string[]possibilities = { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
                        if (!Array.Exists(possibilities, element => element == value))
                            return false;
                        break;

                    case "pid:":
                        rgx = new Regex(@"^[0-9]{9}$");
                        if (!rgx.IsMatch(value))
                            return false;
                        break;
                }
            }
            return true;
        }
    }
}
