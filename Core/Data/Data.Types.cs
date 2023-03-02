using System;
using System.Collections.Generic;
using System.Linq;
using NETGraph.Core;

namespace NETGraph.Data
{

    public enum DataStructures
    {
        Scalar,
        Array,
        List,
        Named,
    }

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
        MethodQuery, IMethodProvider, MethodAccessor,
        DataQuery, IDataProvider, DataAccessor,

    }

    public static class TypeIndices
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
        };
    }

    public interface IDataGenerator
    {
        DataBase Scalar(object scalar);
        DataBase List(int size, bool isResizable);
        DataBase Dict(bool isRezisable);
    }

    public class DataRegistry
    {
        // ToDo: Implement Func<DataBase> based generators which allow passing Data constructor parameters
        //      Wrap generator into simple type/interface which provides all methods to generate:
        //          scalar data
        //          array/list data
        //          dict data
        static Dictionary<DataTypes, IDataGenerator> generators = new Dictionary<DataTypes, IDataGenerator>();

        public static bool Register(DataTypes type, IDataGenerator generator)
        {
            if (!generators.ContainsKey(type))
            {
                generators.Add(type, generator);
                return true;
            }
            return false;
        }

        public DataBase generateScalar<T>(DataTypes type, object scalar) => generators[type].Scalar(scalar);
        public DataBase generateList<T>(DataTypes type, int size, bool isResizable) => generators[type].List(size, isResizable);
        public DataBase generateDict<T>(DataTypes type, bool isResizable) => generators[type].Dict(isResizable);

    }
}

