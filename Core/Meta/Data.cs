using System;
using System.Collections.Generic;
using System.Linq;
using NETGraph.Core;

namespace NETGraph.Core.Meta
{
    [Flags()]
    public enum DataOptions
    {
        Scalar = 1,
        List = 2,
        Named = 4,
        Resizable = 8,

    }

    public static class DataExtensions
    {

        public static void assign<V>(this IData data, string signature, V value) => data.assign(new DataSignature(signature), value);
        public static DataResolver resolver(this IData data, string signature) => data.resolver(new DataSignature(signature));

        public static bool match(IData lh, IData rh) => lh.typeIndex == rh.typeIndex;
        public static bool matchStructure(IData lh, IData rh) => lh.typeIndex == rh.typeIndex && lh.options == rh.options;
        //public static bool matchStructureAndSize(IData lh, IData rh) => lh.typeIndex == rh.typeIndex && lh.options == rh.options && lh.count == rh.count;


        //public static bool matchExact<T>(Data<T> lh, Data<T> rh)
        //{
        //    // if self reference, return early true
        //    if (lh == rh)
        //        return true;
        //    if (matchStructureAndSize(lh, rh))
        //    {
        //        if (lh.Structure == DataStructure.Named)
        //        {
        //            foreach (string key in lh.dict.Keys)
        //                if (!rh.dict.ContainsKey(key)) return false;
        //            return true;
        //        }
        //        return true;
        //    }
        //    return false;
        //}
        //public virtual bool matchWithValue<T>(Data<T> lh, Data<T> rh)
        //{
        //    // if self reference, return early true
        //    if (lh == rh)
        //        return true;
        //    if (matchStructureAndSize(lh, rh))
        //    {
        //        if (lh.Structure == DataStructure.Named)
        //        {
        //            foreach (string key in this.dict.Keys)
        //            {
        //                if (!rh.dict.ContainsKey(key)) return false;
        //                if (rh.dict.TryGetValue(key, out T rhValue))
        //                    if (!lh[key].Equals(rhValue)) return false;
        //            }
        //            return true;
        //        }
        //        if (lh.Structure == DataStructure.List)
        //        {
        //            for (int i = 0; i < lh.Count; i++)
        //                if (!lh[i].Equals(rh[i]))
        //                    return false;
        //            return true;
        //        }
        //        if (lh.Structure == DataStructure.Scalar)
        //            return lh.scalar.Equals(rh.scalar);
        //    }
        //    return false;
        //}

    }

    public interface IData
    {
        DataOptions options { get; }
        int typeIndex { get; }

        // methods for accessing nested or dynamic data instances
        IData access(string dataPath);


        //method to resolve data object into underlying type instances
        V resolve<V>(DataSignature signature);
        void assign(DataSignature signature, object scalar);

        DataResolver resolver(DataSignature signature);

    }


    public abstract class DataBase : IData
    {
        public DataOptions options { get; protected set; }
        public int typeIndex { get; protected set; }

        protected DataBase(DataTypes type, DataOptions options, bool isResizable = true) : this((int)type, options, isResizable)
        { }
        protected DataBase(int typeIndex, DataOptions options, bool isResizable = true)
        {
            this.typeIndex = typeIndex;
            this.options = options;
        }


        // methods for accessing nested or dynamic data instances
        public abstract IData access(string dataPath);
        //method to resolve data object into underlying type instances
        public abstract void assign(DataSignature signature, object scalar);
        public abstract V resolve<V>(DataSignature signature);
        public DataResolver resolver(DataSignature signature) => new DataResolver(this, signature);
    }

    public class Data<T> : DataBase
    {
        public static GeneratorDefinition Generator()
        {
            return new GeneratorDefinition(
            () => new Data<T>(typeof(T), DataOptions.Scalar),
            (r) => new Data<T>(typeof(T), DataOptions.List | DataOptions.Resizable),
            (r) => new Data<T>(typeof(T), DataOptions.Named | DataOptions.Resizable)
        );
        }

        private T scalar;
        private List<T> list;
        private Dictionary<string, T> dict;


        protected Data(Type type, DataOptions options) : this(TypeMapping.instance.BuiltInDataTypeFor(type), options)
        { }
        protected Data(int typeIndex, DataOptions structure) : this((DataTypes)typeIndex, structure)
        { }
        protected Data(DataTypes type, DataOptions options) : base(type, options)
        {
            if (options.HasFlag(DataOptions.List))
                this.list = new List<T>();
            else if (options.HasFlag(DataOptions.Named))
                this.dict = new Dictionary<string, T>();
        }


        protected T this[int index]
        {
            get
            {
                //throwCheckAccessByIndex();
                return list[index];
            }
            set => list[index] = value;
        }
        protected T this[string name]
        {
            get
            {
                //throwCheckAccessByKey();
                return dict[name];
            }
            set
            {
                if (!dict.ContainsKey(name))
                    dict.Add(name, value);
                else
                    dict[name] = value;
            }
        }
        protected T Scalar
        {
            get
            {
                //throwCheckAccessByScalar();
                return scalar;
            }
            set { if (this.options == DataOptions.Scalar) scalar = value; }
        }



        protected Data<T> initScalar(T scalar)
        {
            this.Scalar = scalar;
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


        protected object getValueScalar() => this.Scalar;
        protected object getValueAt(int index) => this[index];
        protected object getValueAt(string name) => this[name];

        public override V resolve<V>(DataSignature signature)
        {
            switch (signature.accessType)
            {
                case DataSignature.AccessTypes.Scalar:
                    return (V)this.getValueScalar();
                case DataSignature.AccessTypes.Index:
                    return (V)this.getValueAt(signature.index);
                case DataSignature.AccessTypes.Key:
                    return (V)this.getValueAt(signature.key);
                default:
                    return default(V);
            }
        }
        public override void assign(DataSignature signature, object value)
        {
            switch (signature.accessType)
            {
                case DataSignature.AccessTypes.Scalar:
                    this.Scalar = (T)value;
                    break;
                case DataSignature.AccessTypes.Index:
                    this[signature.index] = (T)value;
                    break;
                case DataSignature.AccessTypes.Key:
                    this[signature.key] = (T)value;
                    break;
                default:
                    break;
            }
        }

        private void throwCheckResizable()
        {
            if (!this.options.HasFlag(DataOptions.Resizable))
                throw new InvalidOperationException($"You cannot add items to a non resizable structure.");
        }
        private void throwCheckStructure(DataOptions structure)
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
            if (!this.options.HasFlag(DataOptions.List))
                throw new InvalidCastException($"{typeof(Data<T>)} cannot be accessed by index.");
        }
        private void throwCheckAccessByKey()
        {
            if (!this.options.HasFlag(DataOptions.Named))
                throw new InvalidCastException($"{typeof(Data<T>)} cannot be accessed by key.");
        }
        private void throwCheckAccessByScalar()
        {
            if (!this.options.HasFlag(DataOptions.Scalar))
                throw new InvalidCastException($"{typeof(Data<T>)} cannot be accessed as scalar.");
        }



        public override string ToString()
        {
            string toString = string.Empty;
            if (this.options == DataOptions.Scalar)
                toString = $"{scalar}";
            else if (this.options == DataOptions.List)
                toString = $"[{string.Join(", ", list)}]";
            else if (this.options == DataOptions.Named)
                toString = $"[{string.Join(", ", dict)}]";

            return base.ToString() + $"[{this.options}] = " + toString;
        }

    }

}

