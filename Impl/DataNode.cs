using System;
using System.Collections.Generic;
using System.Linq;
using NETGraph.Core;

namespace NETGraph.Impl.Generics
{
    // ToDo: Add node description to describe knob mapping and behaviour (optional!)
    // ToDo: Wrap/Encapsulate data structure into own type and build node content based of definitions
    public abstract class DataNode<T> : KnobNode<Guid, string>
    {


        protected DataNode(NodeDefinition definition, Guid id) : base(id)
        {
        }
    }

}

