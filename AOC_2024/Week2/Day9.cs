namespace AdventOfCode2024.Week2;

internal class Day9 : Day
{
    private Dictionary<int, (int Id, int Length)> _reverseFiles = new(); // <start position, (id, length)>
    private Dictionary<int, int> _gaps = new(); // <start position, length>

    public override (object resultA, object resultB) Execute()
    {
        var input = InputLines[0].Select(x => int.Parse(x.ToString())).ToArray();
        int position = 0, fileId = 0;

        foreach (var (value, index) in input.Select((value, index) => (value, index)))
        {
            if (index % 2 == 0)
            {
                _reverseFiles[position] = (fileId++, value);
            }
            else
            {
                _gaps[position] = value;
            }

            position += value;
        }

        _reverseFiles = _reverseFiles.Reverse().ToDictionary();

        return (TaskA(), TaskB());
    }

    long TaskA()
    {
        var gaps = _gaps.Where(x => x.Value > 0).ToDictionary();
        var fileSystemResult = new Dictionary<int, (int, int)>();

        foreach (var file in _reverseFiles)
        {
            var fileLengthLeft = file.Value.Length;
            while (fileLengthLeft > 0)
            {
                var gapPosition = gaps.MinBy(x => x.Key);
                if (gapPosition.Key > file.Key)
                {
                    fileSystemResult.Add(file.Key, (file.Value.Id, fileLengthLeft));
                    break;
                }

                gaps.Remove(gapPosition.Key);
                if (fileLengthLeft >= gapPosition.Value)
                {
                    fileSystemResult.Add(gapPosition.Key, (file.Value.Id, gapPosition.Value));
                    fileLengthLeft -= gapPosition.Value;
                }
                else
                {
                    fileSystemResult.Add(gapPosition.Key, (file.Value.Id, fileLengthLeft));
                    gaps.Add(gapPosition.Key + fileLengthLeft, gapPosition.Value - fileLengthLeft);
                    fileLengthLeft = 0;
                }
            }
        }

        return fileSystemResult
            .Sum(r => Enumerable.Range(r.Key, r.Value.Item2)
                .Sum(i => (long)r.Value.Item1 * i));
    }

    long TaskB()
    {
        var gaps = _gaps.Where(x => x.Value > 0).ToDictionary();
        var fileSystemResult = new Dictionary<int, (int, int)>();

        foreach (var file in _reverseFiles)
        {
            var gapPosition = gaps.FirstOrDefault(x => x.Value >= file.Value.Length && x.Key < file.Key);

            if (gapPosition is { Key: 0, Value: 0 }) // not found
            {
                fileSystemResult.Add(file.Key, (file.Value.Id, file.Value.Length));
            }
            else
            {
                fileSystemResult.Add(gapPosition.Key, (file.Value.Id, file.Value.Length));

                gaps.Remove(gapPosition.Key);
                if (gapPosition.Value > file.Value.Length)
                {
                    gaps.Add(gapPosition.Key + file.Value.Length, gapPosition.Value - file.Value.Length);
                }
            }
        }

        return fileSystemResult
            .Sum(r => Enumerable.Range(r.Key, r.Value.Item2)
                .Sum(i => (long)r.Value.Item1 * i));
    }
}