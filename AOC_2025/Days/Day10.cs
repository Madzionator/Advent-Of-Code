using Google.OrTools.LinearSolver;

namespace AdventOfCode2025.Days;

[AocData("example10_1.txt", 7, 33)]
[AocData("input10.txt", 530, 20172)]
public class Day10 : Day
{
    private record Manual(int Lights, List<int[]> buttons, List<int> JoltageRequirement);

    public override (object? PartA, object? PartB) Execute(string[] inputLines)
    {
        var manuals = inputLines.Select(line =>
        {
            var lightEndAt = line.IndexOf(']');
            var joltageStartAt = line.IndexOf('{');

            var lights = Convert.ToInt32(line[1..lightEndAt].Replace('.', '0').Replace('#', '1'), 2);
            var joltages = line[(joltageStartAt + 1)..^1].Split(',').Select(int.Parse).ToList();
            var buttons = line[(lightEndAt + 3)..(joltageStartAt - 2)].Split(") (")
                .Select(button => button.Split(',').Select(int.Parse).ToArray()).ToList();

            return new Manual(lights, buttons, joltages);
        }).ToList();

        return (manuals.Sum(MinButtonPressToConfigureIndicatorLights),
            manuals.Sum(MinButtonPressToConfigureJoltageLevels));
    }

    private int MinButtonPressToConfigureIndicatorLights(Manual manual)
    {
        var lightsCount = manual.JoltageRequirement.Count;
        var lights = new HashSet<int>();
        lights.Add(0);

        for (var k = 1; ; k++)
        {
            var newLights = new HashSet<int>();

            foreach (var light in lights)
            {
                foreach (var button in manual.buttons)
                {
                    var newLight = button.Aggregate(light, (current, lightIdx) => current ^ (1 << (lightsCount - 1 - lightIdx)));

                    if (newLight == manual.Lights)
                    {
                        return k;
                    }

                    newLights.Add(newLight);
                }
            }

            lights = newLights;
        }
    }
    
    /// <summary>
    /// Finds the minimum total number of button presses required to reach the target joltage counters.
    ///
    /// Example:
    /// Buttons: (3) (1,3) (2) (2,3) (0,2) (0,1)
    /// Joltage requirements: {3,5,4,7}
    ///
    /// Equations for each counter:
    /// x4 + x5 = 3
    /// x1 + x5 = 5
    /// x2 + x3 + x4 = 4
    /// x0 + x1 + x3 = 7
    ///
    /// Example solution:
    /// x0 = 1, x1 = 3, x2 = 0, x3 = 3, x4 = 1, x5 = 2
    /// Total presses = 1 + 3 + 0 + 3 + 1 + 2 = 10
    ///
    /// Uses OR-Tools CBC solver for integer linear programming (ILP).
    /// </summary>
    /// <param name="manual">Manual containing buttons and target joltage counters.</param>
    /// <returns>Minimum total number of button presses.</returns>

    private int MinButtonPressToConfigureJoltageLevels(Manual manual)
    {
        var solver = Solver.CreateSolver("CBC_MIXED_INTEGER_PROGRAMMING");
        var buttonCount = manual.buttons.Count;

        // Variables: number of presses for each button (>=0)
        var vars = solver.MakeIntVarArray(buttonCount, 0, int.MaxValue);

        // Constraints: for each counter, sum of button presses affecting it must equal the target
        for (var j = 0; j < manual.JoltageRequirement.Count; j++)
        {
            var constraint = solver.MakeConstraint(manual.JoltageRequirement[j], manual.JoltageRequirement[j]);

            for (var b = 0; b < buttonCount; b++)
            {
                if (manual.buttons[b].Contains(j))
                {
                    constraint.SetCoefficient(vars[b], 1); // button b contributes to counter j
                }
            }
        }

        // Goal: minimize total number of button presses
        var objective = solver.Objective();
        for (var b = 0; b < buttonCount; b++)
        {
            objective.SetCoefficient(vars[b], 1);
        }
        objective.SetMinimization();

        solver.Solve();

        return vars.Select(v => (int)v.SolutionValue()).Sum();
    }
}