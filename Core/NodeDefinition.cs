using System;
using System.Collections.Generic;

namespace NETGraph.Core
{

    public enum DataStructures
    {
        Scalar,
        Array,
        List,
        Dict,
    }

    public static class TypeIndices
    {
        public const int Object = 0;
        public const int Bool = 1;
        public const int Int = 2;
        public const int Long = 3;
        public const int Float = 4;
        public const int Double = 4;
        public const int Short = 5;
    }


    public struct AccessorDefinition
    {
        string name;
        string dataName; // name of data mapped by the accessor
        string dataAccessor; // accessor (subname) to query data for, [scalar], [#index] or ['key']

        public AccessorDefinition(string name, string dataName, string dataAccessor)
        {
            this.name = name;
            this.dataName = dataName;
            this.dataAccessor = dataAccessor;
        }
    }


    public abstract class DataDefinition
    {
        string name;
        int typeIndex;
        DataStructures structure;
        int size;
        int isResizable;

    }


    public abstract class NodeDefinition
    {

        List<AccessorDefinition> accessors = new List<AccessorDefinition>();
        List<DataDefinition> datas = new List<DataDefinition>();

        protected NodeDefinition(IEnumerable<AccessorDefinition> accessors, IEnumerable<DataDefinition> datas)
        {
            this.accessors.AddRange(accessors);
            this.datas.AddRange(datas);
        }


    }
}

