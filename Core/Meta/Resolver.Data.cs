using System;
using NETGraph.Data;

namespace NETGraph.Core
{
    public interface IDataResolver
    {
        IData access();
        V resolve<V>();
        bool resolve<V>(out V value);
        void assign<V>(V value);
    }
    // ToDo: Implement connstant value resolver which ignores assignments and returns a scalar copy/instance of the genric type

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

        public IData access() => data.access(signature);
        public V resolve<V>() => data.resolve<V>(signature);
        public bool resolve<V>(out V value) => data.resolve<V>(signature, out value);

        public void assign<V>(V value)
        {
            if (signature.accessType == DataSignature.AccessTypes.Index)
                data.assign(signature.index, value);
            else if (signature.accessType == DataSignature.AccessTypes.Key)
                data.assign(signature.key, value);
            else if (signature.accessType == DataSignature.AccessTypes.Scalar)
                data.assign(value);
        }

    }

}

