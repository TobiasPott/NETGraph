using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using NETGraph.Core;
using NETGraph.Core.Meta;

namespace NETGraph.Core.Meta
{


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
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Void, typeof(Void), Data<Void>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Object, typeof(object), Data<object>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Bool, typeof(bool), Data<bool>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Byte, typeof(byte), Data<byte>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.SByte, typeof(sbyte), Data<sbyte>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Short, typeof(short), Data<short>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.UShort, typeof(ushort), Data<ushort>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Char, typeof(char), Data<char>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Int, typeof(int), Data<int>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.UInt, typeof(uint), Data<uint>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Long, typeof(long), Data<long>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.ULong, typeof(ulong), Data<ulong>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Float, typeof(float), Data<float>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Double, typeof(double), Data<double>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Decimal, typeof(decimal), Data<decimal>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.String, typeof(string), Data<string>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.IData, typeof(IData), Data<IData>.Generator()));
        }

        public static IDataGenerator Generator(DataTypes dataType) => blueprints[dataType].generator;

    }
}

