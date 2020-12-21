using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2020.Week3
{
    public class Day21
    {
        public static void Execute()
        {
            var data = File.ReadAllLines(@"Week3\input21.txt");

            var results = Task(data);
            int resultA = results.Item1;
            string resultB = results.Item2;

            Console.WriteLine(resultA);
            Console.WriteLine(resultB);
        }

        private static (int, string) Task(string[] data)
        {
            //// prepare data ////
            var allergens = new Dictionary<string, (Dictionary<string, int>, int)>();
            var ingredients = new Dictionary<string, bool>();
            foreach (var line in data)
            {
                var splitLine = line.Replace(")", "").Split(" (contains ");
                var ingrs = splitLine[0].Split(" ");
                var allergs = splitLine[1].Split(", ");

                foreach (var ingr in ingrs) // create ingredients dictionary
                    if (!ingredients.ContainsKey(ingr))
                        ingredients.Add(ingr, false);

                foreach (var alerg in allergs) // create dictionary of allergens and their properties
                {
                    if(!allergens.ContainsKey(alerg))
                    {
                        var insideDict = new Dictionary<string, int>();
                        foreach (var ingr in ingrs)
                            insideDict.Add(ingr, 1);
                        allergens.Add(alerg, (insideDict, 1));
                    }
                    else
                    {
                        foreach (var ingr in ingrs)
                        {
                            if (allergens[alerg].Item1.ContainsKey(ingr))
                                allergens[alerg].Item1[ingr]++;
                            else
                                allergens[alerg].Item1.Add(ingr, 1);
                        }
                        var temp = allergens[alerg]; temp.Item2++;
                        allergens[alerg] = temp;
                    }
                }
            }

            //// Task A ////
            foreach (var allergen in allergens)
            {
                int quantity = allergen.Value.Item2;
                foreach (var allerg in allergen.Value.Item1)
                    if (allerg.Value == quantity)
                        ingredients[allerg.Key] = true; // can have allergen on not
            }

            int resultA = 0;

            foreach (var line in data) // "How many times do any of those ingredients appear?"
            {
                var splitLine = line.Replace(")", "").Split(" (contains ");
                var ingrs = splitLine[0].Split(" ");
                foreach (var ingr in ingrs)
                    if (ingredients[ingr] == false)
                        resultA++;
            }

            //// Task B ////
            foreach (var allergen in allergens)  // remove non-allergenic ingredients
            {
                foreach (var allergenDict in allergen.Value.Item1)
                    if (!ingredients[allergenDict.Key])
                        allergen.Value.Item1.Remove(allergenDict.Key);
            }

            var solution = new SortedDictionary<string, string>();
            while (true)
            {
                foreach (var allergen in allergens)
                {
                    int possibilitiesCounter = 0;
                    string ingr = "";
                    int quantity = allergen.Value.Item2;
                    foreach (var allergDict in allergen.Value.Item1)
                    {
                        if (allergDict.Value == quantity)    // try to find exactly one solution
                        {
                            possibilitiesCounter++;
                            ingr = allergDict.Key;
                        }
                    }

                    if (possibilitiesCounter == 1)
                    {
                        solution.Add(allergen.Key, ingr);
                        foreach (var allergDict in allergens)     // remove known ingredients and allergens
                            if (ingredients[ingr])
                                allergDict.Value.Item1.Remove(ingr);
                        allergen.Value.Item1.Remove(allergen.Key);
                    }
                }
                if (solution.Count == allergens.Count)
                    break;
            }

            string resultB = String.Join(",", solution.Values);

            return (resultA, resultB);
        }
    }
}