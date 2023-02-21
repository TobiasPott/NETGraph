using System;
using System.Collections.Generic;

namespace NETGraph.Core
{

    public abstract class Node<Id> : IEquatable<Node<Id>>, IComparable<Node<Id>> where Id : IComparable<Id>
    {
        protected Id id;

        public Node(Id id)
        {
            this.id = id;
        }

        bool IEquatable<Node<Id>>.Equals(Node<Id> other)
        {
            if (other != null)
                return other.id.Equals(this.id);
            return false;
        }

        public override string ToString()
        {
            return $"{this.GetType()} ({id})";
        }

        public int CompareTo(Node<Id> other)
        {
            if (other == null)
                return 1;
            return this.id.CompareTo(other.id);
        }
    }

    public abstract class KnobNode<Id, KnobType> : Node<Id> where Id : IComparable<Id> where KnobType : IComparable<KnobType>
    {
        public KnobNode(Id id) : base(id)
        { }

        protected abstract bool hasKnob(KnobType knob);
        protected abstract Type knobValueType(KnobType knob);


        public static bool canConnect(KnobNode<Id, KnobType> from, KnobType fromKnob, KnobNode<Id, KnobType> to, KnobType toKnob)
        {
            if (from == null || to == null)
                return false;

            if(from.hasKnob(fromKnob)
                && to.hasKnob(toKnob))
            {
                if (from.knobValueType(fromKnob).Equals(to.knobValueType(toKnob)))
                    return true;
            }
            return false;
        }
    }

}

