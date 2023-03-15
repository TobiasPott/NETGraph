using System;

namespace NETGraph.Core.Meta
{
    public interface IResolver
    {
        V resolve<V>();
        void assign<V>(V value);
    }


}

