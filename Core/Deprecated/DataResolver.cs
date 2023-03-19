using System;

namespace NETGraph.Core.Meta
{
    ///// <summary>
    ///// Provides an actual handle to a data reference and data signature to resolve into an underlying type instance
    ///// </summary>
    //// ToDo: Ponder about a way to extend resolver to resolve IEnumerable<T> for index or key ranges (e.g. 0 - 3 as index range)
    //public struct DataWithAccessor : IResolver
    //{
    //    DataAccessor accessor;
    //    IData data;


    //    // ToDo: Move to extension method for IData type
    //    //      extension method should try to auto generate resolver based on data type and specified accessor

    //    public DataWithAccessor(IData data, DataAccessor accessor)
    //    {
    //        this.accessor = accessor;
    //        this.data = data;
    //    }
    //    public DataWithAccessor(IData data, string accessor)
    //    {
    //        this.accessor = new DataAccessor(accessor);
    //        this.data = data;
    //    }

    //    public V resolve<V>() => data.resolve<V>(accessor);
    //    public void assign<V>(V value) => data.assign(accessor, value);

    //}
}

