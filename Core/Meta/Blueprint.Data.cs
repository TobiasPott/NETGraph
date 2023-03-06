using System;

namespace NETGraph.Core.Meta
{

    // ToDo: Implement IDynamicData type
    //      IDynamicData holds a method referencee returning an IData reference or sth like that
    // Todo: change resolver path parser to parse nested keys/scalar/index access

    //
    // ToDo: Ponder about the possible conflict of IDataDefinition and DataSginature
    //      One is used to access data in a data provider
    //      Thee other is used to describe data structe#
    public struct DataBlueprint
    {
        public string Name { get; private set; }
        public int TypeIndex { get; private set; }
        public DataOptions Options { get; private set; }
        public string[] Keys { get; private set; }

        public DataBlueprint(string name, DataTypes type, DataOptions options = DataOptions.Scalar, bool isResizable = false, params string[] keys) : this(name, (int)type, options, isResizable, keys)
        { }
        public DataBlueprint(string name, int typeIndex, DataOptions options = DataOptions.Scalar, bool isResizable = false, params string[] keys)
        {
            this.Name = name;
            this.TypeIndex = typeIndex;
            this.Options = options;
            this.Keys = (keys.Length > 0) ? keys : null;
        }

    }

}

