using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using NETGraph.Core;
using NETGraph.Core.Meta;

namespace NETGraph.Data
{

    public enum DataTypes : int
    {
        Void = -1,
        Object = 0,
        Bool,
        Byte, SByte,
        Short, UShort,
        Int, UInt, Char,
        Long, ULong,
        Float, Double, Decimal,
        String,
        // internal library types (e.g. node, data, method
        IData,
    }


    public struct TypeBlueprint
    {
        private static int _runningIndex = 1024;

        public DataTypes dataType => (DataTypes)typeIndex;
        public int typeIndex { get; private set; }
        public string typeName { get; private set; }
        public Type type { get; private set; }
        public IDataGenerator generator { get; private set; }

        public TypeBlueprint(DataTypes dataType, IDataGenerator generator)
        {
            this.typeName = dataType.ToString().ToLowerInvariant();
            this.typeIndex = (int)dataType;
            this.type = TypeRegistry.BuiltInTypeFor(dataType);
            this.generator = generator;

        }
        public TypeBlueprint(string name, Type type, IDataGenerator generator)
        {
            this.typeIndex = _runningIndex++;
            this.typeName = name;
            this.type = type;
            this.generator = generator;
        }
    }

    public struct Void
    { }

    public class TypeRegistry
    {
        // reversed lookup for type to DataTypes
        private static Dictionary<Type, DataTypes> builtInTypesMapRev = new Dictionary<Type, DataTypes>(builtInTypesMap.Select(x => new KeyValuePair<Type, DataTypes>(x.Value, x.Key)));
        private static Dictionary<DataTypes, Type> builtInTypesMap = new Dictionary<DataTypes, Type>()
        {
            { DataTypes.Void, typeof(Void) },
            { DataTypes.Object, typeof(object) },
            { DataTypes.Bool, typeof(bool) },
            { DataTypes.Byte, typeof(byte) },
            { DataTypes.SByte, typeof(sbyte) },
            { DataTypes.Short, typeof(short) },
            { DataTypes.UShort, typeof(ushort) },
            { DataTypes.Char, typeof(char) },
            { DataTypes.Int, typeof(int) },
            { DataTypes.UInt, typeof(uint) },
            { DataTypes.Long, typeof(long) },
            { DataTypes.ULong, typeof(ulong) },
            { DataTypes.Float, typeof(float) },
            { DataTypes.Double, typeof(double) },
            { DataTypes.Decimal, typeof(decimal) },
            { DataTypes.String, typeof(string) },
            // internal library types
            { DataTypes.IData, typeof(IData) },
        };
        private static Dictionary<DataTypes, TypeBlueprint> blueprints = new Dictionary<DataTypes, TypeBlueprint>();

        // ToDo: transform to operate on blueprints dictionary and auto-init if required
        //      
        public static Type BuiltInTypeFor(DataTypes dataType) => builtInTypesMap[dataType];


        public static DataTypes GetDataTypeFor(string dataName)
        {
            TypeRegistry.AutoInit();
            return blueprints.Values.Where(x => x.typeName.Equals(dataName.ToLowerInvariant())).Select(x => x.dataType).First();
        }


        public static bool RegisterDataType(TypeBlueprint blueprint)
        {
            if (!blueprints.ContainsKey(blueprint.dataType))
                blueprints.Add(blueprint.dataType, blueprint);
            return false;
        }
        private static void AutoInit()
        {
            if (blueprints.Count == 0)
                RegisterBuiltIn();
        }
        private static void RegisterBuiltIn()
        {
            RegisterDataType(new TypeBlueprint(DataTypes.Void, null));
            RegisterDataType(new TypeBlueprint(DataTypes.Object, null));
            RegisterDataType(new TypeBlueprint(DataTypes.Bool, null));
            RegisterDataType(new TypeBlueprint(DataTypes.Byte, null));
            RegisterDataType(new TypeBlueprint(DataTypes.SByte, null));
            RegisterDataType(new TypeBlueprint(DataTypes.Short, null));
            RegisterDataType(new TypeBlueprint(DataTypes.UShort, null));
            RegisterDataType(new TypeBlueprint(DataTypes.Char, null));
            RegisterDataType(new TypeBlueprint(DataTypes.Int, Simple.IntData.Generator));
            RegisterDataType(new TypeBlueprint(DataTypes.UInt, null));
            RegisterDataType(new TypeBlueprint(DataTypes.Long, null));
            RegisterDataType(new TypeBlueprint(DataTypes.ULong, null));
            RegisterDataType(new TypeBlueprint(DataTypes.Float, null));
            RegisterDataType(new TypeBlueprint(DataTypes.Double, null));
            RegisterDataType(new TypeBlueprint(DataTypes.Decimal, null));
            RegisterDataType(new TypeBlueprint(DataTypes.String, null));
            RegisterDataType(new TypeBlueprint(DataTypes.IData, null));
        }


        public DataBase generateScalar<T>(DataTypes type, object scalar) => blueprints[type].generator.Scalar(scalar);
        public DataBase generateList<T>(DataTypes type, int size, bool isResizable) => blueprints[type].generator.List(size, isResizable);
        public DataBase generateDict<T>(DataTypes type, bool isResizable) => blueprints[type].generator.Dict(isResizable);



    }
}

