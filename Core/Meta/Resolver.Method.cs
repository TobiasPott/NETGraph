using System;
using NETGraph.Data;

namespace NETGraph.Core.Meta
{

    //ToDo: implement a IDataProvider which is based around MethodResolver
    //      this should resolve to T as expected but holds a MethodResolver (and possibly temporary DataResolver) to execute a method
    //  ToDo:   May require implementation of ConstantDataResolver type

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

