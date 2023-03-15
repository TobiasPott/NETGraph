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


    public class ScalarData<T> : IData
    {
        public static DataGenerator Generator()
        {
            return new DataGenerator((o) => new ScalarData<T>(TypeMapping.instance.BuiltInDataTypeFor(typeof(T)), o));
        }

        public IData.Options options { get; protected set; }
        public int typeIndex { get; protected set; }

        protected T scalar { get; set; }


        protected ScalarData(Type type, IData.Options options) : this(TypeMapping.instance.BuiltInDataTypeFor(type), options)
        { }
        protected ScalarData(int typeIndex, IData.Options options)
        {
            this.typeIndex = typeIndex;
            this.options = IData.Options.Scalar | options;
        }


        protected ScalarData<T> initScalar(T scalar)
        {
            this.scalar = scalar;
            return this;
        }
      

        protected object getValueScalar() => this.scalar;

        public virtual V resolve<V>(DataAccessor accessor)
        {
            switch (accessor.accessType)
            {
                case DataAccessor.AccessTypes.Scalar:
                    return (V)this.getValueScalar();
                default:
                    throw new InvalidOperationException($"Cannot acceess {this.GetType()} as {accessor.accessType}.");
            }
        }
        public virtual void assign(DataAccessor accessor, object value)
        {
            switch (accessor.accessType)
            {
                case DataAccessor.AccessTypes.Scalar:
                    this.scalar = (T)value;
                    break;
                default:
                    throw new InvalidOperationException($"Cannot acceess {this.GetType()} as {accessor.accessType}.");
            }
        }

        public override string ToString()
        {
            string toString = string.Empty;
            if (this.options == IData.Options.Scalar)
                toString = $"{scalar}";
            else
                toString = $"INVALID {this.GetType()}";

            return base.ToString() + $"[{this.options}] = " + toString;
        }

    }

    public class ListData<T> : IData
    {
        public static DataGenerator Generator()
        {
            return new DataGenerator((o) => new ListData<T>(TypeMapping.instance.BuiltInDataTypeFor(typeof(T)), o));
        }

        public IData.Options options { get; protected set; }
        public int typeIndex { get; protected set; }

        private List<T> list;


        protected ListData(Type type, IData.Options options) : this(TypeMapping.instance.BuiltInDataTypeFor(type), options)
        { }
        protected ListData(int typeIndex, IData.Options options)
        {
            this.typeIndex = typeIndex;
            this.options = IData.Options.List | options;
            this.list = new List<T>();
        }


        protected T this[int index]
        {
            get => list[index];
            set => list[index] = value;
        }



        protected ListData<T> initList(IEnumerable<T> values)
        {
            if (this.list.Count == 0)
            {
                this.list = new List<T>(values);
                return this;
            }
            else
                throw new InvalidOperationException($"Cannot initialize data on {this.options} structure that contains data. Please clear the structure beforee re-init.");
        }


        protected object getValueAt(int index) => this[index];

        public virtual V resolve<V>(DataAccessor accessor)
        {
            switch (accessor.accessType)
            {
                case DataAccessor.AccessTypes.Index:
                    return (V)this.getValueAt(accessor.index);
                default:
                    throw new InvalidOperationException($"Cannot acceess {this.GetType()} as {accessor.accessType}.");
            }
        }
        public virtual void assign(DataAccessor accessor, object value)
        {
            switch (accessor.accessType)
            {
                case DataAccessor.AccessTypes.Index:
                    this[accessor.index] = (T)value;
                    break;
                default:
                    throw new InvalidOperationException($"Cannot acceess {this.GetType()} as {accessor.accessType}.");
            }
        }

        public override string ToString()
        {
            string toString = string.Empty;
            if (this.options == IData.Options.List)
                toString = $"[{string.Join(", ", list)}]";
            else
                toString = $"INVALID {this.GetType()}";
            return base.ToString() + $"[{this.options}] = " + toString;
        }

    }

    public class DictData<T> : IData
    {
        public static DataGenerator Generator()
        {
            return new DataGenerator((o) => new DictData<T>(TypeMapping.instance.BuiltInDataTypeFor(typeof(T)), o));
        }

        public IData.Options options { get; protected set; }
        public int typeIndex { get; protected set; }

        private Dictionary<string, T> dict;


        protected DictData(Type type, IData.Options options) : this(TypeMapping.instance.BuiltInDataTypeFor(type), options)
        { }
        protected DictData(int typeIndex, IData.Options options)
        {
            this.typeIndex = typeIndex;
            this.options = IData.Options.Named | options;
            this.dict = new Dictionary<string, T>();
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


        protected DictData<T> initNamed(IEnumerable<KeyValuePair<string, T>> namedValues)
        {
            if (this.dict.Count == 0)
            {
                this.dict = new Dictionary<string, T>(namedValues);
                return this;
            }
            else
                throw new InvalidOperationException($"Cannot initialize data on {this.options} structure that contains data. Please clear the structure beforee re-init.");
        }


        protected object getValueAt(string name) => this[name];

        public virtual V resolve<V>(DataAccessor accessor)
        {
            switch (accessor.accessType)
            {
                case DataAccessor.AccessTypes.Key:
                    return (V)this.getValueAt(accessor.key);
                default:
                    throw new InvalidOperationException($"Cannot acceess {this.GetType()} as {accessor.accessType}.");
            }
        }
        public virtual void assign(DataAccessor accessor, object value)
        {
            switch (accessor.accessType)
            {
                case DataAccessor.AccessTypes.Key:
                    this[accessor.key] = (T)value;
                    break;
                default:
                    throw new InvalidOperationException($"Cannot acceess {this.GetType()} as {accessor.accessType}.");
            }
        }

        public override string ToString()
        {
            string toString = string.Empty;
            if (this.options.HasFlag(IData.Options.Named))
                toString = $"[{string.Join(", ", dict)}]";
            else
                toString = $"INVALID {this.GetType()}";
            return base.ToString() + $"[{this.options}] = " + toString;
        }

    }

}

