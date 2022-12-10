
using System;
using static System.Formats.Asn1.AsnWriter;

List<Move> moves = new();

using (var sr = new StreamReader("input.txt"))
{
    while (!sr.EndOfStream)
    {
        var line = sr.ReadLine();
        var split = line.Split(' ').ToArray();
        moves.Add(new Move { Direction = split[0], Amount = int.Parse(split[1]) });
    }
}

var movesRight = moves.Where(x => x.Direction == "R").Sum(x => x.Amount);
var movesLeft = moves.Where(x => x.Direction == "L").Sum(x => x.Amount);
var width = movesRight - movesLeft + 1;

var movesUp = moves.Where(x => x.Direction == "U").Sum(x => x.Amount);
var movesDown = moves.Where(x => x.Direction == "D").Sum(x => x.Amount);

var grid = Grid.FromMoves(moves);
grid.DrawZoomed(6);
grid.Simulate();

Console.WriteLine();


class Move
{
    public string Direction { get; set; } = string.Empty;
    public int Amount { get; set; }
}

class Grid
{
    private int[] _startPos;
    private int[][] _rope;
    private List<Move> _moves;
    private bool[,] _visited;

    private int[] HeadPos { get => _rope[0]; }
    private int[] TailPos { get => _rope.Last(); }


    private Grid(int width, int height, int[] startPos, List<Move> moves)
    {
        _startPos = startPos;
        _moves = moves;

        _rope = Enumerable.Range(0,2)
            .Select(x => new int[2] { _startPos[0], _startPos[1] })
            .ToArray();

        _visited = new bool[height, width];
        _visited[TailPos[1], TailPos[0]] = true;
    }

    /// <summary>
    /// Draws the head and the surrounding area
    /// </summary>
    public void DrawZoomed(int radius)
    {
        Console.Clear();

        // index overflow risk, too bad!
        var rowStart = HeadPos[1] - radius;
        var rowEnd = HeadPos[1] + radius;
        var colStart = HeadPos[0] - radius;
        var colEnd = HeadPos[0] + radius;

        for (int row = rowStart; row < rowEnd; row++)
        {
            for (int col = colStart; col < colEnd; col++)
            {
                if (HeadPos[1] == row && HeadPos[0] == col)
                {
                    Console.Write("H");
                    continue;
                }

                for (int i = 1; i < _rope.Length; i++)
                {
                    var position = _rope[i];
                    
                    if (position[1] == row && position[0] == col)
                    {
                        Console.Write(i);
                        continue;
                    }
                }
                
                if (_startPos[1] == row && _startPos[0] == col)
                {
                    Console.Write("s");
                    continue;
                }

                Console.Write(".");
            }

            Console.WriteLine();
        }

        Thread.Sleep(200);
    }


    public static Grid FromMoves(List<Move> moves)
    {
        var maxUp = 0;
        var maxDown = 0;
        var maxLeft = 0;
        var maxRight = 0;
        var relPosition = new int[] { 0, 0 }; // x, y

        for (int i = 0; i < moves.Count; i++)
        {
            var move = moves[i];

            if (move.Direction == "U")
            {
                relPosition[1] += move.Amount;
                if (maxUp < relPosition[1])
                {
                    maxUp = relPosition[1];
                }
            }

            if (move.Direction == "D")
            {
                relPosition[1] -= move.Amount;
                if (maxDown > relPosition[1])
                {
                    maxDown = relPosition[1];
                }
            }

            if (move.Direction == "R")
            {
                relPosition[0] += move.Amount;
                if (maxRight < relPosition[0])
                {
                    maxRight = relPosition[0];
                }
            }

            if (move.Direction == "L")
            {
                relPosition[0] -= move.Amount;
                if (maxLeft > relPosition[0])
                {
                    maxLeft = relPosition[0];
                }
            }



        }

        return new Grid(
            maxRight - maxLeft + 1,
            maxUp - maxDown + 1,
            new int[] { -maxLeft, maxUp },
            moves);

    }

    internal void Simulate()
    {
        for (int i = 0; i < _moves.Count; i++)
        {
            var move = _moves[i];

            MoveStepByStep(move, draw: false);

        }

        var count = 0;
        foreach (var visited in _visited)
        {
            if (visited) { count++; }
        }

        Console.WriteLine($"Visited tiles by the tail: {count}");
    }


    private void MoveStepByStep(Move move, bool draw = false)
    {
        // must calculate the moves step by step and not jumping
        for (int i = 0; i < move.Amount; i++)
        {
            var headStartPos = HeadPos.ToArray();

            // Move the head
            if (move.Direction == "U")
            {
                HeadPos[1]--;
            }

            if (move.Direction == "D")
            {
                HeadPos[1]++;
            }

            if (move.Direction == "L")
            {
                HeadPos[0]--;
            }

            if (move.Direction == "R")
            {
                HeadPos[0]++;
            }

            if (draw)
            {
                DrawZoomed(6);
            }

            var precedingKnotLastPos = headStartPos;

            for (int knotIdx = 1; knotIdx < _rope.Length; knotIdx++)
            {
                var knotPos = _rope[knotIdx];
                var precedingKNot = _rope[knotIdx - 1];

                // Move the follower knot
                if (Math.Abs(precedingKNot[0] - knotPos[0]) > 1 ||
                    Math.Abs(precedingKNot[1] - knotPos[1]) > 1)
                {
                    knotPos[0] = precedingKnotLastPos[0];
                    knotPos[1] = precedingKnotLastPos[1];

                    precedingKnotLastPos = knotPos.ToArray();

                    

                    if (draw)
                    {
                        DrawZoomed(6);
                    }
                }
            }

            _visited[TailPos[1], TailPos[0]] = true;
        }
    }
}