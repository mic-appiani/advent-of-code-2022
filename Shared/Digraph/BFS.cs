using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Digraph
{
    public class BFS
    {
        private bool[] _visited;
        private int[] _edgeTo;
        private readonly int _source;


        /// <param name="graph"></param>
        /// <param name="source">The id of the source vertex</param>
        public BFS(Digraph graph, int source)
        {
            _visited = new bool[graph.VertexCount];
            _edgeTo = new int[graph.VertexCount];
            _source = source;

            BreadthFirstSearch(graph, source);
        }

        private void BreadthFirstSearch(Digraph graph, int source)
        {
            Queue<int> queue = new Queue<int>();

            _visited[source] = true;
            queue.Enqueue(source);

            while (queue.Count > 0)
            {
                int vertex = queue.Dequeue();

                foreach (int adjVertex in graph.Adjacent(vertex))
                    if (!_visited[adjVertex])
                    {
                        _edgeTo[adjVertex] = vertex;
                        _visited[adjVertex] = true;
                        queue.Enqueue(adjVertex);
                    }
            }
        }


        public bool HasPathTo(int vertex)
        {
            return _visited[vertex];
        }

        public List<int> PathTo(int vertex)
        {
            if (!HasPathTo(vertex))
            {
                return new();
            };

            Stack<int> path = new();

            for (int x = vertex; x != _source; x = _edgeTo[x])
            {
                path.Push(x);
            }

            path.Push(_source);

            return path.ToList();
        }

    }
}
