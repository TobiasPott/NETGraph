using System;

namespace NETGraph
{

    public interface IEdge<E> where E : IEdge<E>, IEquatable<E>
    {
        int u { get; set; }
        int v { get; set; }
        bool directed { get; set; }

        E reversed { get; }
    }

    //ToDo: Consider implementing AttributedEdge (or similar name) to provide an IEdge implementation which also holds references to vertex attributes/components (by int/string identification)
    /// A basic unweighted edge.
    public struct Edge : IEdge<Edge>, IEquatable<Edge>
    {
        public int u { get; set; }
        public int v { get; set; }
        public bool directed { get; set; }


        public Edge(int u, int v, bool directed)
        {
            this.u = u;
            this.v = v;
            this.directed = directed;
        }



        public Edge reversed => new Edge(v, u, directed);

        public override string ToString()
        { return $"{u} -> {v}"; }


        public bool Equals(Edge other)
        {
            return this.u == other.u && this.v == other.v;
        }
        public override bool Equals(object obj)
        {
            return obj is Edge edge && Equals(edge);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(u, v, directed);
        }


        public static bool operator ==(Edge lh, Edge rh) => (lh.u == rh.u && lh.v == rh.v);
        public static bool operator !=(Edge lh, Edge rh) => (lh.u != rh.u || lh.v != rh.v);

    }
}
