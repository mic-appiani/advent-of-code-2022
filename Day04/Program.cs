
using System.Diagnostics.CodeAnalysis;
/// In how many assignment pairs does one range fully contain the other?
/// 
int completeOverlapCount = 0;
int totalLines = 0;
int noOverlap = 0;

using (var sr = new StreamReader("input.txt"))
{
    while (!sr.EndOfStream)
    {
        var line = sr.ReadLine() ?? string.Empty;

        var couple = line.Split(',').ToArray();
        int[] elf1 = couple[0].Split("-").Select(x => Int32.Parse(x)).ToArray();
        int[] elf2 = couple[1].Split("-").Select(x => Int32.Parse(x)).ToArray();

        // complete overlap

        if (elf1[0] >= elf2[0] && elf1[1] <= elf2[1] ||
            elf2[0] >= elf1[0] && elf2[1] <= elf1[1])
        {
            completeOverlapCount++;
        }

        // part2
        totalLines++;

        if (elf1[1] < elf2[0] || elf1[0] > elf2[1])
        {
            noOverlap++;
        }
        
    }

    Console.WriteLine($"The complete overlap count is {completeOverlapCount}");
    Console.WriteLine($"No overlap count: {noOverlap}");
    Console.WriteLine($"The partial overlap count is {totalLines - noOverlap}");

}