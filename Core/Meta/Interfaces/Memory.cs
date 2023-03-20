using System;
using System.Collections.Generic;
using System.Reflection;

namespace NETGraph.Core.Meta
{

    public static class Memory
    {
        // ToDo: Add memory frame/stack frame and data scope support to the memory class
        //
        private static Dictionary<string, IData> _data = new Dictionary<string, IData>();


        public static void Assign(string name, IData data) => _data.Add(name, data);
        public static IData Get(string name) => _data[name];
        public static void Free(string name)
        {
            if (_data.ContainsKey(name))
                _data.Remove(name);
        }

        public static IData Alloc<T>(Options options)
        {
            int typeIndex = MetaTypeRegistry.GetTypeIndex(typeof(T));
            if (options.HasFlag(Options.Index))
                return new IndexedData<T>(typeIndex, options);
            else if (options.HasFlag(Options.Named))
                return new NamedData<T>(typeIndex, options);
            else
                return new ScalarData<T>(typeIndex, options);
        }

        private static Type[] iDataDenericTypeCache = new Type[1];
        private static Type[] iDataArgumentsTypeCache = new Type[] { typeof(int), typeof(Options) };
        private static object[] iDataArgumentsCache = new object[2];

        public static IData Alloc(int typeIndex, Options options) => Alloc(typeIndex, MetaTypeRegistry.GetType(typeIndex), options);
        public static IData Alloc(Type type, Options options) => Alloc(MetaTypeRegistry.GetTypeIndex(type), type, options);
        private static IData Alloc(int typeIndex, Type type, Options options)
        {
            // Specify the type parameter of the A<> type
            iDataDenericTypeCache[0] = type;
            Type genericType;
            if (options.HasFlag(Options.Index))
                genericType = typeof(IndexedData<>).MakeGenericType(iDataDenericTypeCache);
            else if (options.HasFlag(Options.Named))
                genericType = typeof(NamedData<>).MakeGenericType(iDataDenericTypeCache);
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

