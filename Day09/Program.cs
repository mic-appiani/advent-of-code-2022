
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
grid.Simulate();

Console.WriteLine();


class Move
{
    public string Direction { get; set; } = string.Empty;
    public int Amount { get; set; }
}

class Grid
{
    private int _height;
    private int _width;
    private int[] _startPos;
    private int[] _headPos;
    private int[] _tailPos;
    private List<Move> _moves;

    private Grid(int width, int height, int[] startPos, List<Move> moves)
    {
        _width = width;
        _height = height;
        _startPos = startPos;
        _headPos = startPos.ToArray();
        _tailPos = startPos.ToArray();
        _moves = moves;
    }

    public void Draw()
    {
        var scale = 6;
        for (int i = 0; i < _height / scale; i++)
        {
            for (int j = 0; j < _width / scale; j++)
            {
                if (_headPos[1] / scale == i && _headPos[0] / scale == j)
                {
                    Console.Write("H");
                    continue;
                }
                else if (_tailPos[1] / scale == i && _tailPos[0] / scale == j)
                {
                    Console.Write("T");
                    continue;
                }
                else if (_startPos[1] / scale == i && _startPos[0] / scale == j)
                {
                    Console.Write("s");
                    continue;
                }


                Console.Write(".");
            }

            Console.WriteLine();
        }
    }

    /// <summary>
    /// Draws the head and the surrounding area
    /// </summary>
    public void DrawZoomed(int radius) 
    {
        // index overflow risk, too bad!
        var rowStart = _headPos[1] - radius;
        var rowEnd = _headPos[1] + radius;
        var colStart = _headPos[0] - radius;
        var colEnd = _headPos[0] + radius;

        for (int row = rowStart; row < rowEnd; row++)
        {
            for (int col = colStart;  col < colEnd; col++)
            {
                if (_headPos[1] == row && _headPos[0] == col)
                {
                    Console.Write("H");
                    continue;
                }
                else if (_tailPos[1] == row && _tailPos[0] == col)
                {
                    Console.Write("T");
                    continue;
                }
                else if (_startPos[1] == row && _startPos[0] == col)
                {
                    Console.Write("s");
                    continue;
                }

                Console.Write(".");
            }

            Console.WriteLine();
        }
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
            _headPos[1]--;
            DrawZoomed(6);
            Thread.Sleep(200);
            Console.Clear();
        }
    }
}