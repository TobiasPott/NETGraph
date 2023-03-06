using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using NETGraph.Core;
using NETGraph.Core.Meta;

namespace NETGraph.Core.Meta
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

    public class MetaTypeRegistry
    {
        private static Dictionary<DataTypes, MetaTypeBlueprint> blueprints = new Dictionary<DataTypes, MetaTypeBlueprint>();


        public static DataTypes GetDataTypeFor(string dataName)
        {
            return blueprints.Values.Where(x => x.typeName.Equals(dataName.ToLowerInvariant())).Select(x => x.dataType).First();
        }

        public static bool RegisterDataType(MetaTypeBlueprint blueprint)
        {
            if (!blueprints.ContainsKey(blueprint.dataType))
            {
                // query for typeIndex of generate new from blueprint running index
                blueprints.Add(blueprint.dataType, blueprint);
                TypeMapping.instance.Register(blueprint.dataType, blueprint.type);
            }
            return false;
        }

        public static void RegisterBuiltIn()
        {
            RegisterDataType(new MetaTypeBlueprint(DataTypes.Void, Data<Void>.Generator()));
            RegisterDataType(new MetaTypeBlueprint(DataTypes.Object, Data<object>.Generator()));
            RegisterDataType(new MetaTypeBlueprint(DataTypes.Bool, Data<bool>.Generator()));
            RegisterDataType(new MetaTypeBlueprint(DataTypes.Byte, Data<byte>.Generator()));
            RegisterDataType(new MetaTypeBlueprint(DataTypes.SByte, Data<sbyte>.Generator()));
            RegisterDataType(new MetaTypeBlueprint(DataTypes.Short, Data<short>.Generator()));
            RegisterDataType(new MetaTypeBlueprint(DataTypes.UShort, Data<ushort>.Generator()));
            RegisterDataType(new MetaTypeBlueprint(DataTypes.Char, Data<char>.Generator()));
            RegisterDataType(new MetaTypeBlueprint(DataTypes.Int, Data<int>.Generator()));
            RegisterDataType(new MetaTypeBlueprint(DataTypes.UInt, Data<uint>.Generator()));
            RegisterDataType(new MetaTypeBlueprint(DataTypes.Long, Data<long>.Generator()));
            RegisterDataType(new MetaTypeBlueprint(DataTypes.ULong, Data<ulong>.Generator()));
            RegisterDataType(new MetaTypeBlueprint(DataTypes.Float, Data<float>.Generator()));
            RegisterDataType(new MetaTypeBlueprint(DataTypes.Double, Data<double>.Generator()));
            RegisterDataType(new MetaTypeBlueprint(DataTypes.Decimal, Data<decimal>.Generator()));
            RegisterDataType(new MetaTypeBlueprint(DataTypes.String, Data<string>.Generator()));
            RegisterDataType(new MetaTypeBlueprint(DataTypes.IData, Data<IData>.Generator()));
        }

        public static IDataGenerator Generator(DataTypes dataType) => blueprints[dataType].generator;

    }
}

