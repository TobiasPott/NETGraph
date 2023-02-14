using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{

    public interface IWeightedEdge<W> where W : IEquatable<W>
    {

        //void init(int u, int v, bool directed, W weight); // this is replaced by defined constructor in implementation
        W weight { get; }

    }


    public struct WeightedEdge<W> : IEdge<WeightedEdge<W>>, IWeightedEdge<W>, IEquatable<WeightedEdge<W>> where W : IEquatable<W>, IEquatable<WeightedEdge<W>>
    {
        public int u { get; set; }
        public int v { get; set; }
        public bool directed { get; set; }

        public W weight { get; private set; }



        public WeightedEdge(int u, int v, bool directed, W weight)
        {
            this.u = u;
            this.v = v;
            this.directed = directed;
            this.weight = weight;
        }

        public WeightedEdge<W> reversed()
        {
            return new WeightedEdge<W>(v, u, directed, weight);
        }

        public override string ToString()
        {
            return $"{u} {weight}> {v}";
        }

        public override bool Equals(object obj)
        {
            return obj is WeightedEdge<W> edge && Equals(edge);
        }

        public bool Equals(WeightedEdge<W> other)
        {
            return u == other.u &&
                   v == other.v &&
                   directed == other.directed &&
                   EqualityComparer<W>.Default.Equals(weight, other.weight);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(u, v, directed, weight);
        }

        public static bool operator ==(WeightedEdge<W> lh, WeightedEdge<W> rh) => (lh.u == rh.u && lh.v == rh.v && lh.weight.Equals(rh.weight));
        public static bool operator !=(WeightedEdge<W> lh, WeightedEdge<W> rh) => (lh.u != rh.u || lh.v != rh.v || !lh.weight.Equals(rh.weight));

    }

}
