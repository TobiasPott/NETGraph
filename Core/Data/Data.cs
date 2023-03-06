using System;
using System.Collections.Generic;
using System.Linq;
using NETGraph.Core;
using NETGraph.Core.Meta;

namespace NETGraph.Data
{

    public enum DataStructure
    {
        Scalar = 1,
        List = 2,
        Named = 4,
    }

    public static class DataExtensions
    {

        public static bool match(IData lh, IData rh) => lh.TypeIndex == rh.TypeIndex;
        public static bool matchStructure(IData lh, IData rh) => lh.TypeIndex == rh.TypeIndex && lh.Structure == rh.Structure;
        public static bool matchStructureAndSize(IData lh, IData rh) => lh.TypeIndex == rh.TypeIndex && lh.Structure == rh.Structure && lh.Count == rh.Count;


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
        DataStructure Structure { get; }
        int TypeIndex { get; }
        int Count { get; }


        object getValueScalar();
        object getValueAt(int index);
        object getValueAt(string key);

        IData access(DataSignature signature);
        V resolve<V>(DataSignature signature);
        bool resolve<V>(DataSignature signature, out V value);

        void assign(object scalar);
        void assign(int index, object scalar);
        void assign(string name, object scalar);

        DataResolver resolver();
        DataResolver resolver(string key);
        DataResolver resolver(int index);

    }


    public abstract class DataBase : IData
    {
        protected DataStructure structure = DataStructure.Scalar;
        protected int typeIndex;
        protected bool isResizable;

        public DataStructure Structure { get => structure; }
        public int TypeIndex { get => typeIndex; }
        public virtual int Count { get; } = 1;


        protected DataBase(DataTypes type, DataStructure structure, bool isResizable = true) : this((int)type, structure, isResizable)
        { }
        protected DataBase(int typeIndex, DataStructure structure, bool isResizable = true)
        {
            this.typeIndex = typeIndex;
            this.structure = structure;
            this.isResizable = structure == DataStructure.Scalar ? false : isResizable;
        }

        public abstract object getValueScalar();
        public abstract object getValueAt(int index);
        public abstract object getValueAt(string key);
        public abstract void assign(object scalar);
        public abstract void assign(int index, object scalar);
        public abstract void assign(string name, object scalar);

        public DataResolver resolver() => new DataResolver(this, string.Empty);
        public DataResolver resolver(string key) => new DataResolver(this, "." + key);
        public DataResolver resolver(int index) => new DataResolver(this, $"[{index}]");

        // accessing nested DataBase objects and resolving to actual types
        public abstract IData access(DataSignature signature);
        public abstract V resolve<V>(DataSignature signature);
        public abstract bool resolve<V>(DataSignature signature, out V value);

    }

    public class Data<T> : DataBase
    {
        public static GeneratorDefinition Generator()
        {
            return new GeneratorDefinition(
            (s) => new Data<T>(typeof(T), DataStructure.Scalar, false).initScalar((T)s),
            (s, r) => new Data<T>(typeof(T), DataStructure.List, r),
            (r) => new Data<T>(typeof(T), DataStructure.Named, r)
        );
        }

        private T scalar;
        private List<T> list;
        private Dictionary<string, T> dict;


        protected Data(Type type, DataStructure structure, bool isResizable) : base(TypeMapping.instance.BuiltInDataTypeFor(type), structure, isResizable)
        { }
        protected Data(DataTypes type, DataStructure structure, bool isResizable) : base(type, structure, isResizable)
        {
            if (structure == DataStructure.List)
                this.list = new List<T>();
            else if (structure == DataStructure.Named)
                this.dict = new Dictionary<string, T>();
        }
        protected Data(int typeIndex, DataStructure structure, bool isResizable) : this((DataTypes)typeIndex, structure, isResizable)
        { }


        protected T this[int index]
        {
            get
            {
                throwCheckAccessByIndex();
                return list[index];
            }
            set => list[index] = value;
        }
        protected T this[string name]
        {
            get
            {
                throwCheckAccessByKey();
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
                throwCheckAccessByScalar();
                return scalar;
            }
            set { if (this.structure == DataStructure.Scalar) scalar = value; }
        }

        public override int Count
        {
            get
            {
                switch (this.structure)
                {
                    case DataStructure.List: return list.Count;
                    case DataStructure.Named: return dict.Count;
                }
                return 1;
            }
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
                throw new InvalidOperationException($"Cannot initialize data on {this.structure} structure that contains data. Please clear the structure beforee re-init.");
        }
        protected Data<T> initNamed(IEnumerable<KeyValuePair<string, T>> namedValues)
        {
            if (this.dict.Count == 0)
            {
                this.dict = new Dictionary<string, T>(namedValues);
                return this;
            }
            else
                throw new InvalidOperationException($"Cannot initialize data on {this.structure} structure that contains data. Please clear the structure beforee re-init.");
        }

        public override IData access(DataSignature signature)
        {
            switch (signature.accessType)
            {
                case DataSignature.AccessTypes.Key:
                    return this[signature.key] as IData;
                case DataSignature.AccessTypes.Index:
                    return this[signature.index] as IData;
                case DataSignature.AccessTypes.Scalar:
                default:
                    return this;
            }
        }
        public override V resolve<V>(DataSignature signature)
        {
            if (resolve<V>(signature, out V value))
                return value;
            return default(V);

        }
        public override bool resolve<V>(DataSignature signature, out V value)
        {
            switch (signature.accessType)
            {
                case DataSignature.AccessTypes.Scalar:
                    value = (V)this.getValueScalar();
                    return true;
                case DataSignature.AccessTypes.Index:
                    value = (V)this.getValueAt(signature.index);
                    return true;
                case DataSignature.AccessTypes.Key:
                    value = (V)this.getValueAt(signature.key);
                    return true;
                default:
                    value = default(V);
                    return false;
            }
        }

        public override object getValueScalar() => this.Scalar;
        public override object getValueAt(int index) => this[index];
        public override object getValueAt(string name) => this[name];
        public override void assign(object scalar) => this.Scalar = (T)scalar;
        public override void assign(int index, object scalar) => this[index] = (T)scalar;
        public override void assign(string name, object scalar) => this[name] = (T)scalar;


        #region Data<T> get and set implementations
        public virtual T getScalar() => this.Scalar;
        public virtual T getAt(int index) => this[index];
        public virtual T getAt(string name) => this[name];
        public virtual void setScalar(T scalarValue) => this.Scalar = scalarValue;
        public virtual void setAt(int index, T value) => this[index] = value;
        public virtual void setAt(string name, T value) => this[name] = value;
        #endregion


        //#region Data Add & Remove implementations
        //public override void Add(object item)
        //{
        //    throwCheckResizable();
        //    throwCheckStructure(DataStructures.List);
        //    // typecheck
        //    throwCheckGenericType(item);

        //    this.list.Add((T)item);
        //}
        //public override void Add(string key, object item)
        //{
        //    throwCheckResizable();
        //    throwCheckStructure(DataStructures.Named);
        //    // typecheck
        //    throwCheckGenericType(item);

        //    this.dict.Add(key, (T)item);
        //}

        //public override void RemoveAt(int index)
        //{
        //    throwCheckResizable();
        //    throwCheckStructure(DataStructures.List);
        //    this.list.RemoveAt(index);
        //}
        //public override void RemoveAt(string key)
        //{
        //    throwCheckResizable();
        //    throwCheckStructure(DataStructures.Named);
        //    this.dict.Remove(key);
        //}
        //public override void RemoveRange(int index, int count)
        //{
        //    throwCheckResizable();
        //    throwCheckStructure(DataStructures.List);
        //    this.list.RemoveRange(index, count);
        //}
        //public override void RemoveRange(params string[] keys)
        //{
        //    throwCheckResizable();
        //    throwCheckStructure(DataStructures.Named);
        //    foreach (string key in keys)
        //        this.dict.Remove(key);
        //}
        //#endregion

        private void throwCheckResizable()
        {
            if (!this.isResizable)
                throw new InvalidOperationException($"You cannot add items to a non resizable structure.");
        }
        private void throwCheckStructure(DataStructure structure)
        {
            if (this.Structure != structure)
                throw new InvalidOperationException($"You cannot add items to a non collection structure. Consider using a {structure} structure.");
        }
        private void throwCheckGenericType(object item)
        {
            if (!(item is T))
                throw new InvalidCastException($"{nameof(item)} has missmatching type.");
        }
        private void throwCheckAccessByIndex()
        {
            if (this.structure != DataStructure.List)
                throw new InvalidCastException($"{typeof(Data<T>)} cannot be accessed by index.");
        }
        private void throwCheckAccessByKey()
        {
            if (this.structure != DataStructure.Named)
                throw new InvalidCastException($"{typeof(Data<T>)} cannot be accessed by key.");
        }
        private void throwCheckAccessByScalar()
        {
            if (this.structure != DataStructure.Scalar)
                throw new InvalidCastException($"{typeof(Data<T>)} cannot be accessed as scalar.");
        }



        public override string ToString()
        {
            string toString = string.Empty;
            if (this.structure == DataStructure.Scalar)
                toString = $"{scalar}";
            else if (this.structure == DataStructure.List)
                toString = $"[{string.Join(", ", list)}]";
            else if (this.structure == DataStructure.Named)
                toString = $"[{string.Join(", ", dict)}]";

            return base.ToString() + $"[{this.structure}] = " + toString;
        }

    }

}

