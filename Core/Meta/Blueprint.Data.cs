using System;
using NETGraph.Data;

namespace NETGraph.Core.Meta
{

    // ToDo: Implement IDynamicData type
    //      IDynamicData holds a method referencee returning an IData reference or sth like that
    // Todo: change resolver path parser to parse nested keys/scalar/index access

    //
    // ToDo: Ponder about the possible conflict of IDataDefinition and DataSginature
    //      One is used to access data in a data provider
    //      Thee other is used to describe data structe#
    public interface IDataBlueprint
    {
        // ToDo: insert Type field
        //  ToDo: add nested as IDataBlueprint[] for neested data types
        string Name { get; }
        int TypeIndex { get; }
        DataStructure Structure { get; }
        bool IsResizable { get; }
        string[] Keys { get; }
    }


    public struct DataBlueprint : IDataBlueprint
    {
        public string Name { get; private set; }
        public int TypeIndex { get; private set; }
        public DataStructure Structure { get; private set; }
        public bool IsResizable { get; private set; }
        public string[] Keys { get; private set; }

        public DataBlueprint(string name, DataTypes type, DataStructure structure = DataStructure.Scalar, bool isResizable = false, params string[] keys) : this(name, (int)type, structure, isResizable, keys)
        { }
        public DataBlueprint(string name, int typeIndex, DataStructure structure = DataStructure.Scalar, bool isResizable = false, params string[] keys)
        {
            this.Name = name;
            this.TypeIndex = typeIndex;
            this.Structure = structure;
            this.IsResizable = isResizable;
            this.Keys = (keys.Length > 0) ? keys : null;
        }

    }

}

