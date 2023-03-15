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
            { DataTypes.Any, typeof(Any) },
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
        public int BuiltInDataTypeFor(Type type) => (int)builtInTypesMap.First(x => x.Value.Equals(type)).Key;

    }

    public class MetaTypeRegistry
    {
        private static Dictionary<int, MetaTypeBlueprint> blueprints = new Dictionary<int, MetaTypeBlueprint>();


        public static DataTypes GetDataTypeFor(string dataName)
        {
            return blueprints.Values.Where(x => x.typeName.Equals(dataName.ToLowerInvariant())).Select(x => x.dataType).First();
        }

        public static bool RegisterDataType(MetaTypeBlueprint blueprint)
        {
            if (!blueprints.ContainsKey(blueprint.typeIndex))
            {
                // query for typeIndex of generate new from blueprint running index
                blueprints.Add(blueprint.typeIndex, blueprint);
                TypeMapping.instance.Register(blueprint.dataType, blueprint.type);
            }
            return false;
        }

        public static void RegisterBuiltIn()
        {
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Any, typeof(Any), ScalarData<Any>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Void, typeof(Void), ScalarData<Void>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Object, typeof(object), ScalarData<object>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Bool, typeof(bool), ScalarData<bool>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Byte, typeof(byte), ScalarData<byte>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.SByte, typeof(sbyte), ScalarData<sbyte>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Short, typeof(short), ScalarData<short>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.UShort, typeof(ushort), ScalarData<ushort>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Char, typeof(char), ScalarData<char>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Int, typeof(int), ScalarData<int>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.UInt, typeof(uint), ScalarData<uint>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Long, typeof(long), ScalarData<long>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.ULong, typeof(ulong), ScalarData<ulong>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Float, typeof(float), ScalarData<float>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Double, typeof(double), ScalarData<double>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Decimal, typeof(decimal), ScalarData<decimal>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.String, typeof(string), ScalarData<string>.Generator()));
            RegisterDataType(new MetaTypeBlueprint((int)DataTypes.IData, typeof(IData), ScalarData<IData>.Generator()));
        }

        public static IGenerator<IData, IData.Options> Generator(int typeIndex) => blueprints[typeIndex].generator;

    }
}

