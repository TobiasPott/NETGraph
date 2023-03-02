using System;

namespace NETGraph.Data
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

        public DataDefinition(string name, DataTypes type, DataStructures structure = DataStructures.Scalar, bool isResizable = false, params string[] keys) : this(name, (int)type, structure, isResizable, keys)
        { }
        public DataDefinition(string name, int typeIndex, DataStructures structure = DataStructures.Scalar, bool isResizable = false, params string[] keys)
        {
            this.Name = name;
            this.TypeIndex = typeIndex;
            this.Structure = structure;
            this.IsResizable = isResizable;
            this.Keys = (keys.Length > 0) ? keys : null;
        }

    }

    public struct GeneratorDefinition : IDataGenerator
    {
        Func<object, DataBase> scalar;
        Func<int, bool, DataBase> list;
        Func<bool, DataBase> dict;

        public GeneratorDefinition(Func<object, DataBase> scalar, Func<int, bool, DataBase> list, Func<bool, DataBase> dict)
        {
            if (scalar == null || list == null || dict == null)
                throw new ArgumentNullException($"{nameof(scalar)},{nameof(list)} and {nameof(dict)} cannot be left empty. Please provide all generator methods.");
            this.scalar = scalar;
            this.list = list;
            this.dict = dict;
        }

        public DataBase Scalar(object scalar) => this.scalar.Invoke(scalar);
        public DataBase List(int size, bool isResizable) => this.list.Invoke(size, isResizable);
        public DataBase Dict(bool isRezisable) => this.dict.Invoke(isRezisable);

    }

}

