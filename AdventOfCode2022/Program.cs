using Advent._2022.Day;
using Advent._2022.Week1;
using System.Diagnostics;
using System.Reflection;

var day = $"Day{DateTime.Now.AddHours(-6).Day}";
//day = "Day1";
var type = Assembly.GetExecutingAssembly().DefinedTypes.First(x => x.Name.Equals(day));
var dayInstance = (IDay)Activator.CreateInstance(type);

var s = new Stopwatch();
s.Start();
dayInstance.Execute();
s.Stop();

Console.WriteLine($"\n\nTime Elapsed: {s.ElapsedMilliseconds}ms");