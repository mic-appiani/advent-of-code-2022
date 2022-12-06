using System.Text.RegularExpressions;

List<char>[] crates = Enumerable.Range(0, 9).Select(x => new List<char>()).ToArray();
Stack<char>[] stacks = Enumerable.Range(0, 9).Select(x => new Stack<char>()).ToArray();

using (var sr = new StreamReader("input.txt"))
{
    // first 8 lines are initial configuration
    for (int i = 0; i < 8; i++)
    {
        var line = sr.ReadLine();

        int stackIndex = 0;

        for (int j = 1; j < line.Length; j += 4)
        {
            var letter = line[j];
            if (char.IsLetter(letter))
            {
                crates[stackIndex].Add(letter);
            }
            stackIndex++;
        }
    }

    // reverse collection
    for (int i = 0; i < crates.Length; i++)
    {
        var crate = crates[i];
        crate.Reverse();
        
        stacks[i] = new Stack<char>(crate);
        Console.WriteLine($"Peek: {stacks[i].Peek()}");

        foreach (var element in stacks[i])
        {
            Console.Write(element);
        }
        Console.WriteLine();
    }

    // skip 2
    sr.ReadLine();
    sr.ReadLine();

    const string pattern = @"move (\d+) from (\d+) to (\d+)";
    while (!sr.EndOfStream)
    {
        var line = sr.ReadLine();
        var match = Regex.Match(line, pattern);

        int amount = int.Parse(match.Groups[1].Value);
        int fromIdx = int.Parse(match.Groups[2].Value) - 1;
        int toIdx = int.Parse(match.Groups[3].Value) - 1;

        MoveBoxes(fromIdx, toIdx, amount);
    }

    // display the top of each stack

    Console.WriteLine("Solution:");
    for (int i = 0; i < crates.Length; i++)
    {
        Console.Write(stacks[i].Peek());
    }
}

void MoveBoxes(int from, int to, int amount)
{
    for (int i = 0; i < amount; i++)
    {
        var item = stacks[from].Pop();
        stacks[to].Push(item);
    }
}