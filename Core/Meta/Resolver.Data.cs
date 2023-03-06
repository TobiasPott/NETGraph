using System;
using NETGraph.Data;

namespace NETGraph.Core.Meta
{
    public interface IDataResolver
    {
        V resolve<V>();
        void assign<V>(V value);
    }

    /// <summary>
    /// Provides an actual handle to a data reference and data signature to resolve into an underlying type instance
    /// </summary>
    // ToDo: Ponder about a way to extend resolver to resolve IEnumerable<T> for index or key ranges (e.g. 0 - 3 as index range)
    public struct DataResolver : IDataResolver
    {
        DataSignature signature;
        IData data;

        public DataResolver(IData data, DataSignature signature)
        {
            this.signature = signature;
            this.data = data;
        }
        public DataResolver(IData data, string signature)
        {
            this.signature = new DataSignature(signature);
            this.data = data;
        }

        public V resolve<V>() => data.resolve<V>(signature);
        public void assign<V>(V value) => data.assign(signature, value);

    }

}

