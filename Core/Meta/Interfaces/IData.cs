using System;


namespace NETGraph.Core.Meta
{

    public interface IData : IResolver
    {
        Options options { get; }
        int typeIndex { get; }

        // methods for accessing nested or dynamic data instances
        //IData access(string dataPath);

        ////method to resolve data object into underlying type instances
        //V resolve<V>(DataAccessor accessor);
        //void assign(DataAccessor accessor, object scalar);
    }

}

