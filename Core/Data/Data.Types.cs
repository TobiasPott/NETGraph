using System;
using System.Collections.Generic;
using NETGraph.Core;

namespace NETGraph.Data
{

    public enum DataStructures
    {
        Scalar,
        Array,
        List,
        Dict,
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

}

