using System;
using System.Collections.Generic;
using System.Linq;

namespace NETGraph.Core
{

    public abstract class Data
    {
        protected string name; // descriptor set by container/owner
        protected DataStructures structure = DataStructures.Scalar;
        protected int typeIndex;
        protected bool isResizable = true;

        public DataStructures Structure { get => structure; }
        public int TypeIndex { get => typeIndex; }
        public abstract int Count { get; }

        protected abstract object GetScalarUntyped();
        protected abstract void SetScalarUntyped(object value);
        protected abstract object GetAtUntyped(int index);
        protected abstract void SetAtUntyped(int index, object value);
        protected abstract object GetAtUntyped(string key);
        protected abstract void SetAtUntyped(string key, object value);


        public abstract void RemoveAt(int index);
        public abstract void RemoveRange(int index, int count);
        public abstract void RemoveAt(string key);
        public abstract void RemoveRange(params string[] keys);


        protected bool canCast(int typeIndex) => this.typeIndex == typeIndex;
        protected bool canCopy(int typeIndex, DataStructures structure) => (this.structure == structure && this.typeIndex == typeIndex); // does not check for size as this is left to copy function

        public virtual bool match(Data lh, Data rh) => lh.TypeIndex == rh.TypeIndex;
        // checks for match with structure
        public virtual bool matchStructure(Data lh, Data rh) => lh.TypeIndex == rh.TypeIndex && lh.Structure == rh.Structure;
        public virtual bool matchStructureAndSize(Data lh, Data rh) => lh.TypeIndex == rh.TypeIndex && lh.Structure == rh.Structure && lh.Count == rh.Count;
    }

    public abstract class Data<T> : Data
    {
        private T scalar;
        private List<T> list;
        private Dictionary<string, T> dict;

        protected Data(string name, int typeIndex, T scalar)
        {
            this.typeIndex = TypeIndices.Long;
            this.name = name;
            this.structure = DataStructures.Scalar;
            this.isResizable = false;
            this.Scalar = scalar;
        }
        protected Data(string name, int typeIndex, IEnumerable<T> values, bool isResizable)
        {
            this.structure = isResizable ? DataStructures.List : DataStructures.Array;
            this.isResizable = isResizable;
            this.list = new List<T>(values);
        }
        protected Data(string name, int typeIndex, int size, bool isResizable) : this(name, typeIndex, Enumerable.Repeat(default(T), size), isResizable)
        { }
        protected Data(string name, int typeIndex, IEnumerable<KeyValuePair<string, T>> namedValues, bool isResizable)
        {
            this.structure = DataStructures.Dict;
            this.isResizable = isResizable;
            this.dict = new Dictionary<string, T>(namedValues);
        }


        public override int Count
        {
            get
            {
                switch (this.structure)
                {
                    case DataStructures.Array: return list.Count;
                    case DataStructures.List: return list.Count;
                    case DataStructures.Dict: return dict.Count;
                }
                return 1;
            }
        }
        protected T Scalar
        {
            get
            {
                if (this.structure != DataStructures.Scalar)
                    throw new InvalidCastException($"{typeof(Data<T>)}[{this.name}] cannot be accessed as scalar.");
                return scalar;
            }
            set { if (this.structure == DataStructures.Scalar) scalar = value; }
        }
        protected T this[int index]
        {
            get
            {
                if (this.structure != DataStructures.Array && this.structure != DataStructures.List)
                    throw new InvalidCastException($"{typeof(Data<T>)}[{this.name}] cannot be accessed by index.");
                return list[index];
            }
            set => list[index] = value;
        }
        public T this[string key]
        {
            get
            {
                if (this.structure != DataStructures.Dict)
                    throw new InvalidCastException($"{typeof(Data<T>)}[{this.name}] cannot be accessed by key.");
                return dict[key];
            }
        }
        protected Dictionary<string, T> Dict
        {
            get
            {
                if (this.structure != DataStructures.Dict)
                    throw new InvalidCastException($"{typeof(Data<T>)}[{this.name}] cannot be accessed as dict.");
                return dict;
            }
        }

        #region Data untyped get and set implementations
        protected override object GetScalarUntyped() => this.scalar;
        protected override object GetAtUntyped(int index) => this[index];
        protected override object GetAtUntyped(string key) => this.dict[key];

        protected override void SetScalarUntyped(object value)
        {
            if (value is T)
                this.SetScalar((T)value);
            else
                throw new InvalidCastException($"{nameof(value)} has missmatching type and cannot be set");
        }
        protected override void SetAtUntyped(int index, object value)
        {
            if (value is T)
                this.SetAt(index, (T)value);
            else
                throw new InvalidCastException($"{nameof(value)} has missmatching type and cannot be set");
        }
        protected override void SetAtUntyped(string key, object value)
        {
            if (value is T)
                this.SetAt(key, (T)value);
            else
                throw new InvalidCastException($"{nameof(value)} has missmatching type and cannot be set");
        }

        #endregion

        #region Data Remove implementations
        public override void RemoveAt(int index)
        {
            if (!this.isResizable)
                throw new InvalidOperationException($"You cannot remove items from a non resizable structure.");
            if (this.Structure != DataStructures.List)
                throw new InvalidOperationException($"You cannot remove items from a non collection structure. Consider using a {DataStructures.List} structure.");
            this.list.RemoveAt(index);
        }
        public override void RemoveAt(string key)
        {
            if (!this.isResizable)
                throw new InvalidOperationException($"You cannot remove items from a non resizable structure.");
            if (this.Structure != DataStructures.List)
                throw new InvalidOperationException($"You cannot remove items from a non collection structure. Consider using a {DataStructures.Dict} structure.");
            this.dict.Remove(key);
        }
        public override void RemoveRange(int index, int count)
        {
            if (!this.isResizable)
                throw new InvalidOperationException($"You cannot remove items from a non resizable structure.");
            if (this.Structure != DataStructures.List)
                throw new InvalidOperationException($"You cannot remove items from a non collection structure. Consider using a {DataStructures.List} structure.");
            this.list.RemoveRange(index, count);
        }
        public override void RemoveRange(params string[] keys)
        {
            if (!this.isResizable)
                throw new InvalidOperationException($"You cannot remove items from a non resizable structure.");
            if (this.Structure != DataStructures.List)
                throw new InvalidOperationException($"You cannot remove items from a non collection structure. Consider using a {DataStructures.Dict} structure.");
            foreach (string key in keys)
                this.dict.Remove(key);
        }
        #endregion


        #region Data<T> get and set implementations
        public virtual T GetScalar() => this.Scalar;
        public virtual T GetAt(int index) => this[index];
        public virtual T GetAt(string key) => this.Dict[key];
        public virtual void SetScalar(T scalarValue) => this.Scalar = scalarValue;
        public virtual void SetAt(int index, T value) => this[index] = value;
        public virtual void SetAt(string knobName, T value)
        {
            if (this.Dict.ContainsKey(knobName))
                this.Dict[knobName] = value;
            else
                this.Dict.Add(knobName, value);
        }
        #endregion

        public virtual bool matchExact(Data<T> rh)
        {
            // if self reference, return early true
            if (this == rh)
                return true;
            if (matchStructureAndSize(this, rh))
            {
                if (this.Structure == DataStructures.Dict)
                {
                    foreach (string key in this.dict.Keys)
                        if (!rh.dict.ContainsKey(key)) return false;
                    return true;
                }
                return true;
            }
            return false;
        }
        public virtual bool matchWithValue(Data<T> rh)
        {
            // if self reference, return early true
            if (this == rh)
                return true;
            if (matchStructureAndSize(this, rh))
            {
                if (this.Structure == DataStructures.Dict)
                {
                    foreach (string key in this.dict.Keys)
                    {
                        if (!rh.dict.ContainsKey(key)) return false;
                        if (rh.dict.TryGetValue(key, out T rhValue))
                            if (!this[key].Equals(rhValue)) return false;
                    }
                    return true;
                }
                if (this.Structure == DataStructures.Array || this.Structure == DataStructures.List)
                {
                    for (int i = 0; i < this.Count; i++)
                        if (!this[i].Equals(rh[i]))
                            return false;
                    return true;
                }
                if (this.Structure == DataStructures.Scalar)
                    return this.scalar.Equals(rh.scalar);
            }
            return false;
        }


        public override string ToString()
        {
            string toString = string.Empty;
            if (this.structure == DataStructures.Scalar)
                toString = $"{scalar}";
            else if (this.structure == DataStructures.Array || this.structure == DataStructures.List)
                toString = $"[{string.Join(", ", list)}]";
            else if (this.structure == DataStructures.Dict)
                toString = $"[{string.Join(", ", dict)}]";

            return base.ToString() + $"[{this.structure}] = " + toString;
        }

    }

}

