using System;
using NETGraph.Core.Meta;
using System.Collections.Generic;

namespace NETGraph.Core.BuiltIn
{
    public abstract class LibBase : IMethodProvider
    {
        protected Dictionary<string, Call> methods = new Dictionary<string, Call>();


        public void assign(string signature, IDataResolver reference, IDataResolver assignTo, IEnumerable<IDataResolver> inputs)
        {
            Call call = methods[signature];
            if (call.assign != null)
                call.assign.Invoke(reference, assignTo, inputs);
            else
                throw new InvalidOperationException($"Assignment call for {signature} is not supported.");
        }

        public IDataResolver invoke(string signature, IDataResolver reference, IEnumerable<IDataResolver> inputs)
        {
            Call call = methods[signature];
            if (call.invoke != null)
                return call.invoke.Invoke(reference, inputs);
            else
                throw new InvalidOperationException($"Method call for {signature} is not supported.");
        }
    }

}

