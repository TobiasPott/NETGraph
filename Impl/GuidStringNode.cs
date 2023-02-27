using System;
using System.Collections.Generic;
using System.Linq;
using NETGraph.Core;

namespace NETGraph.Impl.Generics
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


        protected bool canCast(int typeIndex) => this.typeIndex == typeIndex;
        protected bool canCopy(int typeIndex, DataStructures structure) => (this.structure == structure && this.typeIndex == typeIndex); // does not check for size as this is left to copy function

    }

    public abstract class Data<T> : Data
    {
        private T scalar;
        private List<T> list;
        private Dictionary<string, T> dict;

        protected Data(T scalar)
        {
            this.structure = DataStructures.Scalar;
            this.isResizable = false;
            this.Scalar = scalar;
        }
        protected Data(IEnumerable<T> values, bool isResizable)
        {
            this.structure = isResizable ? DataStructures.List : DataStructures.Array;
            this.isResizable = isResizable;
            this.list = new List<T>(values);
        }
        protected Data(int size, bool isResizable) : this(Enumerable.Repeat(default(T), size), isResizable)
        { }
        protected Data(IEnumerable<KeyValuePair<string, T>> namedValues, bool isResizable)
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
        protected Dictionary<string, T> Dict
        {
            get
            {
                if (this.structure != DataStructures.Dict)
                    throw new InvalidCastException($"{typeof(Data<T>)}[{this.name}] cannot be accessed as dict.");
                return dict;
            }
        }


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

    // ToDo: Add node description to describe knob mapping and behaviour (optional!)
    // ToDo: Wrap/Encapsulate data structure into own type and build node content based of definitions
    public abstract class KnobNode<T> : KnobNode<Guid, string>
    {
        private DataStructures structure = DataStructures.Scalar;
        private bool isResizable = true;

        private T scalar;
        private List<T> list;
        private Dictionary<string, T> dict;


        protected int Count
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
        protected T AsScalar
        {
            get
            {
                if (this.structure != DataStructures.Scalar)
                    throw new InvalidCastException($"{typeof(KnobNode<T>)}[{this.id}] cannot be accessed as scalar.");
                return scalar;
            }
            set { if (this.structure == DataStructures.Scalar) scalar = value; }
        }
        public T this[int index]
        {
            get
            {
                if (this.structure != DataStructures.Array && this.structure != DataStructures.List)
                    throw new InvalidCastException($"{typeof(KnobNode<T>)}[{this.id}] cannot be accessed by index.");
                return list[index];
            }
            set => list[index] = value;
        }
        protected Dictionary<string, T> AsDict
        {
            get
            {
                if (this.structure != DataStructures.Dict)
                    throw new InvalidCastException($"{typeof(KnobNode<T>)}[{this.id}] cannot be accessed as dict.");
                return dict;
            }
        }


        protected KnobNode(Guid id, DataStructures structure = DataStructures.Scalar) : base(id)
        {
            this.structure = structure;
            this.buildFrom(structure);
        }
        protected KnobNode(Guid id, DataStructures structure, int size) : base(id)
        {
            this.structure = structure;
            this.buildFrom(structure, size);
        }
        protected KnobNode(Guid id, T[] values) : base(id)
        {
            this.structure = DataStructures.Array;
            this.buildFrom(structure, values.Length);
        }



        protected void buildFrom(DataStructures structure, int size = 1)
        {
            switch (structure)
            {
                case DataStructures.Array:
                case DataStructures.List:
                    list = new List<T>(Enumerable.Repeat<T>(default(T), size));
                    return;
                case DataStructures.Dict:
                    dict = new Dictionary<string, T>(size);
                    return;
                case DataStructures.Scalar:
                    scalar = default(T);
                    return;
            }

            throw new ArgumentException($"{structure} is not supported and the {nameof(KnobNode<T>)} is not build correctly.");
        }


        protected T GetScalar() => AsScalar;
        protected T GetAt(int index) => this[index];
        protected T GetAt(string knobName) => AsDict[knobName];

        protected void SetScalar(T scalarValue) => AsScalar = scalarValue;
        protected void SetAt(int index, T value) => this[index] = value;
        protected void SetAt(string knobName, T value)
        {
            if (AsDict.ContainsKey(knobName))
                AsDict[knobName] = value;
            else
                AsDict.Add(knobName, value);
        }
        // ToDo: Add overloads for collection arguments for Add and Set methods to add ranges or set (with index/length, to use direct copy/memCopy)

        protected void Add(T value)
        {
            if (!isResizable)
                throw new InvalidOperationException($"You cannot add items to a non resizable structure.");
            if (this.structure == DataStructures.List)
                this.list.Add(value);
            throw new InvalidOperationException($"You can only add items to a {DataStructures.List} structure. You tried to add to {this.structure} structure instead.");
        }
        protected void Add(string knobName, T value)
        {
            if (!isResizable)
                throw new InvalidOperationException($"You cannot add items to a non resizable structure.");
            if (this.structure == DataStructures.Dict)
            {
                if (this.dict.ContainsKey(knobName))
                    this.dict[knobName] = value;
                else
                    this.dict.Add(knobName, value);
            }
            throw new InvalidOperationException($"You can only add named items to a {DataStructures.Dict} structure. You tried to add to {this.structure} structure instead.");
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

