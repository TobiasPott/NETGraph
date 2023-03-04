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
            this.type = TypeMapping.instance.BuiltInTypeFor(dataType);
            this.generator = generator;

        }
        public TypeBlueprint(Type type, IDataGenerator generator)
        {
            this.typeIndex = _runningIndex++;
            this.typeName = type.Name;
            this.type = type;
            this.generator = generator;
        }

    }

    public class TypeMapping
    {
        public static TypeMapping instance => new TypeMapping();

        private TypeMapping()
        { }

        private Dictionary<DataTypes, Type> builtInTypesMap = new Dictionary<DataTypes, Type>()
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

        public void Register(DataTypes dataType, Type type)
        {
            if (!builtInTypesMap.ContainsKey(dataType))
                builtInTypesMap.Add(dataType, type);
            else if (!builtInTypesMap[dataType].Equals(type))
                Console.WriteLine($"{dataType} is already registered with {builtInTypesMap[dataType]} and cannot be registeered again. Try using a different data type for {type}.");
        }

        public Type BuiltInTypeFor(DataTypes dataType) => builtInTypesMap[dataType];
        public DataTypes BuiltInDataTypeFor(Type type) => builtInTypesMap.First(x => x.Value.Equals(type)).Key;

    }

    public struct Void
    { }

    public class TypeRegistry
    {
        private static Dictionary<DataTypes, TypeBlueprint> blueprints = new Dictionary<DataTypes, TypeBlueprint>();


        public static DataTypes GetDataTypeFor(string dataName)
        {
            return blueprints.Values.Where(x => x.typeName.Equals(dataName.ToLowerInvariant())).Select(x => x.dataType).First();
        }

        public static bool RegisterDataType(TypeBlueprint blueprint)
        {
            if (!blueprints.ContainsKey(blueprint.dataType))
            {
                blueprints.Add(blueprint.dataType, blueprint);
                TypeMapping.instance.Register(blueprint.dataType, blueprint.type);
            }
            return false;
        }
        public static void RegisterBuiltIn()
        {
            RegisterDataType(new TypeBlueprint(DataTypes.Void, DataBase<Void>.Generator()));
            RegisterDataType(new TypeBlueprint(DataTypes.Object, DataBase<object>.Generator()));
            RegisterDataType(new TypeBlueprint(DataTypes.Bool, DataBase<bool>.Generator()));
            RegisterDataType(new TypeBlueprint(DataTypes.Byte, DataBase<byte>.Generator()));
            RegisterDataType(new TypeBlueprint(DataTypes.SByte, DataBase<sbyte>.Generator()));
            RegisterDataType(new TypeBlueprint(DataTypes.Short, DataBase<short>.Generator()));
            RegisterDataType(new TypeBlueprint(DataTypes.UShort, DataBase<ushort>.Generator()));
            RegisterDataType(new TypeBlueprint(DataTypes.Char, DataBase<char>.Generator()));
            RegisterDataType(new TypeBlueprint(DataTypes.Int, DataBase<int>.Generator()));
            RegisterDataType(new TypeBlueprint(DataTypes.UInt, DataBase<uint>.Generator()));
            RegisterDataType(new TypeBlueprint(DataTypes.Long, DataBase<long>.Generator()));
            RegisterDataType(new TypeBlueprint(DataTypes.ULong, DataBase<ulong>.Generator()));
            RegisterDataType(new TypeBlueprint(DataTypes.Float, DataBase<float>.Generator()));
            RegisterDataType(new TypeBlueprint(DataTypes.Double, DataBase<double>.Generator()));
            RegisterDataType(new TypeBlueprint(DataTypes.Decimal, DataBase<decimal>.Generator()));
            RegisterDataType(new TypeBlueprint(DataTypes.String, DataBase<string>.Generator()));
            RegisterDataType(new TypeBlueprint(DataTypes.IData, DataBase<IData>.Generator()));
        }


        public static DataBase generateScalar<T>(DataTypes type, object scalar) => blueprints[type].generator.Scalar(scalar);
        public static DataBase generateList<T>(DataTypes type, int size, bool isResizable) => blueprints[type].generator.List(size, isResizable);
        public static DataBase generateDict<T>(DataTypes type, bool isResizable) => blueprints[type].generator.Dict(isResizable);



    }
}

