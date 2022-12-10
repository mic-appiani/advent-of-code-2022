// maybe 1?
var cycle = 1;
var xRegister = 1;

var reportAtCycle = new int[] { 20, 60, 100, 140, 180, 220 };
var reportIdx = 0;
var sum = 0;


using (var sr = new StreamReader("input.txt"))
{
    while (!sr.EndOfStream)
    {
        CheckSignalStrength();

        var line = sr.ReadLine();
        Console.WriteLine($"Cycle:{cycle} {line}");

        if (line.Contains("noop"))
        {
            cycle++;
            continue;
        }

        // in any other case is addx

        var value = int.Parse(line.Split(' ')[1]);

        cycle++;
        CheckSignalStrength();
        cycle++;

        xRegister += value;
        Console.WriteLine($"x register is: {xRegister}");
    }
    Console.WriteLine($"Sum: {sum}");
}

void CheckSignalStrength()
{
    if (reportIdx < reportAtCycle.Length &&
            cycle == reportAtCycle[reportIdx])
    {
        Console.WriteLine($"Signal strenght at cycle {reportAtCycle[reportIdx]}: " +
            $"{xRegister} * {cycle} =  {xRegister * cycle}");
        sum += xRegister * cycle;
        reportIdx++;
    }
}