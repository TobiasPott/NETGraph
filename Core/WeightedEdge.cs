using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{

    public interface IWeightedEdge<W> where W : IEquatable<W>
    {

        void init(int u, int v, bool directed, W weight);
        W weight { get; }

    }

}
