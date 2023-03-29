using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NETGraph.Core;


namespace NETGraph.Core.Meta
{

    [Flags()]
    public enum Options : int
    {
        Scalar = 1,
        Index = 2,
        Named = 4,
        Resizable = 8,
    }


    public static class CoreExtensions
    {
        public static ValueData<T> AsValueData<T>(this T value) => new ValueData<T>(value);
        public static bool IsAssignableFrom<From>(this Type to) => to.IsAssignableFrom(typeof(From));
        public static bool IsAssignableFrom<To, From>() => typeof(To).IsAssignableFrom(typeof(From));

        public static bool TryCast<To>(this object obj, out To casted)
        {
            if (obj is To)
            {
                casted = (To)obj;
                return true;
            }

            casted = default(To);
            return false;

        }
        public static bool TryAssignableCast<To>(this object obj, out To casted)
        {
            if (obj.GetType().IsAssignableFrom<To>())
                return TryCast<To>(obj, out casted);

            casted = default(To);
            return false;
        }
        public static bool TryCastOrResolve<To, From>(out To casted, From value)
        {
            if (CoreExtensions.IsAssignableFrom<To, From>() && value.TryCast<To>(out casted))
            {
                return true;
            }
            if (typeof(IResolver).IsAssignableFrom(typeof(From)))
            {
                casted = (value as IResolver).resolve<To>();
                return true;
            }
            casted = default(To);
            return false;
        }
    }

    public static class IDataExtensions
    {
        public static void assign<V>(this IData data, string accessor, V value) => data.assign(new DataAccessor(accessor), value);
        public static void assign<V>(this IData data, V value) => data.assign(DataAccessor.Scalar, value);

        public static bool match(IData lh, IData rh) => lh.typeIndex == rh.typeIndex;
        public static bool matchStructure(IData lh, IData rh) => lh.typeIndex == rh.typeIndex && lh.options == rh.options;
    }

}