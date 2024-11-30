global using Advent.Helpers.Extensions;
global using Advent.Helpers.Methods;
using System.Diagnostics;
using System.Reflection;
using AdventOfCode2024;

var day = DateTime.Now.AddHours(-6).Day;
//day=1;

var type = Assembly.GetExecutingAssembly().DefinedTypes.First(x => x.Name.Equals($"Day{day}"));
var dayInstance = (IDay)Activator.CreateInstance(type)!;

var s = new Stopwatch();
s.Start();
dayInstance.Execute();
s.Stop();

Console.WriteLine($"\n\nTime Elapsed: {s.ElapsedMilliseconds}ms");