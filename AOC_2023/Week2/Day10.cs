namespace AdventOfCode2023.Week2;

using Advent._2023.Day;

class Day10 : IDay
{
    private char[,] _map;
    private (int dY, int dX) _dir;

    private const char StartSign = 'S';
    private const char PipeSign = '@';
    private const char LoopSign = '#';
    private const char VoidSign = ' ';
    private const char WaterSign = '~';

    public void Execute()
    {
        var input = File.ReadAllLines(@"Week2\input10.txt").ToCharMatrix();
        _map = ExpandMap(input);

        Console.WriteLine($"A: {TaskA()}");
        Console.WriteLine($"B: {TaskB()}");
    }

    int TaskA()
    {
        var steps = 1;
        var startPoint = _map.First(x => x == StartSign);
        var nextStep = Move(startPoint.Item1, startPoint.Item2);

        while (_map[nextStep.Item1, nextStep.Item2] != StartSign)
        {
            _map[nextStep.Item1, nextStep.Item2] = LoopSign;
            nextStep = Move(nextStep.Item1, nextStep.Item2);
            steps++;
        }

        _map[nextStep.Item1, nextStep.Item2] = LoopSign;
        
        return (steps / 3 + 1) / 2;
    }

    int TaskB()
    {
        _map.OnEachInMatrix(c => c == LoopSign ? LoopSign : VoidSign);

        FillEmptyEdges();
        FillEmptySpaces();

        //_map.DrawMatrix();

        return CountEmpty3x3Square();
    }

    public char[,] ExpandMap(char[,] map)
    {
        //expand each char to char[,]:
        // for ex. 'L' to:
        //                 .@.
        //                 .@@
        //                 ...
        
        var newMap = new char[map.GetLength(0) * 3, map.GetLength(1) * 3];
        newMap.OnEachInMatrix(x => '.');

        var sCenterY = 0;
        var sCenterX = 0;

        for (var y = 0; y < map.GetLength(0); y++)
            for (var x = 0; x < map.GetLength(1); x++)
            {
                var mY = y * 3;
                var nX = x * 3;

                switch (map[y, x])
                {
                    case '|':
                        newMap[mY, nX + 1] = PipeSign;
                        newMap[mY + 1, nX + 1] = PipeSign;
                        newMap[mY + 2, nX + 1] = PipeSign;
                        break;

                    case '-':
                        newMap[mY + 1, nX] = PipeSign;
                        newMap[mY + 1, nX + 1] = PipeSign;
                        newMap[mY + 1, nX + 2] = PipeSign;
                        break;

                    case 'L':
                        newMap[mY, nX + 1] = PipeSign;
                        newMap[mY + 1, nX + 1] = PipeSign;
                        newMap[mY + 1, nX + 2] = PipeSign;
                        break;

                    case 'J':
                        newMap[mY, nX + 1] = PipeSign;
                        newMap[mY + 1, nX + 1] = PipeSign;
                        newMap[mY + 1, nX] = PipeSign;
                        break;

                    case '7':
                        newMap[mY + 1, nX] = PipeSign;
                        newMap[mY + 1, nX + 1] = PipeSign;
                        newMap[mY + 2, nX + 1] = PipeSign;
                        break;

                    case 'F':
                        newMap[mY + 1, nX + 1] = PipeSign;
                        newMap[mY + 1, nX + 2] = PipeSign;
                        newMap[mY + 2, nX + 1] = PipeSign;
                        break;

                    case 'S':
                        newMap[mY + 1, nX + 1] = StartSign;
                        sCenterY = mY + 1;
                        sCenterX = nX + 1;
                        break;
                }
            }


        // visually connect S with near pipe
        var dir = new[] { (-1, 0), (1, 0), (0, -1), (0, 1) };

        foreach (var d in dir)
        {
            try
            {
                if (newMap[sCenterY + (2 * d.Item1), sCenterX + (2 * d.Item2)] == PipeSign)
                {
                    newMap[sCenterY + d.Item1, sCenterX + d.Item2] = PipeSign;
                    _dir = d;
                }
            }
            catch {}
        }

        return newMap;
    }

    (int, int) Move(int currentY, int currentX)
    {
        if (_dir != (-1, 0) && _map[currentY + 1, currentX] is PipeSign or StartSign)
        {
            _dir = (1, 0);
            return (currentY + 1, currentX);
        }

        if (_dir != (1, 0) && _map[currentY - 1, currentX] is PipeSign or StartSign)
        {
            _dir = (-1, 0);
            return (currentY - 1, currentX);
        }

        if (_dir != (0, -1) && _map[currentY, currentX + 1] is PipeSign or StartSign)
        {
            _dir = (0, 1);
            return (currentY, currentX + 1);
        }

        if (_dir != (0, 1) && _map[currentY, currentX - 1] is PipeSign or StartSign)
        {
            _dir = (0, -1);
            return (currentY, currentX - 1);
        }

        throw new Exception("Dead End");
    }
    
    void FillEmptyEdges()
    {
        for (var y = 0; y < _map.GetLength(0); y++)
        {
            if (_map[y, 0] != LoopSign)
                _map[y, 0] = WaterSign;

            if (_map[y, _map.GetLength(1) - 1] != LoopSign)
                _map[y, _map.GetLength(1) - 1] = WaterSign;
        }

        for (var x = 1; x < _map.GetLength(1) - 1; x++)
        {
            if (_map[0, x] != LoopSign)
                _map[0, x] = WaterSign;

            if (_map[_map.GetLength(0) - 1, x] != LoopSign)
                _map[_map.GetLength(0) - 1, x] = WaterSign;
        }
    }

    void FillEmptySpaces()
    {
        var sthChanged = true;

        while (sthChanged)
        {
            sthChanged = false;
            for (var y = 1; y < _map.GetLength(0) - 1; y++)
            for (var x = 1; x < _map.GetLength(1) - 1; x++)
                if (_map[y, x] == VoidSign && (_map[y - 1, x] is WaterSign || _map[y + 1, x] is WaterSign || _map[y, x - 1] is WaterSign || _map[y, x + 1] is WaterSign))
                {
                    _map[y, x] = WaterSign;
                    sthChanged = true;
                }
        }
    }

    int CountEmpty3x3Square()
    {
        var inside = 0;

        for (var y = 0; y < _map.GetLength(0); y += 3)
        for (var x = 0; x < _map.GetLength(1); x += 3)
        {
            var isEmpty = true;
            for (var y2 = y; y2 < y + 3; y2++)
            {
                for (var x2 = x; x2 < x + 3; x2++)
                {
                    if (_map[y2, x2] != VoidSign)
                    {
                            isEmpty = false;
                            break;
                    }
                }

                if (!isEmpty) break;
            }

            if (isEmpty)
                inside++;
        }

        return inside;
    }
}