using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Week1;
class Day7 : Day
{
    record Equation(long Result, int[] Numbers);

    private List<Equation> _equations = null!;

    public override (object resultA, object resultB) Execute()
    {
        _equations = InputLines.Select(line =>
        {
            var split = line.Split(':');
            var numbers = split[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            return new Equation(long.Parse(split[0]), numbers);

        }).ToList();

        return (Task(false), Task(true));
    }

    long Task(bool isTaskB) => _equations
        .Select(e => new { IsValid = EvaluateEquation(e, 1, 1L * e.Numbers[0], isTaskB), e.Result })
        .Where(x => x.IsValid)
        .Sum(x => x.Result);

    bool EvaluateEquation(Equation equation, int idx, long partResult, bool isTaskB)
    {
        if (idx >= equation.Numbers.Length)
            return partResult == equation.Result;

        if (partResult > equation.Result)
            return false;
        
        var resPlus = EvaluateEquation(equation, idx + 1, partResult + equation.Numbers[idx], isTaskB);
        var resMul = EvaluateEquation(equation, idx + 1, partResult * equation.Numbers[idx], isTaskB);
        var resChanged = isTaskB && EvaluateEquation(equation, idx + 1, partResult.Concat(equation.Numbers[idx]), isTaskB);

        return resPlus || resMul || resChanged;
    }
}