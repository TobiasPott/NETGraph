using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Core
{

    public interface IGraph<V, E> where V : IEquatable<V> where E : IEdge<E>, IEquatable<E>
    {
        List<V> vertices { get; set; }
        Dictionary<int, List<E>> edges { get; set; }

        // init is replaced by abstract class constructor enforcing parameter requirement and distinct creation of vertices list
        //void init(ICollection<V> vertices);
        void addEdge(E edge, bool directed);
    }


    public abstract class Graph<V, E> : IGraph<V, E> where V : IEquatable<V> where E : IEdge<E>, IEquatable<E>
    {
        public abstract List<V> vertices { get; set; }
        public abstract Dictionary<int, List<E>> edges { get; set; }

        protected Graph() { }
        public Graph(IEnumerable<V> vertices)
        {
            if (vertices != null)
                this.vertices.AddRange(vertices.Distinct());
            // add edge lists for each vertex
            for(int i = 0; i < this.vertices.Count; i++)
                this.edges.Add(i, new List<E>());
        }


        public int vertexCount => vertices.Count;
        public int edgeCount => edges.Sum(x => x.Value.Count);

        public V this[int index]
        {
            get => vertices[index];
            private set => vertices[index] = value;
        }


        public V vertexAtIndex(int index) => vertices[index];
        public int indexOfVertex(V vertex) => vertices.IndexOf(vertex);

        /// Returns a list of all the edges, undirected edges are only appended once.
        public List<E> edgeList()
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
            if (vertex != null)
            {
                vertices.Add(vertex);

                int index = vertexCount - 1;
                edges.Add(index, new List<E>());
                return index;
            }
            return -1;
        }

        /// Add an edge to the graph.
        /// ToDo: check for distinct add to edge list
        public virtual void addEdge(E edge, bool directed = false)
        {
            edges[edge.u].Add(edge);
            if (!directed || edge.u != edge.v)
                edges[edge.v].Add((E)edge.reversed());

        }

        /// Removes all edges in both directions between vertices at indexes from & to.
        /// ToDo: Check for index bound or key existence to prevent errors and exceptions
        public void removeAllEdges(int from, int to, bool bidirectional = true)
        {
            edges[from].RemoveAll(x => x.v == to);
            if (bidirectional)
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
    }



    // Base class implementation providing interface properties and constructor
    public class GenericGraph<V, E> : Graph<V, E> where V : IEquatable<V> where E : IEdge<E>, IEquatable<E>
    {
        protected GenericGraph() : base() { }
        public GenericGraph(IEnumerable<V> vertices) : base(vertices)
        { }

        public override List<V> vertices { get; set; } = new List<V>();
        public override Dictionary<int, List<E>> edges { get; set; } = new Dictionary<int, List<E>>();
    }


}
