using Advent._2023.Day;
using System.Text.RegularExpressions;

namespace AdventOfCode2023.Week3;

class Day19 : IDay
{
    public void Execute()
    {
        var input = File.ReadAllText(@"Week3\input19.txt").Split($"{Environment.NewLine}{Environment.NewLine}");

        //read workFlows 
        var workFlows = new Dictionary<string, List<WorkFlowRule>>();

        _ = input[0].Split(Environment.NewLine).Select(line =>
        {
            var x = line.Split('{');
            var label = x[0];
            var rulesStr = x[1][..^1].Split(',');

            var rules = new List<WorkFlowRule>();
            foreach (var r in rulesStr)
            {
                var y = r.Split(":", StringSplitOptions.RemoveEmptyEntries);
                if (y.Length == 1)
                {
                    rules.Add(new WorkFlowRule('0', '0', 0, y[0]));
                    continue;
                }

                rules.Add(new WorkFlowRule(y[0][0], y[0][1], int.Parse(y[0][2..]), y[1]));
            }

            workFlows.Add(label, rules);
            return 0;
        }).ToList();

        //read parts
        string regexPattern = @"\{x=(\d+),m=(\d+),a=(\d+),s=(\d+)\}";
        var partRatings = input[1].Split(Environment.NewLine).Select(line =>
        {
            Match match = Regex.Match(line, regexPattern);
            int x = int.Parse(match.Groups[1].Value);
            int m = int.Parse(match.Groups[2].Value);
            int a = int.Parse(match.Groups[3].Value);
            int s = int.Parse(match.Groups[4].Value);

            return new Xmas(x, m, a, s);

        }).ToList();

        //do tasks
        Console.WriteLine($"A: {TaskA(partRatings, workFlows)}");
        Console.WriteLine($"B: {TaskB(workFlows)}");
    }

    record Xmas(int X, int M, int A, int S);

    record WorkFlowRule(char Letter, char ConditionSign, int Value, string NextWorkFlow)
    {
        public bool Apply(Xmas xmas)
        {
            switch (Letter)
            {
                case 'x':
                    if (ConditionSign == '<')
                        return xmas.X < Value;
                    return xmas.X > Value;
                case 'm':
                    if (ConditionSign == '<')
                        return xmas.M < Value;
                    return xmas.M > Value;
                case 'a':
                    if (ConditionSign == '<')
                        return xmas.A < Value;
                    return xmas.A > Value;
                case 's':
                    if (ConditionSign == '<')
                        return xmas.S < Value;
                    return xmas.S > Value;
                default:
                    return true;
            }
        }
    }
    
    int TaskA(List<Xmas> partRatings, Dictionary<string, List<WorkFlowRule>> workFlows)
    {
        var A = new List<Xmas>();

        foreach (var part in partRatings)
        {
            var label = "in";

            while (true)
            {
                var rule = workFlows[label].First(rule => rule.Apply(part));
                label = rule.NextWorkFlow;

                if (label == "A")
                {
                    A.Add(part);
                    break;
                }

                if (label == "R")
                    break;
            }
        }

        return A.Sum(x => x.X + x.M + x.A + x.S);
    }

    record Range(string label,
        int Xb, int Xe,
        int Mb, int Me,
        int Ab, int Ae,
        int Sb, int Se);

    long TaskB(Dictionary<string, List<WorkFlowRule>> workFlows)
    {
        var accepted = new List<Range>();
        var ranges = new List<Range>() { new ("in",1, 4000, 1, 4000, 1, 4000, 1, 4000) };

        while (ranges.Count > 0)
        {
            var newRanges = new List<Range>();

            foreach (var range in ranges)
            {
                if (range.label == "A")
                {
                    accepted.Add(range);
                    continue;
                }

                if (range.label == "R")
                    continue;

                //rule-based division of the ranges
                var rules = workFlows[range.label];
                var rangeToModify = range;

                foreach (var rule in rules)
                {
                    if (rule.Letter == '0')
                    {
                        newRanges.Add(rangeToModify with {label = rule.NextWorkFlow});
                        break;
                    }

                    var x = DivideRange(rule, rangeToModify);
                    if(x.Pass is not null)
                        newRanges.Add(x.Pass with {label = rule.NextWorkFlow});

                    if (x.NotPass is not null)
                        rangeToModify = x.NotPass with { label = rule.NextWorkFlow };
                }
            }

            ranges = newRanges;
        }

        return accepted.Sum(range => (1L * (range.Xe - range.Xb + 1) * (range.Me - range.Mb + 1) 
                                      * (range.Ae - range.Ab + 1) * (range.Se - range.Sb + 1)));

        (Range? Pass, Range? NotPass) DivideRange(WorkFlowRule rule, Range range)
        {
            return rule.Letter switch
            {
                'x' => ProcessRange( range.Xb, range.Xe, v => range with { Xb = v }, v => range with { Xe = v }),
                'm' => ProcessRange( range.Mb, range.Me, v => range with { Mb = v }, v => range with { Me = v }),
                'a' => ProcessRange( range.Ab, range.Ae, v => range with { Ab = v }, v => range with { Ae = v }),
                's' => ProcessRange( range.Sb, range.Se, v => range with { Sb = v }, v => range with { Se = v }),
                _ => throw new Exception()
            };

            (Range?, Range?) ProcessRange(int start, int end, Func<int, Range> updateStart, Func<int, Range> updateEnd)
            {
                if (rule.ConditionSign == '<')
                {
                    if (end < rule.Value)
                        return (range, null);

                    if (start < rule.Value)
                        return (updateEnd(rule.Value - 1), updateStart(rule.Value));
                    
                    return (null, range);
                }

                // '>'
                if (start > rule.Value)
                    return (range, null);

                if (end > rule.Value)
                    return (updateStart(rule.Value + 1), updateEnd(rule.Value));

                return (null, range);
            }
        }
    }
}