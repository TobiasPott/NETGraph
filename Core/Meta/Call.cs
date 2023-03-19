using System;
using System.Collections.Generic;

namespace NETGraph.Core.Meta
{
    // ToDo: Extract assignment call component to global method which is performed with any result of an invokation
    //      assignment takes a IResolver, resolves and assigns the data to any given other IResolver

    public struct Call
    {
        public Assignment assign { get; private set; }
        public Invokation invoke { get; private set; }

        public Call(Assignment assign, Invokation invoke)
        {
            this.assign = assign;
            this.invoke = invoke;
        }
    }

    public delegate IResolver Invokation(IResolver reference, params IResolver[] inputs);
    public delegate void Assignment(IResolver reference, IResolver assignTo, params IResolver[] inputs);

}

