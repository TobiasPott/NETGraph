using System.Collections.Generic;

namespace NETGraph.Core.Meta
{
    public interface IMethodProvider
    {
        // reference can be null for static calls, assignTo can be null, inputs can be null/empty
        //void assign(string accessor, IResolver reference, IResolver assignTo, params IResolver[] inputs);
        // reference can be null for static calls, result cannot be null, inputs can be null/empty
        IResolver invoke(string accessor, IResolver reference, params IResolver[] args);
    }

}

