using System;
namespace NETGraph.Core
{
    public interface IDataDefinition
    {
        string Name { get; }
        int TypeIndex { get; }
        DataStructures Structure { get; }
        bool IsResizable { get; }
        string[] Keys { get; }
    }

    public struct DataDefinition : IDataDefinition
    {
        public string Name { get; private set; }
        public int TypeIndex { get; private set; }
        public DataStructures Structure { get; private set; }
        public bool IsResizable { get; private set; }
        public string[] Keys { get; private set; }

        public DataDefinition(string name, int typeIndex, DataStructures structure = DataStructures.Scalar, bool isResizable = false, params string[] keys)
        {
            this.Name = name;
            this.TypeIndex = typeIndex;
            this.Structure = structure;
            this.IsResizable = isResizable;
            this.Keys = (keys.Length > 0) ? keys : null;
        }

    }

}

