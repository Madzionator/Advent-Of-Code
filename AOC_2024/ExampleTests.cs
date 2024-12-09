using AdventOfCode2024.Week1;
using AdventOfCode2024.Week2;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2024;

public class ExampleTests
{
    #region Week1

    [Fact]
    public void Day1() => ExecuteDay<Day1>("Week1/example1_1.txt", "11", "31");

    [Fact]
    public void Day2() => ExecuteDay<Day2>("Week1/example2_1.txt", "2", "4");

    [Theory]
    [InlineData(1, "161", "")]
    [InlineData(2, "", "48")]
    public void Day3(int ex, string a, string b) => ExecuteDay<Day3>($"Week1/example3_{ex}.txt", a, b);

    [Fact]
    public void Day4() => ExecuteDay<Day4>("Week1/example4_1.txt", "18", "9");

    [Fact]
    public void Day5() => ExecuteDay<Day5>("Week1/example5_1.txt", "143", "123");

    [Fact]
    public void Day6() => ExecuteDay<Day6>("Week1/example6_1.txt", "41", "6");

    [Fact]
    public void Day7() => ExecuteDay<Day7>("Week1/example7_1.txt", "3749", "11387");

    #endregion

    #region Week2

    [Theory]
    [InlineData(1, "14", "34")]
    [InlineData(2, "", "9")]
    public void Day8(int ex, string a, string b) => ExecuteDay<Day8>($"Week2/example8_{ex}.txt", a, b);

    [Fact]
    public void Day9() => ExecuteDay<Day9>("Week2/example9_1.txt", "1928", "2858");

    #endregion

    private static void ExecuteDay<TDay>(string path, string expectedResultA, string expectedResultB) where TDay : Day
    {
        var dayInstance = (Day)Activator.CreateInstance(typeof(TDay))!;
        dayInstance.InputLines = File.ReadAllLines(path);

        var (A, B) = dayInstance.Execute();

        if (!string.IsNullOrEmpty(expectedResultA))
        {
            var resultA = A.ToString()!;
            resultA.Should().Be(expectedResultA);
        }

        if (!string.IsNullOrEmpty(expectedResultB))
        {
            var resultB = B.ToString()!;
            resultB.Should().Be(expectedResultB);
        }
    }
}