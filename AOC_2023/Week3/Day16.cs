using Advent._2023.Day;

namespace AdventOfCode2023.Week3;

class Day16 : IDay
{
    public void Execute()
    {
        var input = File.ReadAllLines(@"Week3\input16.txt").ToCharMatrix();

        Console.WriteLine($"A: {TaskA(input, 0, 0, 0, 1)}");
        Console.WriteLine($"B: {TaskB(input)}");
    }

    record Point
    {
        public int Y { get; set; }
        public int X { get; set; }

        public (int dY, int dX) Dir { get; set; }

        public Point(int y, int x, int dy, int dx)
        {
            Y = y; X = x; Dir = (dy, dx);
        }

        public void Move(int dy, int dx)
        {
            Y += dy; X += dx;
            Dir = (dy, dx);
        }
    }

    int TaskA(char[,] map, int startY, int startX, int startDirY, int startDirX)
    {
        var maxY = map.GetLength(0) - 1;
        var maxX = map.GetLength(1) - 1;

        var alreadyChecked = new HashSet<(int, int, int, int)>();

        var beams = new List<Point>() { new (startY, startX, startDirY, startDirX) };
        var beamsToAdd = new List<Point>();
        var beamsToRemove = new List<Point>();

        while (beams.Count > 0)
        {
            foreach (var beam in beams)
                ManageBeam(beam); 

            foreach (var r in beamsToRemove)
                beams.Remove(r);

            beams.AddRange(beamsToAdd);
            beamsToAdd = new List<Point>();
            beamsToRemove = new List<Point>();
        }

        return alreadyChecked.Select(x => (x.Item1, x.Item2)).ToHashSet().Count;

        void ManageBeam(Point beam)
        {
            var a = (beam.Y, beam.X, beam.Dir.dY, beam.Dir.dX);
            if (alreadyChecked.Contains(a))
            {
                beamsToRemove.Add(beam);
                return;
            }
            
            alreadyChecked.Add(a);

            switch (map[beam.Y, beam.X])
            {
                case '.':
                    beam.Move(beam.Dir.dY, beam.Dir.dX);
                    break;

                case '\\':
                    beam.Move(beam.Dir.dX, beam.Dir.dY);
                    break;

                case '/':
                    beam.Move(beam.Dir.dX * -1, beam.Dir.dY * -1);
                    break;

                case '|' when beam.Dir.dX == 0:
                    beam.Move(beam.Dir.dY, 0);
                    break;

                case '|':
                    var p1 = new Point(beam.Y - 1, beam.X, -1, 0);
                    if (p1.X <= maxX && p1.Y <= maxY && p1 is { X: >= 0, Y: >= 0 })
                        beamsToAdd.Add(p1);
                    beam.Move(1, 0);
                    break;

                case '-' when beam.Dir.dY == 0:
                    beam.Move(0, beam.Dir.dX);
                    break;

                case '-':
                    var p2 = new Point(beam.Y, beam.X - 1, 0, -1);
                    if (p2.X <= maxX && p2.Y <= maxY && p2 is { X: >= 0, Y: >= 0 })
                        beamsToAdd.Add(p2);
                    beam.Move(0, 1);
                    break;
            }

            if (beam.X > maxX || beam.Y > maxY || beam.X < 0 || beam.Y < 0)
                beamsToRemove.Add(beam);
        }
    }

    int TaskB(char[,] map)
    {
        var answers = new List<int>();
        
        for (var x = 0; x < map.GetLength(1); x++)
        {
            answers.Add(TaskA(map, 0, x, 1, 0));
            answers.Add(TaskA(map, map.GetLength(0)-1, x, -1, 0));
        }

        for (var y = 0; y < map.GetLength(0); y++)
        {
            answers.Add(TaskA(map, y, 0, 0, 1));
            answers.Add(TaskA(map, y, map.GetLength(1)-1, 0, -1));
        }

        return answers.Max();
    }
}