using System;
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

    public static class DataExtensions
    {

        public static void assign<V>(this IData data, string accessor, V value) => data.assign(new DataAccessor(accessor), value);
        public static void assign<V>(this IData data, V value) => data.assign(DataAccessor.Scalar, value);
        public static IResolver resolver(this IData data, string accessor) => data.resolver(new DataAccessor(accessor));
        public static IResolver resolver(this IData data) => data.resolver(DataAccessor.Scalar);

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
        IData access(string dataPath);


        //method to resolve data object into underlying type instances
        V resolve<V>(DataAccessor accessor);
        void assign(DataAccessor accessor, object scalar);

        IResolver resolver(DataAccessor accessor);
    }


    public abstract class DataBase : IData
    {
        public IData.Options options { get; protected set; }
        public int typeIndex { get; protected set; }


        protected DataBase(int typeIndex, IData.Options options)
        {
            this.typeIndex = typeIndex;
            this.options = options;
        }


        // methods for accessing nested or dynamic data instances
        public abstract IData access(string dataPath);
        //method to resolve data object into underlying type instances
        public abstract void assign(DataAccessor accessor, object scalar);
        public abstract V resolve<V>(DataAccessor accessor);
        public IResolver resolver(DataAccessor accessor) => new DataResolver(this, accessor);
    }

    public class Data<T> : DataBase
    {
        public static DataGenerator Generator()
        {
            return new DataGenerator((o) => new Data<T>(TypeMapping.instance.BuiltInDataTypeFor(typeof(T)), o));
        }

        protected T scalar { get; set; }
        private List<T> list;
        private Dictionary<string, T> dict;


        protected Data(Type type, IData.Options options) : this(TypeMapping.instance.BuiltInDataTypeFor(type), options)
        { }
        protected Data(int typeIndex, IData.Options options) : base(typeIndex, options)
        {
            if (options.HasFlag(IData.Options.List))
                this.list = new List<T>();
            else if (options.HasFlag(IData.Options.Named))
                this.dict = new Dictionary<string, T>();
        }


        protected T this[int index]
        {
            get => list[index];
            set => list[index] = value;
        }
        protected T this[string name]
        {
            get => dict[name];
            set
            {
                if (!dict.ContainsKey(name))
                    dict.Add(name, value);
                else
                    dict[name] = value;
            }
        }



        protected Data<T> initScalar(T scalar)
        {
            this.scalar = scalar;
            return this;
        }
        protected Data<T> initList(IEnumerable<T> values)
        {
            if (this.list.Count == 0)
            {
                this.list = new List<T>(values);
                return this;
            }
            else
                throw new InvalidOperationException($"Cannot initialize data on {this.options} structure that contains data. Please clear the structure beforee re-init.");
        }
        protected Data<T> initNamed(IEnumerable<KeyValuePair<string, T>> namedValues)
        {
            if (this.dict.Count == 0)
            {
                this.dict = new Dictionary<string, T>(namedValues);
                return this;
            }
            else
                throw new InvalidOperationException($"Cannot initialize data on {this.options} structure that contains data. Please clear the structure beforee re-init.");
        }


        public override IData access(string dataPath)
        {
            throw new InvalidOperationException($"{nameof(Data<T>)} does not support accessing nested or dynamic data. Implement your own type to support arbitrary data access with dynamic data.");
        }


        protected object getValueScalar() => this.scalar;
        protected object getValueAt(int index) => this[index];
        protected object getValueAt(string name) => this[name];

        public override V resolve<V>(DataAccessor accessor)
        {
            switch (accessor.accessType)
            {
                case DataAccessor.AccessTypes.Scalar:
                    return (V)this.getValueScalar();
                case DataAccessor.AccessTypes.Index:
                    return (V)this.getValueAt(accessor.index);
                case DataAccessor.AccessTypes.Key:
                    return (V)this.getValueAt(accessor.key);
                default:
                    return default(V);
            }
        }
        public override void assign(DataAccessor accessor, object value)
        {
            switch (accessor.accessType)
            {
                case DataAccessor.AccessTypes.Scalar:
                    this.scalar = (T)value;
                    break;
                case DataAccessor.AccessTypes.Index:
                    this[accessor.index] = (T)value;
                    break;
                case DataAccessor.AccessTypes.Key:
                    this[accessor.key] = (T)value;
                    break;
                default:
                    break;
            }
        }

        private void throwCheckResizable()
        {
            if (!this.options.HasFlag(IData.Options.Resizable))
                throw new InvalidOperationException($"You cannot add items to a non resizable structure.");
        }
        private void throwCheckStructure(IData.Options structure)
        {
            if (!this.options.HasFlag(structure))
                throw new InvalidOperationException($"You cannot add items to a non collection structure. Consider using a {structure} structure.");
        }
        private void throwCheckGenericType(object item)
        {
            if (!(item is T))
                throw new InvalidCastException($"{nameof(item)} has missmatching type.");
        }
        private void throwCheckAccessByIndex()
        {
            if (!this.options.HasFlag(IData.Options.List))
                throw new InvalidCastException($"{typeof(Data<T>)} cannot be accessed by index.");
        }
        private void throwCheckAccessByKey()
        {
            if (!this.options.HasFlag(IData.Options.Named))
                throw new InvalidCastException($"{typeof(Data<T>)} cannot be accessed by key.");
        }
        private void throwCheckAccessByScalar()
        {
            if (!this.options.HasFlag(IData.Options.Scalar))
                throw new InvalidCastException($"{typeof(Data<T>)} cannot be accessed as scalar.");
        }



        public override string ToString()
        {
            string toString = string.Empty;
            if (this.options == IData.Options.Scalar)
                toString = $"{scalar}";
            else if (this.options == IData.Options.List)
                toString = $"[{string.Join(", ", list)}]";
            else if (this.options == IData.Options.Named)
                toString = $"[{string.Join(", ", dict)}]";

            return base.ToString() + $"[{this.options}] = " + toString;
        }

    }

}

