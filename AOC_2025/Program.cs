using AdventOfCode2025;
using System.Reflection;

var day="01";

var type = Assembly.GetExecutingAssembly().DefinedTypes.First(x => x.Name.Equals($"Day{day}"));
var dayInstance = (Day)Activator.CreateInstance(type)!;

var examplePath = $"example{day}_1.txt";
var inputPath = $"input{day}.txt";

var fullPath = Path.Combine("Days", examplePath);

if (!File.Exists(fullPath))
    throw new FileNotFoundException($"Input file not found: {fullPath}", fullPath);

var lines = File.ReadAllLines(fullPath);

var result = dayInstance.Execute(lines);
Console.WriteLine($"| Day{day} | PartA = {result.PartA} | PartB = {result.PartB} |");