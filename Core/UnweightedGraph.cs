using System;
using System.Collections.Generic;

namespace NETGraph
{

    /// An implementation of Graph with some convenience methods for adding and removing UnweightedEdges.
    public class UnweightedGraph<V> : Graph<V, Edge> where V : IEquatable<V>
    {
        public override List<V> vertices { get; protected set; } = new List<V>();
        public override Dictionary<int, List<Edge>> edges { get; protected set; } = new Dictionary<int, List<Edge>>();


        public UnweightedGraph() : base() { }
        public UnweightedGraph(IEnumerable<V> vertices) : base(vertices)
        { }
        /// Initialize an UnweightedGraph consisting of a path which can be a cycle optionally.
        public UnweightedGraph(IEnumerable<V> path, bool isCycle, bool directed = false) : base(path)
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

    }

}
