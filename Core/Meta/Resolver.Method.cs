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

    public delegate IDataResolver Invokation(IDataResolver reference, IEnumerable<IDataResolver> inputs);
    public delegate void Assignment(IDataResolver reference, IDataResolver assignTo, IEnumerable<IDataResolver> inputs);

    public interface IMethodProvider
    {
        // delegate void Procedure(IDataResolver reference, IEnumerable<IDataResolver> inputs)
        // delegate void StaticProcedure(IEnumerable<IDataResolver> inputs)

        // delegate IDataResolver Method(IDataResolver reference, IDataResolver assignTo, IEnumerable<IDataResolver> inputs)
        // delegate IDataResolver StaticMethod(IDataResolver assignTo, IEnumerable<IDataResolver> inputs)

        // reference can be null for static calls, assignTo can be null, inputs can be null/empty
        void assign(string signature, IDataResolver reference, IDataResolver assignTo, IEnumerable<IDataResolver> inputs);
        // reference can be null for static calls, result cannot be null, inputs can be null/empty
        IDataResolver invoke(string signature, IDataResolver reference, IEnumerable<IDataResolver> inputs);
    }

}

