using System.Security.Cryptography.X509Certificates;

var part1Sum = 0;
var part2Sum = 0;

List<string> groups = new();

using (var sr = new StreamReader("input.txt"))
{
    /// strings are always even
    /// there is only one item that is in both bags
    /// 
    /// To help prioritize item rearrangement, every item type can be converted to a priority:
    /// Lowercase item types a through z have priorities 1 through 26.
    /// Uppercase item types A through Z have priorities 27 through 52
    /// 
    /// Find the item type that appears in both compartments of each rucksack. 
    /// What is the sum of the priorities of those item types?

    while (!sr.EndOfStream)
    {
        var line = sr.ReadLine() ?? string.Empty;
        groups.Add(line);

        // split in half
        var bag1 = line.Substring(0, line.Length / 2);
        var bag2 = line.Substring(line.Length / 2);

        // two char array
        var charInBoth = bag1.Intersect(bag2).Single();
        part1Sum += GetPriority(charInBoth);
    }

    Console.WriteLine($" The sum of priorities for part 1 is {part1Sum}");

    // Part 2

    /// within each group of three Elves, the badge is the only item type carried by all three Elves.
    /// That is, if a group's badge is item type B, then all three Elves will have item type B 
    /// somewhere in their rucksack, and at most two of the Elves will be carrying any other item type.

    // every 3 lines is a group
    // find the char that is in all 3 lines for each group

    for (int i = 0; i< groups.Count; i += 3)
    {
        var intersection = groups[i].Intersect(groups[i + 1]);

        var commonChar = intersection.Intersect(groups[i + 2]).Single();

        part2Sum += GetPriority(commonChar);
    }

    Console.WriteLine($" The sum of priorities for part 2 is {part2Sum}");
}

int GetPriority(char charInBoth)
{
    if (char.IsLower(charInBoth))
    {
        return (int)charInBoth - 96;
    }

    return (int)charInBoth - 38;
}