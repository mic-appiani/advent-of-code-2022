// maybe 1?
using System;
using System.Runtime.InteropServices;

var cycle = 1;
var xRegister = 1;

List<int> report = new() { 1 };

var reportAtCycle = new int[] { 20, 60, 100, 140, 180, 220 };
var reportIdx = 0;
var sum = 0;

var screen = new Screen();
var arr = new char[6, 40];


using (var sr = new StreamReader("input.txt"))
{
    while (!sr.EndOfStream)
    {
        screen.ReadPixel(cycle, xRegister);
        //CheckSignalStrength();

        var line = sr.ReadLine();
        //Console.WriteLine($"Cycle:{cycle} {line}");

        if (line.Contains("noop"))
        {
            cycle++;
            continue;
        }

        // in any other case is addx
        var value = int.Parse(line.Split(' ')[1]);
        Console.WriteLine($"Value: {value}");

        cycle++;
        Console.WriteLine($"Cycle: {cycle}, register: {xRegister}");

        screen.ReadPixel(cycle, xRegister);
        //CheckSignalStrength();
        cycle++;

        xRegister += value;
        report.Add(xRegister);
        Console.WriteLine($"Cycle: {cycle}, register: {xRegister}");
    }

    //Console.WriteLine($"Sum: {sum}");
    Console.WriteLine(report.Min());
    Console.WriteLine(report.Max());
    screen.Render();

}

void CheckSignalStrength()
{
    if (reportIdx < reportAtCycle.Length &&
            cycle == reportAtCycle[reportIdx])
    {
        //Console.WriteLine($"Signal strenght at cycle {reportAtCycle[reportIdx]}: " +
        //    $"{xRegister} * {cycle} =  {xRegister * cycle}");
        sum += xRegister * cycle;
        reportIdx++;
    }
}

public class Screen
{
    private const int _screenHeight = 6;
    private const int _screenWidth = 40;
    private char[,] _buffer = new char[6, 40];

    public Screen()
    {
        _buffer.Initialize();
    }

    //The left-most pixel in each row is in position 0, and the right-most pixel in each row is in
    //position 39.
    public void ReadPixel(int cycle, int register)
    {
        var row = (cycle -1) / 40;
        var col = (cycle -1) % 40;

        if (HasLitPixel(col, register)) {
            _buffer[row, col] = '#';
            return;
        }

        _buffer[row,col] = '.';
    }

    public void Render()
    {
        var height = _buffer.GetLength(0);
        var width = _buffer.GetLength(1);

        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                Console.Write(_buffer[row,col]);
            }
            Console.WriteLine();
        }
    }

    // If the sprite is positioned such that one of its three pixels is the pixel currently being
    // drawn, the screen produces a lit pixel (#); otherwise, the screen leaves the pixel dark (.). 
    bool HasLitPixel(int pointer, int register)
    {
        // check right and left, consider overflow
        if ( register - 1 <= pointer && pointer <= register + 1 )
        {
            return true;
        }

        return false;
    }


}