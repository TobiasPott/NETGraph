using System;

namespace NETGraph.Core.Meta
{

    public struct MethodResolver
    {
        IMethodProvider target;
        MethodSignature signature;
        DataResolver result;
        DataResolver[] inputs;

        public MethodResolver(IMethodProvider target, MethodSignature signature, DataResolver result, params DataResolver[] inputs)
        {
            this.target = target;
            this.signature = signature;
            this.result = result;
            this.inputs = inputs;
        }
        public MethodResolver(IMethodProvider target, string signature, DataResolver result, params DataResolver[] inputs) : this(target, new MethodSignature(signature), result, inputs)
        { }

        public bool evaluate() => target.Invoke(signature, result, inputs);
    }

}

