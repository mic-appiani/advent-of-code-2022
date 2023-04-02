using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Digraph
{
    /// <summary>
    /// Implemented startging implementation in Algorithms, fourth edition
    /// </summary>
    public class Digraph : IDigraph
    {
        private int _numVertices;
        private int _numEdges;
        private List<int>[] _adjacencyList;

        public Digraph(int numVertices)
        {
            this._numVertices = numVertices;
            _numEdges = 0;
            _adjacencyList = new List<int>[numVertices];

            for (int i = 0; i < _adjacencyList.Length; i++)
            {
                _adjacencyList[i] = new();
            }
        }

        public int VertexCount => _numVertices;

        public int EdgeCount => _numEdges;

        public void AddEdge(int v, int w)
        {
            _adjacencyList[v].Add(w);
            _numEdges++;
        }

        public IEnumerable<int> Adjacent(int v)
        {
            return _adjacencyList[v];
        }

        public IDigraph Reverse()
        {
            // not needed atm
            throw new NotImplementedException();
        }
    }
}
