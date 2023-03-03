using System;
using System.Collections.Generic;
using System.Linq;
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
        MethodQuery, IMethodProvider, MethodSignature,
        DataQuery, IDataProvider, DataSignature,

    }


    public class DataRegistry
    {
        public static Dictionary<DataTypes, Type> Map = new Dictionary<DataTypes, Type>()
        {
            { DataTypes.Object, typeof(object) },
            { DataTypes.Bool, typeof(bool) },
            { DataTypes.Byte, typeof(byte) },
            { DataTypes.SByte, typeof(sbyte) },
            { DataTypes.Short, typeof(short) },
            { DataTypes.UShort, typeof(ushort) },
            { DataTypes.Int, typeof(int) },
            { DataTypes.UInt, typeof(uint) },
            { DataTypes.Char, typeof(char) },
            { DataTypes.Long, typeof(long) },
            { DataTypes.ULong, typeof(ulong) },
            { DataTypes.Float, typeof(float) },
            { DataTypes.Double, typeof(double) },
            { DataTypes.Decimal, typeof(decimal) },
            { DataTypes.String, typeof(string) },
            // internal library types
            { DataTypes.IData, typeof(IData) },
        };
        // reversed lookup for type to DataTypes
        public static Dictionary<Type, DataTypes> MapReveresed = new Dictionary<Type, DataTypes>(Map.Select(x => new KeyValuePair<Type, DataTypes>(x.Value, x.Key)));
        static Dictionary<DataTypes, IDataGenerator> generators = new Dictionary<DataTypes, IDataGenerator>();


        public static bool RegisterDataType(DataTypes dataType, Type type, IDataGenerator generator)
        {
            if (!Map.ContainsKey(dataType))
            {
                if (!MapReveresed.ContainsKey(type))
                {
                    // try add and ignore case if generator already exists
                    generators.TryAdd(dataType, generator);

                    Map.Add(dataType, type);
                    MapReveresed.Add(type, dataType);
                    return true;
                }
            }
            //throw new ArgumentException($"{dataType}/{type} is already registered. Duplicate types are not allowed.");
            return false;
        }

        public DataBase generateScalar<T>(DataTypes type, object scalar) => generators[type].Scalar(scalar);
        public DataBase generateList<T>(DataTypes type, int size, bool isResizable) => generators[type].List(size, isResizable);
        public DataBase generateDict<T>(DataTypes type, bool isResizable) => generators[type].Dict(isResizable);



    }
}

