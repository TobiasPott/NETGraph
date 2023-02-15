using System;

namespace Core
{

    public interface IEdge<E> where E : IEdge<E>, IEquatable<E>
    {
        int u { get; set; }
        int v { get; set; }
        bool directed { get; set; }

        E reversed();
    }

}
