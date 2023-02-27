using System;
using System.Collections.Generic;
using System.Linq;
using NETGraph.Core;

namespace NETGraph.Impl
{
    public class GuidStringNode
    {


        public enum Structures
        {
            Scalar,
            Array,
            List,
            Dict,
        }

        public abstract class GenericKnobNode<T> : KnobNode<Guid, string>
        {
            private Structures structure = Structures.Scalar;
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
                        case Structures.Array: return list.Count;
                        case Structures.List: return list.Count;
                        case Structures.Dict: return dict.Count;
                    }
                    return 1;
                }
            }
            protected T AsScalar
            {
                get
                {
                    if (this.structure != Structures.Scalar)
                        throw new InvalidCastException($"{typeof(GenericKnobNode<T>)}[{this.id}] cannot be accessed as scalar.");
                    return scalar;
                }
                set { if (this.structure == Structures.Scalar) scalar = value; }
            }
            public T this[int index]
            {
                get
                {
                    if (this.structure != Structures.Array && this.structure != Structures.List)
                        throw new InvalidCastException($"{typeof(GenericKnobNode<T>)}[{this.id}] cannot be accessed by index.");
                    return list[index];
                }
                set => list[index] = value;
            }
            protected Dictionary<string, T> AsDict
            {
                get
                {
                    if (this.structure != Structures.Dict)
                        throw new InvalidCastException($"{typeof(GenericKnobNode<T>)}[{this.id}] cannot be accessed as dict.");
                    return dict;
                }
            }


            protected GenericKnobNode(Guid id, Structures structure = Structures.Scalar) : base(id)
            {
                this.structure = structure;
                this.buildFrom(structure);
            }
            protected GenericKnobNode(Guid id, Structures structure, int size) : base(id)
            {
                this.structure = structure;
                this.buildFrom(structure, size);
            }


            protected void buildFrom(Structures structure, int size = 1)
            {
                switch (structure)
                {
                    case Structures.Array:
                    case Structures.List:
                        list = new List<T>(Enumerable.Repeat<T>(default(T), size));
                        return;
                    case Structures.Dict:
                        dict = new Dictionary<string, T>(size);
                        return;
                    case Structures.Scalar:
                        scalar = default(T);
                        return;
                }

                throw new ArgumentException($"{structure} is not supported and the {nameof(GenericKnobNode<T>)} is not build correctly.");
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

            protected void Add(T value)
            {
                if (!isResizable)
                    throw new InvalidOperationException($"You cannot add items to a non resizable structure.");
                if (this.structure == Structures.List)
                    this.list.Add(value);
                throw new InvalidOperationException($"You can only add items to a {Structures.List} structure. You tried to add to {this.structure} structure instead.");
            }
            protected void Add(string knobName, T value)
            {
                if (!isResizable)
                    throw new InvalidOperationException($"You cannot add items to a non resizable structure.");
                if (this.structure == Structures.Dict)
                {
                    if (this.dict.ContainsKey(knobName))
                        this.dict[knobName] = value;
                    else
                        this.dict.Add(knobName, value);
                }
                throw new InvalidOperationException($"You can only add named items to a {Structures.Dict} structure. You tried to add to {this.structure} structure instead.");
            }
        }

    }
}

