using Advent._2022.Day;

namespace Advent._2022.Week1;

class Day7 : IDay
{
    public void Execute()
    {
        var input = File.ReadAllLines(@"Week1\input7.txt").Select(x => x.Split(' ').ToArray());
        
        var startDir = CreateFileSystem(input);

        Console.WriteLine(startDir.TaskA());

        var requiredSpace = -(70000000 - startDir.TotalSize - 30000000);
        Console.WriteLine(startDir.TaskB(requiredSpace, startDir.TotalSize));
    }

    private Directory CreateFileSystem(IEnumerable<string[]> input)
    {
        var startDir = new Directory(null, "/");
        var currentDir = startDir;

        foreach (var line in input)
        {
            if (line[0] == "$") // command
            {
                if (line[1] == "cd")
                    currentDir = line[2] switch
                    {
                        "/" => startDir,
                        ".." => currentDir.Parent,
                        _ => currentDir.Directories.FirstOrDefault(x => x.Name == line[2])
                    };
            }
            else //directory content (after ls)
            {
                if (line[0] == "dir")
                    currentDir.Directories.Add(new Directory(currentDir, line[1]));
                else
                    currentDir.Files.Add(new FileD(line[1], int.Parse(line[0])));
            }
        }

        return startDir;
    }

    private record FileD(string Name, int Size);

    private record Directory(Directory Parent, string Name)
    {
        public List<Directory> Directories = new();
        public List<FileD> Files = new();
        public int TotalSize => size ??= Directories.Sum(dir => dir.TotalSize) + Files.Sum(file => file.Size);
        private int? size;

        public int TaskA() => (TotalSize <= 100000 ? TotalSize : 0) + Directories.Sum(dir => dir.TaskA());

        public int TaskB(int requiredSpace, int min)
        {
            if (TotalSize >= requiredSpace && TotalSize < min)
                min = TotalSize;

            return Directories.Aggregate(min, (current, dir) => dir.TaskB(requiredSpace, current));
        }
    }
}