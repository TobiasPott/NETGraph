using System;
using System.Collections.Generic;
using System.Linq;

namespace NETGraph.Core
{

    public abstract class Data
    {
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


        public abstract void Add(object item);
        public abstract void Add(string key, object item);

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

        protected Data(int typeIndex, T scalar)
        {
            this.typeIndex = TypeIndices.Long;
            this.structure = DataStructures.Scalar;
            this.isResizable = false;
            this.Scalar = scalar;
        }
        protected Data(int typeIndex, IEnumerable<T> values, bool isResizable)
        {
            this.structure = isResizable ? DataStructures.List : DataStructures.Array;
            this.isResizable = isResizable;
            this.list = new List<T>(values);
        }
        protected Data(int typeIndex, int size, bool isResizable) : this(typeIndex, Enumerable.Repeat(default(T), size), isResizable)
        { }
        protected Data(int typeIndex, IEnumerable<KeyValuePair<string, T>> namedValues, bool isResizable)
        {
            this.structure = DataStructures.Dict;
            this.isResizable = isResizable;
            this.dict = new Dictionary<string, T>(namedValues);
        }

        protected T this[int index]
        {
            get
            {
                throwCheckAccessByIndex();
                return list[index];
            }
            set => list[index] = value;
        }
        protected T this[string key]
        {
            get
            {
                throwCheckAccessByKey();
                return dict[key];
            }
            set
            {
                if (!dict.ContainsKey(key))
                    dict.Add(key, value);
                else
                    dict[key] = value;
            }
        }
        protected T Scalar
        {
            get
            {
                throwCheckAccessByScalar();
                return scalar;
            }
            set { if (this.structure == DataStructures.Scalar) scalar = value; }
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



        #region Data Add & Remove implementations
        public override void Add(object item)
        {
            throwCheckResizable();
            throwCheckStructure(DataStructures.List);
            // typecheck
            throwCheckGenericType(item);

            this.list.Add((T)item);
        }
        public override void Add(string key, object item)
        {
            throwCheckResizable();
            throwCheckStructure(DataStructures.Dict);
            // typecheck
            throwCheckGenericType(item);

            this.dict.Add(key, (T)item);
        }

        public override void RemoveAt(int index)
        {
            throwCheckResizable();
            throwCheckStructure(DataStructures.List);
            this.list.RemoveAt(index);
        }
        public override void RemoveAt(string key)
        {
            throwCheckResizable();
            throwCheckStructure(DataStructures.Dict);
            this.dict.Remove(key);
        }
        public override void RemoveRange(int index, int count)
        {
            throwCheckResizable();
            throwCheckStructure(DataStructures.List);
            this.list.RemoveRange(index, count);
        }
        public override void RemoveRange(params string[] keys)
        {
            throwCheckResizable();
            throwCheckStructure(DataStructures.Dict);
            foreach (string key in keys)
                this.dict.Remove(key);
        }
        #endregion


        #region Data<T> get and set implementations
        public virtual T GetScalar() => this.Scalar;
        public virtual T GetAt(int index) => this[index];
        public virtual T GetAt(string key) => this[key];
        public virtual void SetScalar(T scalarValue) => this.Scalar = scalarValue;
        public virtual void SetAt(int index, T value) => this[index] = value;
        public virtual void SetAt(string key, T value) => this[key] = value;
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



        private void throwCheckResizable()
        {
            if (!this.isResizable)
                throw new InvalidOperationException($"You cannot add items to a non resizable structure.");
        }
        private void throwCheckStructure(DataStructures structure)
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
            if (this.structure != DataStructures.Array && this.structure != DataStructures.List)
                throw new InvalidCastException($"{typeof(Data<T>)} cannot be accessed by index.");
        }
        private void throwCheckAccessByKey()
        {
            if (this.structure != DataStructures.Dict)
                throw new InvalidCastException($"{typeof(Data<T>)} cannot be accessed by key.");
        }
        private void throwCheckAccessByScalar()
        {
            if (this.structure != DataStructures.Scalar)
                throw new InvalidCastException($"{typeof(Data<T>)} cannot be accessed as scalar.");
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

