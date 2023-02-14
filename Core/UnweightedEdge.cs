using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{

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


        public bool Equals(UnweightedEdge other)
        {
            return this.u == other.u && this.v == other.v;
        }

        public UnweightedEdge reversed()
        {
            return new UnweightedEdge(v, u, directed);
        }


        public override string ToString()
        {
            return $"{u} -> {v}";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(u, v, directed);
        }

        public override bool Equals(object obj)
        {
            return obj is UnweightedEdge edge && Equals(edge);
        }

        public static bool operator ==(UnweightedEdge lh, UnweightedEdge rh) => (lh.u == rh.u && lh.v == rh.v);
        public static bool operator !=(UnweightedEdge lh, UnweightedEdge rh) => (lh.u != rh.u || lh.v != rh.v);

    }
}
