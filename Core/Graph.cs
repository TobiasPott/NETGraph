using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{

    public interface IGraph<V, E> where V : IEquatable<V> where E: IEdge, IEquatable<E>
    {
        List<V> vertices { get; set; }
        Dictionary<int, List<E>> edges { get; set; }

        void init(ICollection<V> vertices);
        bool addEdge(E edge, bool directed);

    }


}
