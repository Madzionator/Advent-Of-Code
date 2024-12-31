using System.Reflection;
using AdventOfCode2024;

var day = DateTime.Now.AddHours(-6).Day;
day=1;

var type = Assembly.GetExecutingAssembly().DefinedTypes.First(x => x.Name.Equals($"Day{day}"));
var dayInstance = (Day)Activator.CreateInstance(type)!;

dayInstance.Run(day);