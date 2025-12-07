namespace AdventOfCode2025.Days;

[AocData("example6_1.txt", 4277556L, 3263827L)]
[AocData("input6.txt", 5784380717354L, 7996218225744L)]
public class Day6 : Day
{
    private record CephalopodProblem(List<int> NumbersA, List<int> NumbersB, char Operation);

    public override (object? PartA, object? PartB) Execute(string[] inputLines)
    {
        var problems = SeparateProblems(inputLines);

        var resultA = 0L;
        var resultB = 0L;

        foreach (var problem in problems)
        {
            resultA += DoOperations(problem.NumbersA, problem.Operation);
            resultB += DoOperations(problem.NumbersB, problem.Operation);
        }
        
        return (resultA, resultB);
    }

    private long DoOperations(List<int> numbers, char operation) => operation == '*'
        ? numbers.Aggregate(1L, (current, number) => current * number)
        : numbers.Aggregate(0L, (current, number) => current + number);

    private List<CephalopodProblem> SeparateProblems(string[] inputLines)
    {
        var nextProblemIndexes = inputLines[^1]
            .Select((c, i) => new { c, i })
            .Where(x => x.c is '*' or '+')
            .Select(x => x.i)
            .ToArray();

        var problems = new List<CephalopodProblem>();

        for (var p = 0; p < nextProblemIndexes.Length; p++)
        {
            var numbersA = new List<int>();
            var numbersB = new List<int>();

            var startX = nextProblemIndexes[p];
            var endX = p >= nextProblemIndexes.Length - 1 
                ? inputLines[0].Length 
                : nextProblemIndexes[p + 1] - 1;

            for (var y = 0; y < inputLines.Length-1; y++)
            {
                var num = "";
                for (var x = startX; x < endX ; x++)
                {
                    num += inputLines[y][x];
                }

                numbersA.Add(int.Parse(num.Trim()));
            }

            for (var x = startX; x < endX; x++)
            {
                var num = "";
                for (var y = 0; y < inputLines.Length - 1; y++)
                {
                    num += inputLines[y][x];
                }

                numbersB.Add(int.Parse(num.Trim()));
            }

            problems.Add(new CephalopodProblem(numbersA, numbersB, inputLines[^1][startX]));
        }

        return problems;
    }
}