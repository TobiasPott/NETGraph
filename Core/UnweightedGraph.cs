using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{

    public class UnweightedGraph<V> : GenericGraph<V, UnweightedEdge> where V : IEquatable<V>
    {
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
                if(isCycle)
                    this.addEdge(vertices.Count - 1, 0, directed);

            }
        }

        /// Add an edge to the graph.
        public override void addEdge(UnweightedEdge edge, bool directed = false)
        {
            edges[edge.u].Add(edge);
            if (!directed && edge.u != edge.v)
                edges[edge.v].Add(edge.reversed());
        }

        /// Add a vertex to the graph.
        public override int addVertex(V vertex)
        {
            vertices.Add(vertex);
            int index = vertices.Count - 1;
            edges.Add(index, new List<UnweightedEdge>());
            return index;
        }

        /// This is a convenience method that adds an unweighted edge.
        public void addEdge(int fromIndex, int toIndex, bool directed = false) => addEdge(new UnweightedEdge(fromIndex, toIndex, directed), directed);

        /// This is a convenience method that adds an unweighted, undirected edge between the first occurence of two vertices. It takes O(n) time.
        public void addEdge(V from, V to, bool directed = false)
        {
            int fromIndex = indexOfVertex(from);
            if(fromIndex >= 0)
            {
                int toIndex = indexOfVertex(to);
                if (toIndex >= 0)
                    addEdge(new UnweightedEdge(fromIndex, toIndex, directed), directed);
            }
        }

        /// Check whether there is an edge from one vertex to another vertex.
        public bool edgeExists(int fromIndex, int toIndex) => edgeExists(new UnweightedEdge(fromIndex, toIndex, true));

        /// Check whether there is an edge from one vertex to another vertex.
        public bool edgeExists(V from, V to)
        {
            int fromIndex = indexOfVertex(from);
            if(fromIndex >= 0)
            {
                int toIndex = indexOfVertex(to);
                if (toIndex >= 0)
                    return edgeExists(fromIndex, toIndex);
            }
            return false;
        }

    }


}
