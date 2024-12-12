using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Week2;

internal class Day12 : Day
{
    private char[,] _input;

    private Dictionary<Vector2, int> _map = new(); // <position, region>
    private (int, bool)[,] _extendedMap;  // (region, isEdge)

    public override (object resultA, object resultB) Execute()
    {
        ProcessInput();
        return (TaskA(), TaskB());
    }

    public int TaskA()
    {
        return _map.GroupBy(x => x.Value) // group by region
            .Sum(region => region
                .Select(plot => CountPerimeters(plot.Key, region.Key))
                .Sum() * region.Count());
    }

    public int TaskB()
    {
        var sides = new Dictionary<int, int>(); // <regionId, count> 

        // search vertical edges: left to right
        for (var y = 0; y < _extendedMap.GetLength(0); y++)
        {
            var lastRegion = -1;
            var streak = 0;

            for (var x = 0; x < _extendedMap.GetLength(1); x++)
            {
                if (!_extendedMap[y, x].Item2)
                {
                    lastRegion = -1;
                    streak = 0;
                    continue;
                }

                if (_extendedMap[y, x].Item1 == lastRegion)
                {
                    streak++;
                    if (streak == 2)
                    {
                        sides.AddOrSet(lastRegion, 1);
                    }
                }
                else
                {
                    lastRegion = _extendedMap[y, x].Item1;
                    streak = 1;
                }
            }
        }

        // search horizontal edges: up to down
        for (var x = 0; x < _extendedMap.GetLength(1); x++)
        {
            var lastRegion = -1;
            var streak = 0;

            for (var y = 0; y < _extendedMap.GetLength(0); y++)
            {
                if (!_extendedMap[y, x].Item2)
                {
                    lastRegion = -1;
                    streak = 0;
                    continue;
                }

                if (_extendedMap[y, x].Item1 == lastRegion)
                {
                    streak++;
                    if (streak == 2)
                    {
                        sides.AddOrSet(lastRegion, 1);
                    }
                }
                else
                {
                    lastRegion = _extendedMap[y, x].Item1;
                    streak = 1;
                }
            }
        }

        return _map.GroupBy(x => x.Value)
            .Sum(group => sides[group.Key] * group.Count());
    }

    int CountPerimeters(Vector2 position, int region)
    {
        var perimeters = 0;

        foreach (var dir in Direction2.Sides)
        {
            var pos = position.Move(dir);
            if (_map.TryGetValue(pos, out var neighbourRegion))
            {
                if (neighbourRegion != region)
                    perimeters++;
            }
            else
            {
                perimeters++;
            }
        }

        return perimeters;
    }

    #region ProcessInput

    private void ProcessInput()
    {
        _input = InputLines.ToCharMatrix();
        _extendedMap = new (int, bool)[_input.GetLength(0) * 3, _input.GetLength(1) * 3];
        var region = 0;

        // 1) Find all regions and compute map
        for (var y = 0; y < _input.GetLength(0); y++)
            for (var x = 0; x < _input.GetLength(1); x++)
            {
                var vector = new Vector2(y, x);
                if (_map.ContainsKey(vector))
                    continue;

                FloodFillMap(vector, _input[y, x], region);
                region++;
            }

        // 2) For PartB - Fill extendedMap with info about edges
        foreach (var position in _map)
        {
            _extendedMap[position.Key.Y * 3, position.Key.X * 3] = (position.Value, false);
            foreach (var dir in Direction2.SidesAndCorners)
            {
                var neighbour = position.Key.Move(dir);
                var newPos = (position.Key * 3).Move(dir);
                if (_map.TryGetValue(neighbour, out var neighbourRegion) && neighbourRegion == position.Value)
                {
                    _extendedMap[newPos.Y + 1, newPos.X + 1] = (position.Value, false);
                }
                else
                {
                    _extendedMap[newPos.Y + 1, newPos.X + 1] = (position.Value, true);
                }
            }
        }

        // 3) For Part B - Fix corners in extendedMap
        for (var y = 1; y < _extendedMap.GetLength(0); y += 3)
            for (var x = 1; x < _extendedMap.GetLength(1); x += 3)
            {

                if (_extendedMap[y - 1, x].Item2)
                {
                    _extendedMap[y - 1, x - 1].Item2 = true;
                    _extendedMap[y - 1, x + 1].Item2 = true;
                }

                if (_extendedMap[y, x + 1].Item2)
                {
                    _extendedMap[y - 1, x + 1].Item2 = true;
                    _extendedMap[y + 1, x + 1].Item2 = true;
                }

                if (_extendedMap[y + 1, x].Item2)
                {
                    _extendedMap[y + 1, x - 1].Item2 = true;
                    _extendedMap[y + 1, x + 1].Item2 = true;
                }

                if (_extendedMap[y, x - 1].Item2)
                {
                    _extendedMap[y - 1, x - 1].Item2 = true;
                    _extendedMap[y + 1, x - 1].Item2 = true;
                }
            }
    }

    private void FloodFillMap(Vector2 position, char plant, int region)
    {
        _map.Add(position, region);

        foreach (var dir in Direction2.Sides)
        {
            var newPosition = position.Move(dir);
            if (IsValidNeighbour(newPosition, plant))
            {
                FloodFillMap(newPosition, plant, region);
            }
        }
    }

    private bool IsValidNeighbour(Vector2 vector, char plant)
    {
        if (vector.Y < 0 || vector.X < 0 || vector.Y >= _input.GetLength(0) || vector.X >= _input.GetLength(1))
            return false;

        if (_input[vector.Y, vector.X] != plant)
            return false;

        return !_map.ContainsKey(vector);
    }

    #endregion
}