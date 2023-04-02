
using Day12;
using Shared.Digraph;

internal class Program
{
    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("input_day_12.txt");

        // "S" starting point, elevation a
        // "E" ending point, elevation z
        // a-z tiles
        // if elevation TO is more than 1 difference, it is disconnected

        var grid = lines.Select(x => x.ToCharArray()).ToArray();

        var solver = new Solver(grid);

        solver.PrintPath();

        int result = solver.SolvePart1();
        Console.WriteLine(result + 1);

        int result2 = solver.SolvePart2();
        Console.WriteLine(result2 + 1);
    }

    // check edges
}