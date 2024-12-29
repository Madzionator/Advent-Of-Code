using AdventOfCode2024.Week1;
using AdventOfCode2024.Week2;
using AdventOfCode2024.Week3;
using AdventOfCode2024.Week4;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2024;

public class ExampleTests
{
    #region Week1

    [Fact]
    public void Day01() => ExecuteDay<Day1>("Week1/example1_1.txt", "11", "31");

    [Fact]
    public void Day02() => ExecuteDay<Day2>("Week1/example2_1.txt", "2", "4");

    [Theory]
    [InlineData(1, "161", "")]
    [InlineData(2, "", "48")]
    public void Day03(int ex, string a, string b) => ExecuteDay<Day3>($"Week1/example3_{ex}.txt", a, b);

    [Fact]
    public void Day04() => ExecuteDay<Day4>("Week1/example4_1.txt", "18", "9");

    [Fact]
    public void Day05() => ExecuteDay<Day5>("Week1/example5_1.txt", "143", "123");

    [Fact]
    public void Day06() => ExecuteDay<Day6>("Week1/example6_1.txt", "41", "6");

    [Fact]
    public void Day07() => ExecuteDay<Day7>("Week1/example7_1.txt", "3749", "11387");

    #endregion

    #region Week2

    [Theory]
    [InlineData(1, "14", "34")]
    [InlineData(2, "", "9")]
    public void Day08(int ex, string a, string b) => ExecuteDay<Day8>($"Week2/example8_{ex}.txt", a, b);

    [Fact]
    public void Day09() => ExecuteDay<Day9>("Week2/example9_1.txt", "1928", "2858");

    [Fact]
    public void Day10() => ExecuteDay<Day10>("Week2/example10_1.txt", "36", "81");

    [Fact]
    public void Day11() => ExecuteDay<Day11>("Week2/example11_1.txt", "55312", "65601038650482");

    [Theory]
    [InlineData(1, "140", "80")]
    [InlineData(2, "772", "436")]
    [InlineData(3, "1930", "1206")]
    public void Day12(int ex, string a, string b) => ExecuteDay<Day12>($"Week2/example12_{ex}.txt", a, b);

    [Fact]
    public void Day13() => ExecuteDay<Day13>("Week2/example13_1.txt", "480", "");

    [Fact]
    public void Day14() => ExecuteDay<Day14>("Week2/example14_1.txt", "12", "");

    #endregion

    #region Week3

    [Theory]
    [InlineData(1, "2028", "1751")]
    [InlineData(2, "10092", "9021")]
    public void Day15(int ex, string a, string b) => ExecuteDay<Day15>($"Week3/example15_{ex}.txt", a, b);


    [Theory]
    [InlineData(1, "7036", "45")]
    [InlineData(2, "11048", "64")]
    public void Day16(int ex, string a, string b) => ExecuteDay<Day16>($"Week3/example16_{ex}.txt", a, b);

    [Theory]
    [InlineData(1, "4,6,3,5,6,3,5,2,1,0", "")]
    [InlineData(2, "", "117440")]
    public void Day17(int ex, string a, string b) => ExecuteDay<Day17>($"Week3/example17_{ex}.txt", a, b);

    [Fact]
    public void Day18() => ExecuteDay<Day18>("Week3/example18_1.txt", "22", "6,1");

    [Fact]
    public void Day19() => ExecuteDay<Day19>("Week3/example19_1.txt", "6", "16");

    [Fact]
    public void Day20() => ExecuteDay<Day20>("Week3/example20_1.txt", "", ""); // Various numbers to verify during debugging

    #endregion

    #region Week4

    [Theory]
    [InlineData(1, "37327623", "")]
    [InlineData(2, "", "23")]
    public void Day22(int ex, string a, string b) => ExecuteDay<Day22>($"Week4/example22_{ex}.txt", a, b);

    [Fact]
    public void Day23() => ExecuteDay<Day23>("Week4/example23_1.txt", "7", "co,de,ka,ta");

    [Theory]
    [InlineData(1, "4", "")]
    [InlineData(2, "2024", "")]
    public void Day24(int ex, string a, string b) => ExecuteDay<Day24>($"Week4/example24_{ex}.txt", a, b);

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