using System;

namespace NETGraph
{
    public struct Edge<N> : IEdge, IEquatable<IEdge>, IEquatable<Edge<N>> where N : IComparable<N>
    {
        public int u { get; set; }
        public int v { get; set; }
        public N uKnob { get; }
        public N vKnob { get; }
        public bool directed { get; set; }


        public Edge(int u, int v, N uKnob, N vKnob)
        {
            this.u = u;
            this.v = v;
            this.uKnob = uKnob;
            this.vKnob = vKnob;
            this.directed = true;
        }


        public IEdge reversed => new Edge<N>(v, u, vKnob, uKnob);

        public override string ToString()
        { return $"{u}.{uKnob} -> {v}.{vKnob}"; }


        public bool Equals(IEdge other)
        {
            if (other is Edge<N>)
                return this.Equals((Edge)other);
            return false;
        }
        public bool Equals(Edge<N> other)
        {
            return this.u == other.u && this.v == other.v && this.uKnob.Equals(other.uKnob) && this.vKnob.Equals(other.vKnob);
        }
        public override bool Equals(object obj)
        {
            return obj is Edge<N> edge && Equals(edge);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(u, v, uKnob, vKnob, directed);
        }


        public static bool operator ==(Edge<N> lh, Edge<N> rh) => (lh.u == rh.u && lh.v == rh.v && lh.uKnob.Equals(rh.uKnob) && lh.vKnob.Equals(rh.vKnob));
        public static bool operator !=(Edge<N> lh, Edge<N> rh) => (lh.u != rh.u || lh.v != rh.v || !lh.uKnob.Equals(rh.uKnob) || !lh.vKnob.Equals(rh.vKnob));

    }
}

