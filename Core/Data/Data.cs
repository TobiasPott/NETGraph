﻿using System;
using System.Collections.Generic;
using System.Linq;
using NETGraph.Core;
using NETGraph.Core.Meta;

namespace NETGraph.Data
{

    public enum DataStructure
    {
        Scalar,
        List,
        Named,
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

        public static bool match(IData lh, IData rh) => lh.TypeIndex == rh.TypeIndex;
        // checks for match with structure
        public static bool matchStructure(IData lh, IData rh) => lh.TypeIndex == rh.TypeIndex && lh.Structure == rh.Structure;
        public static bool matchStructureAndSize(IData lh, IData rh) => lh.TypeIndex == rh.TypeIndex && lh.Structure == rh.Structure && lh.Count == rh.Count;

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

        protected bool canCast(int typeIndex) => this.typeIndex == typeIndex;
        protected bool canCopy(int typeIndex, DataStructure structure) => (this.structure == structure && this.typeIndex == typeIndex);

        // accessing nested DataBase objects and resolving to actual types
        public abstract IData access(DataSignature signature);
        public abstract V resolve<V>(DataSignature signature);
        public abstract bool resolve<V>(DataSignature signature, out V value);

    }

    public abstract class DataBase<T> : DataBase
    {
        private T scalar;
        private List<T> list;
        private Dictionary<string, T> dict;


        protected DataBase(DataTypes type, DataStructure structure, bool isResizable) : base(type, structure, isResizable)
        {
            if (structure == DataStructure.List)
                this.list = new List<T>();
            else if (structure == DataStructure.Named)
                this.dict = new Dictionary<string, T>();
        }
        protected DataBase(int typeIndex, DataStructure structure, bool isResizable) : this((DataTypes)typeIndex, structure, isResizable)
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



        protected DataBase<T> initScalar(T scalar)
        {
            this.Scalar = scalar;
            return this;
        }
        protected DataBase<T> initList(IEnumerable<T> values)
        {
            if (this.list.Count == 0)
            {
                this.list = new List<T>(values);
                return this;
            }
            else
                throw new InvalidOperationException($"Cannot initialize data on {this.structure} structure that contains data. Please clear the structure beforee re-init.");
        }
        protected DataBase<T> initNamed(IEnumerable<KeyValuePair<string, T>> namedValues)
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



        public virtual bool matchExact(DataBase<T> rh)
        {
            // if self reference, return early true
            if (this == rh)
                return true;
            if (IData.matchStructureAndSize(this, rh))
            {
                if (this.Structure == DataStructure.Named)
                {
                    foreach (string key in this.dict.Keys)
                        if (!rh.dict.ContainsKey(key)) return false;
                    return true;
                }
                return true;
            }
            return false;
        }
        public virtual bool matchWithValue(DataBase<T> rh)
        {
            // if self reference, return early true
            if (this == rh)
                return true;
            if (IData.matchStructureAndSize(this, rh))
            {
                if (this.Structure == DataStructure.Named)
                {
                    foreach (string key in this.dict.Keys)
                    {
                        if (!rh.dict.ContainsKey(key)) return false;
                        if (rh.dict.TryGetValue(key, out T rhValue))
                            if (!this[key].Equals(rhValue)) return false;
                    }
                    return true;
                }
                if (this.Structure == DataStructure.List)
                {
                    for (int i = 0; i < this.Count; i++)
                        if (!this[i].Equals(rh[i]))
                            return false;
                    return true;
                }
                if (this.Structure == DataStructure.Scalar)
                    return this.scalar.Equals(rh.scalar);
            }
            return false;
        }


        private void throwCheckResizable()
        {
#if !DEBUG
            if (!this.isResizable)
                throw new InvalidOperationException($"You cannot add items to a non resizable structure.");
#endif
        }
        private void throwCheckStructure(DataStructure structure)
        {

#if !DEBUG
            if (this.Structure != structure)
                throw new InvalidOperationException($"You cannot add items to a non collection structure. Consider using a {structure} structure.");
#endif
        }
        private void throwCheckGenericType(object item)
        {

#if !DEBUG
            if (!(item is T))
                throw new InvalidCastException($"{nameof(item)} has missmatching type.");
#endif
        }
        private void throwCheckAccessByIndex()
        {
#if !DEBUG
            if (this.structure != DataStructures.Array && this.structure != DataStructures.List)
                throw new InvalidCastException($"{typeof(DataBase<T>)} cannot be accessed by index.");
#endif
        }
        private void throwCheckAccessByKey()
        {
#if !DEBUG
            if (this.structure != DataStructures.Named)
                throw new InvalidCastException($"{typeof(DataBase<T>)} cannot be accessed by key.");
#endif
        }
        private void throwCheckAccessByScalar()
        {
#if !DEBUG
            if (this.structure != DataStructures.Scalar)
                throw new InvalidCastException($"{typeof(DataBase<T>)} cannot be accessed as scalar.");
#endif
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

