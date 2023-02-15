using System;

namespace NETGraph
{
    public struct NamedEdge<N> : IEdge<NamedEdge<N>>, IEquatable<NamedEdge<N>> where N : IComparable<N>
    {
        public int u { get; set; }
        public int v { get; set; }
        public N uName { get; }
        public N vName { get; }
        public bool directed { get; set; }


        public NamedEdge(int u, int v, N uName, N vName)
        {
            this.u = u;
            this.v = v;
            this.uName = uName;
            this.vName = vName;
            this.directed = true;
        }



        public NamedEdge<N> reversed => new NamedEdge<N>(v, u, vName, uName);

        public override string ToString()
        { return $"{u}.{uName} -> {v}.{vName}"; }


        public bool Equals(NamedEdge<N> other)
        {
            return this.u == other.u && this.v == other.v && this.uName.Equals(other.uName) && this.vName.Equals(other.vName);
        }
        public override bool Equals(object obj)
        {
            return obj is NamedEdge<N> edge && Equals(edge);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(u, v, uName, vName, directed);
        }


        public static bool operator ==(NamedEdge<N> lh, NamedEdge<N> rh) => (lh.u == rh.u && lh.v == rh.v && lh.uName.Equals(rh.uName) && lh.vName.Equals(rh.vName));
        public static bool operator !=(NamedEdge<N> lh, NamedEdge<N> rh) => (lh.u != rh.u || lh.v != rh.v || !lh.uName.Equals(rh.uName) || !lh.vName.Equals(rh.vName));

    }
}

