using System;
using System.Collections.Generic;
using System.Linq;
using NETGraph.Core;

namespace NETGraph.Impl.Generics
{
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

