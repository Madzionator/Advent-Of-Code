using Advent._2022.Day;

namespace Advent._2022.Week2;

class Day13 : IDay
{
    public void Execute()
    {
        var input = File.ReadAllLines(@"Week2\input13.txt");
        
        Console.WriteLine(TaskA(input));
        Console.WriteLine(TaskB(input));
    }

    private int TaskA(string[] input)
    {
        var packets = input.Chunk(3)
            .Select(x => (new Node(x[0]), new Node(x[1])))
            .ToArray();

        return packets
            .Select((x, i) => (packets: x, idx: i))
            .Where(x => IsInRightOrder(x.packets.Item1, x.packets.Item2) != false)
            .Sum(x => x.idx + 1);
    }

    private int TaskB(string[] input)
    {
        var _2 = new Node("[[2]]");
        var _6 = new Node("[[6]]");

        var sortedPackets = input
            .Where(x => x != "")
            .Select(x => new Node(x))
            .Append(_2)
            .Append(_6)
            .Order(new PacketComparer())
            .ToList();

        return (sortedPackets.IndexOf(_2) + 1) * (sortedPackets.IndexOf(_6) + 1);
    }

    private static bool? IsInRightOrder(Node a, Node b)
    {
        while (true)
        {
            //both values are integers
            if (a.Value is not null && b.Value is not null)
            {
                return a.Value != b.Value
                    ? a.Value < b.Value
                    : null;
            }

            //both values are lists
            if (a.Value is null && b.Value is null)
            {
                for (int iA = 0, iB = 0;; iA++, iB++)
                {
                    if (iA >= a.Children.Count && iB < b.Children.Count) return true;
                    if (iA < a.Children.Count && iB >= b.Children.Count) return false;
                    if (iA >= a.Children.Count && iB >= b.Children.Count) return null;

                    var result = IsInRightOrder(a.Children[iA], b.Children[iB]);
                    if (result != null) return result;
                }
            }

            //value and list
            if (a.Value is null && b.Value is not null)
            {
                b = new Node { Children = new() { b } };
                continue;
            }

            if (a.Value is not null && b.Value is null)
            {
                a = new Node { Children = new() { a } };
                continue;
            }

            return true;
        }
    }

    private class Node
    {
        public int? Value { get; set; }
        public List<Node> Children { get; init; } = new();

        public Node()
        {
        }

        public Node(string str)
        {
            if (int.TryParse(str, out var val))
            {
                Value = val;
                return;
            }

            if (str == "[]")
                return;

            str = str[1..^1]; //remove brackets [] 

            var depth = 0;
            var iS = 0;

            for (var i = 0; i < str.Length; i++)
            {
                switch (str[i])
                {
                    case '[':
                        if (depth == 0)
                            iS = i;
                        depth++;
                        break;
                    case ']':
                        if (depth == 1)
                            Children.Add(new Node(str[iS..(i + 1)]));
                        depth--;
                        break;
                    case ',':
                        break;
                    default:
                        if (depth == 0)
                        {
                            if (i < str.Length - 1 && '0' <= str[i + 1] && str[i + 1] <= '9')
                            {
                                Children.Add(new Node(str[i..(i + 2)]));
                                i++;
                            }
                            else
                                Children.Add(new Node(str[i].ToString()));
                        }
                        break;
                }
            }
        }

        public override string ToString() => Value.HasValue ? Value.ToString() : $"[{string.Join(",", Children)}]";
    }

    private class PacketComparer : IComparer<Node>
    {
        public int Compare(Node x, Node y)
        {
            return IsInRightOrder(x, y) switch
            {
                true => -1,
                null => 0,
                _ => 1
            };
        }
    }
}