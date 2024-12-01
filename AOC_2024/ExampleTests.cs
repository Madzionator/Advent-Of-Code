﻿using AdventOfCode2024.Week1;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2024;

public class ExampleTests
{
    [Fact]
    public void Day1() => ExecuteDay<Day1>("Week1/example1_1.txt", "11", "31");

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