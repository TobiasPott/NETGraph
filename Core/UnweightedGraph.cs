using System;
using System.Collections.Generic;

namespace NETGraph.Graphs
{

    /// An implementation of Graph with some convenience methods for adding and removing UnweightedEdges.
    public class UnweightedGraph<V> : Graph<V, Edge> where V : IEquatable<V>
    {

        public UnweightedGraph() : base() { }
        /// Initialize an UnweightedGraph consisting of a path which can be a cycle optionally.
        public UnweightedGraph(IEnumerable<V> path, bool isCycle, bool directed = false) : base()
        {
            buildWith(path, isCycle, directed);
        }

        /// This is a convenience method that adds an unweighted edge.
        public override void addEdge(int fromIndex, int toIndex, bool directed = false) => addEdge(new Edge(fromIndex, toIndex, directed));
        /// This is a convenience method that adds an unweighted, undirected edge between the first occurence of two vertices. It takes O(n) time.
        public override void addEdge(V from, V to, bool directed = false)
        {
            int fromIndex = indexOfVertex(from);
            if (fromIndex >= 0)
            {
                int toIndex = indexOfVertex(to);
                if (toIndex >= 0)
                    addEdge(new Edge(fromIndex, toIndex, directed));
            }
        }


        /// Returns a graph of the same type with all edges reversed.
        ///
        /// - returns: Graph of the same type with all edges reversed.
        public UnweightedGraph<V> reversed() => this.reversed<UnweightedGraph<V>, V, Edge>();
    }

}
