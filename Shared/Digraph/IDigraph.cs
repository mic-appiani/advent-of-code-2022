using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shared.Digraph
{
    /// <summary>
    /// Interface taken from Algorithms, fourth Edition
    /// </summary>
    public interface IDigraph
    {
        //number of vertices
        int VertexCount { get; }
        //number of edges
        int EdgeCount { get; }
        //add edge v->w to this digraph
        void AddEdge(int v, int w);
        //vertices connected to v by edges pointing from v
        IEnumerable<int> Adjacent(int v);
        //reverse of this digraph
        IDigraph Reverse();
    }
}
