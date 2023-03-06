using System;
using System.Collections.Generic;

namespace NETGraph.Core.Meta
{

    //ToDo: implement a IDataProvider which is based around MethodResolver
    //      this should resolve to T as expected but holds a MethodResolver (and possibly temporary DataResolver) to execute a method
    //  ToDo:   May require implementation of ConstantDataResolver type


    public interface IMethodProvider
    {
        bool invoke(MethodSignature signature, DataResolver result, params DataResolver[] inputs);
        bool invoke(MethodSignature signature, DataResolver result, IEnumerable<DataResolver> inputs);
    }

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
        // ToDo: change signature to evaluate(DataResolver resolut) and add evaluate with DataResolver return type
        //      this will allow to detach the method resolver from a specific result (may be able to combine both methods in some way)
        //      
        public bool resolve() => target.invoke(signature, result, inputs);

    }

}

