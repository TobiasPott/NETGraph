using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NETGraph.Core;


namespace NETGraph.Core.Meta
{

    public enum DataTypes : int
    {
        Any = -1,
        Void = 0,
        Object,
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

    public static class IDataExtensions
    {

        // ToDo: movee to extension method for IData
        public static void assign<V>(this IData data, string accessor, V value) => data.assign(new DataAccessor(accessor), value);
        public static void assign<V>(this IData data, V value) => data.assign(DataAccessor.Scalar, value);
        public static IResolver resolver(this IData data, string accessor) => new DataResolver(data, accessor);
        public static IResolver resolver(this IData data) => data.resolver(string.Empty);

        public static bool match(IData lh, IData rh) => lh.typeIndex == rh.typeIndex;
        public static bool matchStructure(IData lh, IData rh) => lh.typeIndex == rh.typeIndex && lh.options == rh.options;


    }

    public interface IData
    {
        [Flags()]
        public enum Options
        {
            Scalar = 1,
            List = 2,
            Named = 4,
            Resizable = 8,

        }

        Options options { get; }
        int typeIndex { get; }

        // methods for accessing nested or dynamic data instances
        //IData access(string dataPath);

        //method to resolve data object into underlying type instances
        V resolve<V>(DataAccessor accessor);
        void assign(DataAccessor accessor, object scalar);
    }


}