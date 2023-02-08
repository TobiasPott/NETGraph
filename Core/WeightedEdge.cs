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


    public struct WeightedEdge<W> : IEdge, IWeightedEdge<W> where W : IEquatable<W>, IEquatable<WeightedEdge<W>>
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

        public IEdge reversed()
        {
            return new WeightedEdge<W>(v, u, directed, weight);
        }

        public override string ToString()
        {
            return $"{u} {weight}> {v}";
        }

        public static bool operator ==(WeightedEdge<W> lh, WeightedEdge<W> rh) => (lh.u == rh.u && lh.v == rh.v && lh.weight.Equals(rh.weight));
        public static bool operator !=(WeightedEdge<W> lh, WeightedEdge<W> rh) => (lh.u != rh.u || lh.v != rh.v || !lh.weight.Equals(rh.weight));

    }

}
