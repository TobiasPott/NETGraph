using System;
using System.Reflection;

namespace NETGraph.Core.Meta
{

    public static class DataGenerator
    {

        public static IData Generate<T>(IData.Options options)
        {
            int typeIndex = MetaTypeRegistry.GetTypeIndex(typeof(T));
            if (options.HasFlag(IData.Options.List))
                return new ListData<T>(typeIndex, options);
            else if (options.HasFlag(IData.Options.Named))
                return new DictData<T>(typeIndex, options);
            else
                return new ScalarData<T>(typeIndex, options);
        }

        private static Type[] iDataDenericTypeCache = new Type[1];
        private static Type[] iDataArgumentsTypeCache = new Type[] { typeof(int), typeof(IData.Options) };
        private static object[] iDataArgumentsCache = new object[2];
        // ToDo: add overload which takes int typeIndex and makes a lookup against the type registry
        public static IData Generate(Type type, IData.Options options)
        {
            int typeIndex = MetaTypeRegistry.GetTypeIndex(type);
            // Specify the type parameter of the A<> type
            iDataDenericTypeCache[0] = type;
            Type genericType;
            if (options.HasFlag(IData.Options.List))
                genericType = typeof(ListData<>).MakeGenericType(iDataDenericTypeCache);
            else if (options.HasFlag(IData.Options.Named))
                genericType = typeof(DictData<>).MakeGenericType(iDataDenericTypeCache);
            else
                genericType = typeof(ScalarData<>).MakeGenericType(iDataDenericTypeCache);
            // Get the 'B' method and invoke it:
            ConstructorInfo ctor = genericType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, Type.DefaultBinder, iDataArgumentsTypeCache, null);
            // Convert the result to string & return it
            iDataArgumentsCache[0] = typeIndex;
            iDataArgumentsCache[1] = options;
            return (IData)ctor.Invoke(iDataArgumentsCache);
        }

    }
}

