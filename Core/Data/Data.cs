using System;
using System.Collections.Generic;
using System.Linq;

namespace NETGraph.Data
{

    public abstract class DataBase
    {
        protected DataStructures structure = DataStructures.Scalar;
        protected int typeIndex;
        protected bool isResizable;

        public DataStructures Structure { get => structure; }
        public int TypeIndex { get => typeIndex; }
        public abstract int Count { get; }


        protected DataBase(DataTypes type, DataStructures structure, bool isResizable = true) : this((int)type, structure, isResizable)
        { }
        protected DataBase(int typeIndex, DataStructures structure, bool isResizable = true)
        {
            this.typeIndex = typeIndex;
            this.structure = structure;
            this.isResizable = structure == DataStructures.Scalar ? false : isResizable;
        }


        public abstract void Add(object item);
        public abstract void Add(string key, object item);

        public abstract void RemoveAt(int index);
        public abstract void RemoveRange(int index, int count);
        public abstract void RemoveAt(string key);
        public abstract void RemoveRange(params string[] keys);


        protected bool canCast(int typeIndex) => this.typeIndex == typeIndex;
        protected bool canCopy(int typeIndex, DataStructures structure) => (this.structure == structure && this.typeIndex == typeIndex); // does not check for size as this is left to copy function

        public virtual bool match(DataBase lh, DataBase rh) => lh.TypeIndex == rh.TypeIndex;
        // checks for match with structure
        public virtual bool matchStructure(DataBase lh, DataBase rh) => lh.TypeIndex == rh.TypeIndex && lh.Structure == rh.Structure;
        public virtual bool matchStructureAndSize(DataBase lh, DataBase rh) => lh.TypeIndex == rh.TypeIndex && lh.Structure == rh.Structure && lh.Count == rh.Count;
    }


    public abstract class DataBase<T> : DataBase
    {
        private T scalar;
        private List<T> list;
        private Dictionary<string, T> dict;


        protected DataBase(DataTypes type, DataStructures structure, bool isResizable) : base(type, structure, isResizable)
        { }
        protected DataBase(int typeIndex, DataStructures structure, bool isResizable) : base(typeIndex, structure, isResizable)
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
                    case DataStructures.Named: return dict.Count;
                }
                return 1;
            }
        }



        protected DataBase<T> initData(T scalar)
        {
            this.Scalar = scalar;
            return this;
        }
        protected DataBase<T> initData(IEnumerable<T> values)
        {
            if (this.list == null)
            {
                this.list = new List<T>(values);
                return this;
            }
            else
                throw new InvalidOperationException($"Cannot initialize data on assigned {this.structure} structure. Please init data only once.");
        }
        protected DataBase<T> initData(IEnumerable<KeyValuePair<string, T>> keyedValues)
        {
            if (this.dict == null)
            {
                this.dict = new Dictionary<string, T>(keyedValues);
                return this;
            }
            else
                throw new InvalidOperationException($"Cannot initialize data on assigned {this.structure} structure. Please init data only once.");
        }


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
            throwCheckStructure(DataStructures.Named);
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
            throwCheckStructure(DataStructures.Named);
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
            throwCheckStructure(DataStructures.Named);
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

        public virtual bool matchExact(DataBase<T> rh)
        {
            // if self reference, return early true
            if (this == rh)
                return true;
            if (matchStructureAndSize(this, rh))
            {
                if (this.Structure == DataStructures.Named)
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
            if (matchStructureAndSize(this, rh))
            {
                if (this.Structure == DataStructures.Named)
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
                throw new InvalidCastException($"{typeof(DataBase<T>)} cannot be accessed by index.");
        }
        private void throwCheckAccessByKey()
        {
            if (this.structure != DataStructures.Named)
                throw new InvalidCastException($"{typeof(DataBase<T>)} cannot be accessed by key.");
        }
        private void throwCheckAccessByScalar()
        {
            if (this.structure != DataStructures.Scalar)
                throw new InvalidCastException($"{typeof(DataBase<T>)} cannot be accessed as scalar.");
        }



        public override string ToString()
        {
            string toString = string.Empty;
            if (this.structure == DataStructures.Scalar)
                toString = $"{scalar}";
            else if (this.structure == DataStructures.Array || this.structure == DataStructures.List)
                toString = $"[{string.Join(", ", list)}]";
            else if (this.structure == DataStructures.Named)
                toString = $"[{string.Join(", ", dict)}]";

            return base.ToString() + $"[{this.structure}] = " + toString;
        }

    }

}

