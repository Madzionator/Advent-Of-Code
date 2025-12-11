using System.Diagnostics;
using System.Reflection;
using Xunit;
using Xunit.Sdk;
using Xunit.v3;

[assembly: CaptureConsole]

namespace AdventOfCode2025;

/// <summary>
/// Base AoC day with a single run entry and an inherited xUnit theory verifying datasets.
/// </summary>
public abstract class Day
{
    /// <summary>
    /// Executes the puzzle and returns Part A and Part B results.
    /// </summary>
    public abstract (object? PartA, object? PartB) Execute(string[] inputLines);

    /// <summary>
    /// xUnit theory: for each <see cref="AocDataAttribute"/> on the derived day,
    /// loads input and asserts expected Part 1/Part 2 if provided.
    /// </summary>
    /// <param name="path">Relative input file path.</param>
    /// <param name="expectedPart1">Expected Part 1 (nullable).</param>
    /// <param name="expectedPart2">Expected Part 2 (nullable).</param>
    [Theory(DisplayName = "AoC Dataset")]
    [AocDataset]
    public void VerifiesDataset(string path, object? expectedPart1, object? expectedPart2)
    {
        var fullPath = Path.Combine("Days", path);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"Input file not found: {fullPath}", fullPath);

        var lines = File.ReadAllLines(fullPath);
        var instance = (Day)Activator.CreateInstance(GetType())!;

        var sw = Stopwatch.StartNew();
        var (actualPart1, actualPart2) = instance.Execute(lines);
        sw.Stop();

        Console.WriteLine($"{GetType().Name} | {fullPath} | PartA = {actualPart1} | PartB = {actualPart2} | Elapsed = {sw.Elapsed.TotalSeconds:F3}s");

        if (expectedPart1 is not null)
            Assert.Equal(expectedPart1, actualPart1);

        if (expectedPart2 is not null)
        {
            Assert.Equal(expectedPart2, actualPart2);
        }
    }
}

/// <summary>
/// Declares expected AoC inputs and results for a day. Multiple instances allowed.
/// </summary>
/// <param name="path">Relative path to the input file.</param>
/// <param name="partA">Expected Part 1 result (optional).</param>
/// <param name="partB">Expected Part 2 result (optional).</param>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class AocDataAttribute(string path, object? partA = null, object? partB = null) : Attribute
{
    /// <summary>Relative input file path.</summary>
    public string Path { get; } = path;

    /// <summary>Expected result for Part 1 (nullable).</summary>
    public object? PartA { get; } = partA;

    /// <summary>Expected result for Part 2 (nullable).</summary>
    public object? PartB { get; } = partB;
}

/// <summary>
/// Supplies theory rows by reading <see cref="AocDataAttribute"/> from the derived day type.
/// </summary>
internal sealed class AocDatasetAttribute : DataAttribute
{
    /// <inheritdoc/>
    public override ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(MethodInfo testMethod, DisposalTracker disposalTracker)
    {
        var dayType = testMethod.ReflectedType!;
        var dataAttributes = dayType.GetCustomAttributes<AocDataAttribute>();
        var dataRows = new List<ITheoryDataRow>();
        foreach (var dataAttr in dataAttributes)
        {
            var dataRow = new TheoryDataRow(dataAttr.Path, dataAttr.PartA, dataAttr.PartB);
            dataRows.Add(dataRow);
        }
        return new ValueTask<IReadOnlyCollection<ITheoryDataRow>>(dataRows);
    }

    /// <inheritdoc/>
    public override bool SupportsDiscoveryEnumeration() => true;
}