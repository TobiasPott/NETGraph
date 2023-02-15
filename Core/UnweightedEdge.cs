using System;

namespace Core
{

    //ToDo: Consider renaming to DirectableEdge to provide a general base implementation for IEdge<>
    //ToDo: Consider implementing AttributedEdge (or similar name) to provide an IEdge implementation which also holds references to vertex attributes/components (by int/string identification)
    /// A basic unweighted edge.
    public struct UnweightedEdge : IEdge<UnweightedEdge>, IEquatable<UnweightedEdge>
    {
        public int u { get; set; }
        public int v { get; set; }
        public bool directed { get; set; }


        public UnweightedEdge(int u, int v, bool directed)
        {
            this.u = u;
            this.v = v;
            this.directed = directed;
        }



        public UnweightedEdge reversed() => new UnweightedEdge(v, u, directed);

        public override string ToString()
        { return $"{u} -> {v}"; }


        public bool Equals(UnweightedEdge other)
        {
            return this.u == other.u && this.v == other.v;
        }
        public override bool Equals(object obj)
        {
            return obj is UnweightedEdge edge && Equals(edge);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(u, v, directed);
        }


        public static bool operator ==(UnweightedEdge lh, UnweightedEdge rh) => (lh.u == rh.u && lh.v == rh.v);
        public static bool operator !=(UnweightedEdge lh, UnweightedEdge rh) => (lh.u != rh.u || lh.v != rh.v);

    }
}
