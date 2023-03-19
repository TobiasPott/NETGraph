using System;

namespace NETGraph.Core.Meta
{
    public interface IResolver
    {
        V resolve<V>();
        V resolve<V>(DataAccessor accessor);

        void assign<V>(V value);
        void assign<V>(DataAccessor accessor, V value);
        void assign(DataAccessor accessor, object value);
    }
}

