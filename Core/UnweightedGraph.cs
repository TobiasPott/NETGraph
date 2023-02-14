using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    // ToDo: add interface with default constructor (or parametered constructor/new()) constraint
    public class UnweightedGraph<V> : GenericGraph<V, UnweightedEdge> where V : IEquatable<V>
    {
        public UnweightedGraph() : base() { }
        public UnweightedGraph(IEnumerable<V> vertices) : base(vertices)
        { }

        /// Initialize an UnweightedGraph consisting of a path which can be a cycle optionally.
        public UnweightedGraph(IEnumerable<V> path, bool directed = false, bool isCycle = false) : base(path)
        {
            if (vertices.Count >= 2)
            {
                for (int i = 0; i < vertices.Count - 1; i++)
                    this.addEdge(i, i + 1, directed);

                // add cycle edge if path is considered one
                if (isCycle)
                    this.addEdge(vertices.Count - 1, 0, directed);

            }
        }

        ///// This is a convenience method that adds an unweighted edge.
        //public void addEdge(int fromIndex, int toIndex, bool directed = false) => addEdge(new UnweightedEdge(fromIndex, toIndex, directed), directed);

        ///// This is a convenience method that adds an unweighted, undirected edge between the first occurence of two vertices. It takes O(n) time.
        //public void addEdge(V from, V to, bool directed = false)
        //{
        //    int fromIndex = indexOfVertex(from);
        //    if(fromIndex >= 0)
        //    {
        //        int toIndex = indexOfVertex(to);
        //        if (toIndex >= 0)
        //            addEdge(new UnweightedEdge(fromIndex, toIndex, directed), directed);
        //    }
        //}

        ///// Check whether there is an edge from one vertex to another vertex.
        //public bool edgeExists(int fromIndex, int toIndex) => edgeExists(new UnweightedEdge(fromIndex, toIndex, true));

        ///// Check whether there is an edge from one vertex to another vertex.
        //public bool edgeExists(V from, V to)
        //{
        //    int fromIndex = indexOfVertex(from);
        //    if(fromIndex >= 0)
        //    {
        //        int toIndex = indexOfVertex(to);
        //        if (toIndex >= 0)
        //            return edgeExists(fromIndex, toIndex);
        //    }
        //    return false;
        //}

    }


}
