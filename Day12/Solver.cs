using Shared.Digraph;
using Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Day12
{
    public class Solver
    {
        private readonly char[][] _grid;
        private readonly Digraph _graph;
        private readonly int _numRows;
        private readonly int _numCols;

        private readonly int _sourceRow;
        private readonly int _sourceCol;

        private readonly int _targetRow;
        private readonly int _targetCol;


        public Solver(char[][] grid)
        {
            _grid = grid;
            _graph = new Digraph(grid.Length * grid.First().Length);

            _numRows = grid.Length;
            _numCols = grid[0].Length;

            for (int row = 0; row < grid.Length; row++)
            {
                for (int col = 0; col < _numCols; col++)
                {
                    var value = _grid[row][col];

                    if (value == 'S')
                    {
                        _sourceRow = row;
                        _sourceCol = col;
                        _grid[row][col] = 'a';
                    }

                    if (value == 'E')
                    {
                        _targetRow = row;
                        _targetCol = col;
                        _grid[row][col] = 'z';
                    }

                    AddEdges(row, col);
                }
            }
        }

        private void AddEdges(int row, int col)
        {
            var source = Utils.Convert2DIndexToLinear(row, col, _numCols);

            // can not move diagonally!
            List<(int row, int col)> validPositions = new()
            {
                (row, col - 1), (row - 1, col), (row, col + 1), (row + 1, col)
            };

            foreach (var position in validPositions)
            {
                // out of bounds or reference cell
                if ((position.row < 0 || position.row >= _numRows) ||
                    (position.col < 0 || position.col >= _numCols) ||
                    (position.row == row && position.col == col))
                {
                    continue;
                }

                // normal case
                // check conditions for edges, if ok add edge
                var sourceValue = _grid[row][col];
                var targetValue = _grid[position.row][position.col];



                if (targetValue - sourceValue <= 1)
                {
                    var target = Utils.Convert2DIndexToLinear(position.row, position.col, _numCols);
                    _graph.AddEdge(source, target);
                }
            }

        }


        public void PrintGrid()
        {
            for (int row = 0; row < _numRows; row++)
            {
                for (int col = 0; col < _numCols; col++)
                {
                    if (row == _sourceRow && col == _sourceCol)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else if (row == _targetRow && col == _targetCol)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }

                    Console.Write(_grid[row][col]);
                }
                Console.WriteLine();
            }
        }

        public void PrintPath()
        {
            var path = GetShortestPath();

            for (int row = 0; row < _numRows; row++)
            {
                for (int col = 0; col < _numCols; col++)
                {
                    if (row == _sourceRow && col == _sourceCol)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else if (row == _targetRow && col == _targetCol)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (path.Contains(Utils.Convert2DIndexToLinear(row, col, _numCols)))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }

                    Console.Write(_grid[row][col]);
                }
                Console.WriteLine();
            }
        }

        public int SolvePart1()
        {
            var path = GetShortestPath();
            return path.Count;

        }

        private List<int> GetShortestPath()
        {
            var bfs = new BFS(_graph, Utils.Convert2DIndexToLinear(
                _sourceRow, _sourceCol, _numCols));

            return bfs.PathTo(Utils.Convert2DIndexToLinear(_targetRow, _targetCol, _numCols));
        }

        public int SolvePart2()
        {
            int shortest = int.MaxValue;

            for (int row = 0; row < _numRows; row++)
            {
                for (int col = 0; col < _numCols; col++)
                {
                    if (_grid[row][col] == 'a')
                    {
                        var bfs = new BFS(_graph, Utils.Convert2DIndexToLinear(row, col, _numCols));

                        var path = bfs.PathTo(
                            Utils.Convert2DIndexToLinear(_targetRow, _targetCol, _numCols));

                        if (path.Count > 0 && 
                            path.Count < shortest)
                        {
                            shortest = path.Count;
                        }
                    }
                }
            }

            return shortest;
        }
    }
}
