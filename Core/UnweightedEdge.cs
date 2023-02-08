using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{

    public struct UnweightedEdge : IEdge, IEquatable<UnweightedEdge>
    {
        public int u { get; set; }
        public int v { get; set; }
        public bool directed { get; set; }


        #region Ctors
        public UnweightedEdge(int u, int v, bool directed)
        {
            this.u = u;
            this.v = v;
            this.directed = directed;
        }
        #endregion


        public bool Equals(UnweightedEdge other)
        {
            return this.u == other.u && this.v == other.v;
        }

        public IEdge reversed()
        {
            return new UnweightedEdge(v, u, directed);
        }

        public override string ToString()
        {
            return $"{u} -> {v}";
        }
    }
}
