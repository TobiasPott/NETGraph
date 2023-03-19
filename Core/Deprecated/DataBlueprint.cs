using System;

namespace NETGraph.Core.Meta
{

    // ToDo: Reimplement the data blueprint type to include all necessary information
    //      req.: name, type, options, keys (if dict access), size (if list access)
    public struct DataBlueprint
    {
        public string Name { get; private set; }
        public int TypeIndex { get; private set; }
        public Options Options { get; private set; }
        public string[] Keys { get; private set; }

        public DataBlueprint(string name, DataTypes type, Options options = Options.Scalar, bool isResizable = false, params string[] keys) : this(name, (int)type, options, isResizable, keys)
        { }
        public DataBlueprint(string name, int typeIndex, Options options = Options.Scalar, bool isResizable = false, params string[] keys)
        {
            this.Name = name;
            this.TypeIndex = typeIndex;
            this.Options = options;
            this.Keys = (keys.Length > 0) ? keys : null;
        }

    }

}

