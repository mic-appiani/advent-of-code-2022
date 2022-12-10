
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

// theres a bad bug for the grid size calculation, no time to fix sorry
//var grid = Grid.FromMoves(moves);
var grid = new Grid(10000, 10000, new int[] { 5000, 5000 }, moves);
grid.DrawZoomed(10);
grid.Simulate(draw: false);

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


    public Grid(int width, int height, int[] startPos, List<Move> moves)
    {
        _startPos = startPos;
        _moves = moves;

        _rope = Enumerable.Range(0, 10)
            .Select(x => new int[2] { _startPos[0], _startPos[1] })
            .ToArray();


        _visited = new bool[height, width];

        _visited[TailPos[0], TailPos[1]] = true;
    }

    /// <summary>
    /// Draws the head and the surrounding area
    /// </summary>
    public void DrawZoomed(int radius)
    {
        Console.Clear();

        // index overflow risk, too bad!
        var rowStart = HeadPos[0] - radius;
        var rowEnd = HeadPos[0] + radius;
        var colStart = HeadPos[1] - radius;
        var colEnd = HeadPos[1] + radius;

        for (int row = rowStart; row < rowEnd; row++)
        {
            for (int col = colStart; col < colEnd; col++)
            {
                Console.Write(GetChar(row, col));
            }

            Console.WriteLine();
        }

        Thread.Sleep(100);
    }

    private string GetChar(int row, int col)
    {
        if (HeadPos[0] == row && HeadPos[1] == col)
        {
            return "H";
        }

        for (int i = 1; i < _rope.Length; i++)
        {
            var position = _rope[i];

            if (position[0] == row && position[1] == col)
            {
                return i.ToString();
            }
        }

        if (_startPos[0] == row && _startPos[1] == col)
        {
            return ("s");
        }

        return ".";
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
                relPosition[0] -= move.Amount;
                if (maxUp > relPosition[0])
                {
                    maxUp = relPosition[0];
                }
            }

            if (move.Direction == "D")
            {
                relPosition[0] += move.Amount;
                if (maxDown < relPosition[0])
                {
                    maxDown = relPosition[0];
                }
            }

            if (move.Direction == "R")
            {
                relPosition[1] += move.Amount;
                if (maxRight < relPosition[1])
                {
                    maxRight = relPosition[1];
                }
            }

            if (move.Direction == "L")
            {
                relPosition[1] -= move.Amount;
                if (maxLeft > relPosition[1])
                {
                    maxLeft = relPosition[1];
                }
            }

        }

        return new Grid(
            maxRight - maxLeft + 1,
            -maxUp + maxDown + 1,
            new int[] { -maxUp, -maxLeft },
            moves);

    }

    internal void Simulate(bool draw = false)
    {
        for (int i = 0; i < _moves.Count; i++)
        {
            var move = _moves[i];

            MoveStepByStep(move, draw: draw);
        }

        var count = 0;
        foreach (var visited in _visited)
        {
            if (visited) { count++; }
        }

        Console.WriteLine($"Visited tiles by the tail: {count}");
    }


    private void MoveStepByStep(Move move, bool draw)
    {
        // must calculate the moves step by step and not jumping
        for (int i = 0; i < move.Amount; i++)
        {
            var headStartPos = HeadPos.ToArray();

            // Move the head
            if (move.Direction == "U")
            {
                HeadPos[0]--;
            }

            if (move.Direction == "D")
            {
                HeadPos[0]++;
            }

            if (move.Direction == "L")
            {
                HeadPos[1]--;
            }

            if (move.Direction == "R")
            {
                HeadPos[1]++;
            }

            if (draw)
            {
                DrawZoomed(10);
            }

            for (int knotIdx = 1; knotIdx < _rope.Length; knotIdx++)
            {
                var knotPos = _rope[knotIdx];
                var precedingKNot = _rope[knotIdx - 1];

                // Move the follower knot
                if (Math.Abs(precedingKNot[0] - knotPos[0]) > 1 ||
                    Math.Abs(precedingKNot[1] - knotPos[1]) > 1)
                {
                    var temp = knotPos.ToArray();

                    var verticalSeparation = knotPos[0] - precedingKNot[0];
                    var horizontalSeparation = knotPos[1] - precedingKNot[1];

                    // knot is below preceding
                    if (verticalSeparation > 1)
                    {
                        knotPos[0]--;

                        if (horizontalSeparation > 0)
                        {
                            knotPos[1]--;
                        }
                        else if (horizontalSeparation < 0)
                        {
                            knotPos[1]++;
                        }

                    }
                    else if (verticalSeparation < -1)
                    {
                        knotPos[0]++;

                        if (horizontalSeparation > 0)
                        {
                            knotPos[1]--;
                        }
                        else if (horizontalSeparation < 0)
                        {
                            knotPos[1]++;
                        }
                    }
                    else if (horizontalSeparation > 1)
                    {
                        knotPos[1]--;

                        if (verticalSeparation > 0)
                        {
                            knotPos[0]--;
                        }
                        else if (verticalSeparation < 0)
                        {
                            knotPos[0]++;
                        }
                    }
                    else if (horizontalSeparation < -1)
                    {
                        knotPos[1]++;

                        if (verticalSeparation > 0)
                        {
                            knotPos[0]--;
                        }
                        else if (verticalSeparation < 0)
                        {
                            knotPos[0]++;
                        }
                    }

                    if (knotIdx == _rope.Length - 1)
                    {
                        //DrawZoomed(10);
                        _visited[knotPos[0], knotPos[1]] = true;
                    }

                    if (draw)
                    {
                        DrawZoomed(10);
                    }
                }


            }
        }
    }
}