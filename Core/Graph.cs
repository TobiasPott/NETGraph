using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NETGraph
{

    public interface IGraph<V, E> where V : IEquatable<V> where E : IEdge<E>, IEquatable<E>
    {
        List<V> vertices { get; }
        Dictionary<int, List<E>> edges { get; }
        void addEdge(E edge);
        void buildWith(IEnumerable<V> vertices);
        IEnumerable<E> edgeList();
    }


    public abstract class Graph<V, E> : IGraph<V, E> where V : IEquatable<V> where E : IEdge<E>, IEquatable<E>, new()
    {
        public List<V> vertices { get; protected set; } = new List<V>();
        public Dictionary<int, List<E>> edges { get; protected set; } = new Dictionary<int, List<E>>();


        public int vertexCount => vertices.Count;
        public int edgeCount => edges.Sum(x => x.Value.Count);

        public V this[int index]
        {
            get => vertices[index];
            private set => vertices[index] = value;
        }



        protected Graph() { }

        /// Constructs an undirected UnweightedGraph isomorphic to a complete graph.
        public void buildWith(IEnumerable<V> vertices) => buildWith(vertices, false);
        /// Initialize an Graph with the given vertices without any edges.
        public void buildWith(IEnumerable<V> vertices, bool complete = false)
        {
            // graph gets cleared upon build
            this.vertices.Clear();
            this.edges.Clear();

            if (vertices != null)
                foreach (V vertex in vertices)
                    this.addVertex(vertex);

            /// Constructs an undirected UnweightedGraph isomorphic to a complete graph.
            if (complete)
                for (int i = 0; i < this.vertices.Count; i++)
                    for (int j = 0; j < i; j++)
                        this.addEdge(i, j);
        }
        /// Initialize an Graph consisting of a given path which can be a cycle optionally.
        public void buildWith(IEnumerable<V> path, bool isCycle, bool directed = false)
        {
            // graph gets cleared upon build
            this.vertices.Clear();
            this.edges.Clear();

            if (path != null)
                foreach (V vertex in path)
                    this.addVertex(vertex);

            if (path.Count() >= 2)
            {
                for (int i = 0; i < this.vertices.Count - 1; i++)
                    this.addEdge(i, i + 1, directed);

                // add cycle edge if path is considered one
                if (isCycle)
                    this.addEdge(this.vertices.Count - 1, 0, directed);
            }
        }

        /// Constructs an undirected UnweightedGraph isomorphic to a star graph.
        public void buildWith(V center, IEnumerable<V> leafs)
        {
            this.buildWith(leafs.Prepend(center));
            int leafCount = leafs.Count();
            if (leafCount > 0)
            {
                for (int i = 1; i < leafCount; i++)
                    addEdge(0, i);
            }
        }

        public V vertexAtIndex(int index) => vertices[index];
        public int indexOfVertex(V vertex) => vertices.IndexOf(vertex);

        /// Returns a list of all the edges, undirected edges are only appended once.
        public IEnumerable<E> edgeList()
        {
            List<E> results = new List<E>(this.edgeCount);
            foreach (int index in edges.Keys)
            {
                List<E> edgesForVertex = edges[index];
                foreach (E edge in edgesForVertex)
                {
                    if (edge.directed || edge.v >= edge.u)
                        results.Add(edge);
                }
            }
            return results;
        }

        /// Find all of the neighbors of a vertex at a given index.
        /// ToDo: Check actual functionality to get connected vertices list
        public IEnumerable<V> neighborsForIndex(int index)
        {
            IEnumerable<int> neighbourIndicesForIndex = edges[index].Select(x => x.v);
            return vertices.Where(x => neighbourIndicesForIndex.Contains(vertices.IndexOf(x)));
        }

        /// Find all of the neighbors of a given Vertex.
        public IEnumerable<V> neighborsForVertex(V vertex)
        {
            int index = indexOfVertex(vertex);
            if (index >= 0)
                return neighborsForIndex(index);
            return null;
        }


        /// Find all of the edges of a vertex at a given index.
        public List<E> edgesForIndex(int index) => edges[index];
        /// Find all of the edges of a given vertex.
        public List<E> edgesForVertex(V vertex)
        {
            int index = indexOfVertex(vertex);
            if (index >= 0)
                return edgesForIndex(index);
            return null;
        }

        public bool vertexInGraph(V vertex) => indexOfVertex(vertex) >= 0;

        /// Add a vertex to the graph.
        /// ToDo: Check for distinct add to vertex list as it may result in orphaned data and inconsistencies without cleanup
        public virtual int addVertex(V vertex)
        {
            vertices.Add(vertex);
            int index = vertexCount - 1;
            edges.Add(index, new List<E>());
            return index;
        }
        /// Add an edge to the graph.
        public virtual void addEdge(E edge)
        {
            // add edge to first vertex
            edges[edge.u].Add(edge);
            // check if undirected and different vertices and add edge to second vertex
            if (!edge.directed && edge.u != edge.v)
                edges[edge.v].Add(edge.reversed);
        }
        /// This is a convenience method that adds an unweighted edge.
        public virtual void addEdge(int fromIndex, int toIndex, bool directed = false)
        {
            E edge = new E();
            edge.u = fromIndex;
            edge.v = toIndex;
            edge.directed = directed;
            addEdge(edge);
        }
        /// This is a convenience method that adds an unweighted, undirected edge between the first occurence of two vertices. It takes O(n) time.
        public virtual void addEdge(V from, V to, bool directed = false)
        {
            int fromIndex = indexOfVertex(from);
            if (fromIndex >= 0)
            {
                int toIndex = indexOfVertex(to);
                if (toIndex >= 0)
                    addEdge(fromIndex, toIndex, directed);
            }
        }



        /// Removes all edges in both directions between vertices at indexes from & to.
        public void removeAllEdges(int from, int to, bool bidirectional = true)
        {
            if (edges.ContainsKey(from))
                edges[from].RemoveAll(x => x.v == to);
            if (bidirectional && edges.ContainsKey(to))
                edges[to].RemoveAll(x => x.v == from);
        }

        /// Removes all edges in both directions between two vertices.
        public void removeAllEdges(V from, V to, bool bidirectional = true)
        {
            int fromIndex = indexOfVertex(from);
            if (fromIndex >= 0)
            {
                int toIndex = indexOfVertex(to);
                if (toIndex >= 0)
                    removeAllEdges(fromIndex, toIndex, bidirectional);
            }
        }

        /// Remove the first edge found to be equal to *e*
        public void removeEdge(E edge)
        {
            //if(edges.ContainsKey(edge.u))
            edges[edge.u].Remove(edge);
        }

        /// Removes the first occurence of a vertex, all of the edges attached to it, and renumbers the indexes of the rest of the edges.
        public void removeVertex(V vertex)
        {
            int index = indexOfVertex(vertex);
            if (index >= 0)
                removeVertexAtIndex(index);
        }

        /// Check whether an edge is in the graph or not.
        public bool edgeExists(E edge) => edges[edge.u].Contains(edge);
        /// Check whether there is an edge from one vertex to another vertex.
        public bool edgeExists(int fromIndex, int toIndex)
        {
            E edge = new E();
            edge.u = fromIndex;
            edge.v = toIndex;
            return edgeExists(edge);
        }
        /// Check whether there is an edge from one vertex to another vertex.
        public bool edgeExists(V from, V to)
        {
            int fromIndex = indexOfVertex(from);
            if (fromIndex >= 0)
            {
                int toIndex = indexOfVertex(to);
                if (toIndex >= 0)
                    return edgeExists(fromIndex, toIndex);
            }
            return false;
        }


        /// Removes a vertex at a specified index, all of the edges attached to it, and renumbers the indexes of the rest of the edges.
        /// ToDo: Check if this works properly, unsure if I messed up code transcription
        public void removeVertexAtIndex(int index)
        {
            //remove all edges ending at the vertex, first doing the ones below it
            //renumber edges that end after the index
            List<int> toRemove = new List<int>();
            for (int j = 0; j < index; j++)
            {
                toRemove.Clear();
                for (int l = 0; l < edges[j].Count; l++)
                {
                    if (edges[j][l].v == index)
                    {
                        toRemove.Add(l);
                        continue;
                    }
                    if (edges[j][l].v > index)
                        edges[j][l].v -= 1;

                }
                toRemove.Reverse();
                foreach (int f in toRemove)
                    edges[j].RemoveAt(f);

            }

            //remove all edges after the vertex index wise
            //renumber all edges after the vertex index wise
            for (int j = (index + 1); j < edges.Count; j++)
            {
                toRemove.Clear();
                for (int l = 0; l < edges[j].Count; l++)
                {
                    if (edges[j][l].v == index)
                    {
                        toRemove.Add(l);
                        continue;
                    }
                    edges[j][l].u -= 1;
                    if (edges[j][l].v > index)
                        edges[j][l].v -= 1;

                }
                toRemove.Reverse();
                foreach (int f in toRemove)
                    edges[j].RemoveAt(f);

            }
            //remove the actual vertex and its edges
            edges.Remove(index);
            vertices.RemoveAt(index);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < vertices.Count; i++)
            {
                sb.AppendLine($"{vertices[i]} -> {string.Join(", ", neighborsForIndex(i))}");
            }
            return sb.ToString();
        }


        /// Returns a graph of the same type with all edges reversed.
        ///
        /// - returns: Graph of the same type with all edges reversed.
        public static G reversed<G, GV, GE>(G graph) where G : IGraph<GV, GE>, new() where GV : IEquatable<GV> where GE : IEdge<GE>, IEquatable<GE>, new()
        {
            G reversed = new G();
            reversed.buildWith(graph.vertices);
            foreach (GE edge in graph.edgeList())
                reversed.addEdge(edge.reversed);
            return reversed;
        }

    }


}
