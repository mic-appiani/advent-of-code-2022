
using System.ComponentModel;
using System.Text.RegularExpressions;
/// Find all of the directories with a total size of at most 100000. What is the sum of the total 
/// sizes of those directories?
using (var sr = new StreamReader("input.txt"))
{
    var filesystem = new Directory { Name = "/" };
    Directory currentDir = filesystem;

    // skip 2 for edge case
    sr.ReadLine();
    sr.ReadLine();

    while (!sr.EndOfStream)
    {
        var line = sr.ReadLine();

        // cd sets active directory
        string setDir = @"\$ cd (.+)";
        var match = Regex.Match(line, setDir);

        if (match.Success)
        {
            var dirName = match.Groups[1].Value;

            if (dirName.Equals(".."))
            {
                currentDir = currentDir.Parent!;
            } else
            {
                currentDir = currentDir.Directories.Where(x => x.Name == dirName).Single();
            }

            continue;
        }

        // dir creates directory    
        string createDir = @"dir (.+)";
        match = Regex.Match(line, createDir);

        if (match.Success)
        {
            var dirName = match.Groups[1].Value;
            currentDir.Directories.Add(new Directory { Name = dirName, Parent = currentDir });
            continue;
        }

        // a number means a file
        string createFile = @"([0-9]+) (.+)";
        match = Regex.Match(line, createFile);

        if (match.Success)
        {
            var fileSize = int.Parse(match.Groups[1].Value);
            var fileName = match.Groups[2].Value;
            currentDir.Files.Add(new File { Name = fileName, size = fileSize }); ;
            continue;
        }

        if (line.Contains("ls"))
        {
            continue;
        }
        throw new Exception("Unexpected pattern");
    }
    
    var sum = CalculateSumSmallerOrEqual(filesystem, 100_000);
    Console.WriteLine($"sum is {sum}");
    // part 2
    /// The total disk space available to the filesystem is 70000000. To run the update, you need 
    /// unused space of at least 30000000. You need to find a directory you can delete that will 
    /// free up enough space to run the update.

    var diskSize = 70_000_000;
    var requiredSpace = 30_000_000;
    var usedSpace = filesystem.Size;

    var spaceToFree = -(diskSize - usedSpace - requiredSpace);

    // find the smallest dir wich is at least as big as spaceToFree
    List<Directory> flattenedList = new();
    GetAllDirs(filesystem, flattenedList);

    var result = flattenedList.Where(x => x.Size >= spaceToFree)
        .OrderBy(x => x.Size).First();

    Console.WriteLine($"Must delete: {result.Name}, size: {result.Size}");
}

void GetAllDirs(Directory root, List<Directory> flatList)
{
    foreach (var dir in root.Directories)
    {
        flatList.Add(dir);
        GetAllDirs(dir, flatList);
    }
}

int CalculateSumSmallerOrEqual(Directory dir, int maxSize)
{
    var sum = 0;

    foreach (var directory in dir.Directories)
    {
        sum += CalculateSumSmallerOrEqual(directory, maxSize);
    }

    if (dir.Size <= maxSize )
    {
        sum += dir.Size;
    }

    return sum;
}



class Directory
{
    public string Name { get; set; }
    public Directory? Parent { get; set; }
    public List<Directory> Directories { get; set; } = new();
    public List<File> Files { get; set; } = new();
    public int Size { get => GetSize(); }

    private int GetSize()
    {
        var filesSize = (int)Files.Sum(x => x.size);

        var dirSize = Directories.Select(x => x.Size).Sum();

        return filesSize + dirSize;
    }
}

class File
{
    public string Name { get; set; }
    public double size { get; set; }
}