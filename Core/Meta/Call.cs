using System;
using System.Collections.Generic;

namespace NETGraph.Core.Meta
{

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

    public delegate IResolver Invokation(IResolver reference, IEnumerable<IResolver> inputs);
    public delegate void Assignment(IResolver reference, IResolver assignTo, IEnumerable<IResolver> inputs);

}

