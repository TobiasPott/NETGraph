using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using NETGraph.Core;
using NETGraph.Core.Meta;

namespace NETGraph.Core.Meta
{

    public class MetaTypeRegistry
    {
        private static Dictionary<int, MetaTypeBlueprint> blueprints = new Dictionary<int, MetaTypeBlueprint>();
        private static Dictionary<DataTypes, Type> builtInTypesMap = new Dictionary<DataTypes, Type>()
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



        public static bool Register(MetaTypeBlueprint blueprint)
        {
            if (!blueprints.ContainsKey(blueprint.typeIndex))
            {
                // query for typeIndex of generate new from blueprint running index
                blueprints.Add(blueprint.typeIndex, blueprint);

                if (!builtInTypesMap.ContainsKey(blueprint.dataType))
                    builtInTypesMap.Add(blueprint.dataType, blueprint.type);
                else if (!builtInTypesMap[blueprint.dataType].Equals(blueprint.type))
                    Console.WriteLine($"{blueprint.dataType} is already registered with {builtInTypesMap[blueprint.dataType]} and cannot be registeered again. Try using a different data type for {blueprint.type} or {typeof(Custom)}.");
            }
            return false;
        }

        public static void RegisterBuiltIn()
        {
            Register(new MetaTypeBlueprint((int)DataTypes.Any, typeof(Any), DataGenerator.Generator<Any>()));
            Register(new MetaTypeBlueprint((int)DataTypes.Void, typeof(Void), DataGenerator.Generator<Void>()));
            Register(new MetaTypeBlueprint((int)DataTypes.Object, typeof(object), DataGenerator.Generator<object>()));
            Register(new MetaTypeBlueprint((int)DataTypes.Bool, typeof(bool), DataGenerator.Generator<bool>()));
            Register(new MetaTypeBlueprint((int)DataTypes.Byte, typeof(byte), DataGenerator.Generator<byte>()));
            Register(new MetaTypeBlueprint((int)DataTypes.SByte, typeof(sbyte), DataGenerator.Generator<sbyte>()));
            Register(new MetaTypeBlueprint((int)DataTypes.Short, typeof(short), DataGenerator.Generator<short>()));
            Register(new MetaTypeBlueprint((int)DataTypes.UShort, typeof(ushort), DataGenerator.Generator<ushort>()));
            Register(new MetaTypeBlueprint((int)DataTypes.Char, typeof(char), DataGenerator.Generator<char>()));
            Register(new MetaTypeBlueprint((int)DataTypes.Int, typeof(int), DataGenerator.Generator<int>()));
            Register(new MetaTypeBlueprint((int)DataTypes.UInt, typeof(uint), DataGenerator.Generator<uint>()));
            Register(new MetaTypeBlueprint((int)DataTypes.Long, typeof(long), DataGenerator.Generator<long>()));
            Register(new MetaTypeBlueprint((int)DataTypes.ULong, typeof(ulong), DataGenerator.Generator<ulong>()));
            Register(new MetaTypeBlueprint((int)DataTypes.Float, typeof(float), DataGenerator.Generator<float>()));
            Register(new MetaTypeBlueprint((int)DataTypes.Double, typeof(double), DataGenerator.Generator<double>()));
            Register(new MetaTypeBlueprint((int)DataTypes.Decimal, typeof(decimal), DataGenerator.Generator<decimal>()));
            Register(new MetaTypeBlueprint((int)DataTypes.String, typeof(string), DataGenerator.Generator<string>()));
            Register(new MetaTypeBlueprint((int)DataTypes.IData, typeof(IData), DataGenerator.Generator<IData>()));
        }

        public static int GetTypeIndex(Type type) => (int)builtInTypesMap.First(x => x.Value.Equals(type)).Key;

        public static IGenerator<IData, IData.Options> Generator(int typeIndex) => blueprints[typeIndex].generator;

    }
}

