using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace NETGraph.Core.Meta
{
    public interface IMemory
    {
        void Declare(string name, IData data, bool reassign);
        IData Get(string name);
        void Free(string name);
        IData Alloc<T>(Options options);
        IData Alloc(int typeIndex, Options options);
        IData Alloc(Type type, Options options);
    }

    public class MemoryFrame : IMemory
    {
        // class whide cache variables
        private static Type[] iDataDenericTypeCache = new Type[1];
        private static Type[] iDataArgumentsTypeCache = new Type[] { typeof(int), typeof(Options) };
        private static object[] iDataArgumentsCache = new object[2];

        private Dictionary<string, IData> _data = new Dictionary<string, IData>();

        // ToDo: Ponder about pooling for temporary/non declared IData

        public MemoryFrame()
        { }
        public MemoryFrame(IMemory parent, params string[] keptData)
        {
            // copy named data references from parent
            foreach (string dataName in keptData)
                this._data.Add(dataName, parent.Get(dataName));
        }



        public void Declare(string name, IData data, bool reassign)
        {
            if (!reassign)
                _data.Add(name, data);
            else
            {
                if (_data.ContainsKey(name))
                    _data[name] = data;
                else
                    _data.Add(name, data);
            }
        }
        public IData Get(string name) => _data[name];
        public void Free(string name)
        {
            if (_data.ContainsKey(name))
                _data.Remove(name);
        }
        public IData Alloc<T>(Options options)
        {
            int typeIndex = typeof(T).GetTypeIndex();
            if (options.HasFlag(Options.Index))
                return new IndexedData<T>(typeIndex, options);
            else if (options.HasFlag(Options.Named))
                return new NamedData<T>(typeIndex, options);
            else
                return new ScalarData<T>(typeIndex, options);
        }

        public IData Alloc(int typeIndex, Options options) => Alloc(typeIndex, MetaTypeRegistry.GetType(typeIndex), options);
        public IData Alloc(Type type, Options options) => Alloc(type.GetTypeIndex(), type, options);

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

    public static class Memory
    {
        public static IMemory Global { get; private set; } = new MemoryFrame();

        public static void Declare(string name, IData data, bool reassign) => Global.Declare(name, data, reassign);
        public static IData Get(string name) => Global.Get(name);
        public static void Free(string name) => Global.Free(name);

        public static IData Alloc<T>(Options options) => Global.Alloc<T>(options);
        public static IData Alloc(int typeIndex, Options options) => Global.Alloc(typeIndex, options);
        public static IData Alloc(Type type, Options options) => Global.Alloc(type, options);



        public static MethodRef Declare(int typeIndex, string name, Options options)
        {
            MethodRef method = new MethodRef((reference, args) => { return Declare(null, typeIndex.AsValueData(), name.AsValueData(), options.AsValueData()); });
            return method;
        }
        public static MethodRef Assign(string name)
        {
            MethodRef method = new MethodRef((reference, args) => { Declare(name, reference, true); return reference; });
            return method;
        }
        public static IData Declare(IData reference, params IData[] args)
        {
            int typeIndex = args[0].resolve<int>();
            string name = args[1].resolve<string>();
            Options options = Options.Scalar;
            if (args.Length > 2)
                args[2].resolve<Options>();
            IData data = Alloc(typeIndex, options);
            Declare(name, data, true);
            return data;
        }
        //public static IData Assign(IData reference, params IData[] args)
        //{
        //    if (reference != null)
        //        reference.assign(args.First());
        //    return reference;
        //}


    }
}

