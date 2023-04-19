using System;
using System.Collections.Generic;

namespace NETGraph.Core.Meta
{
    public delegate IData MethodRef(IData reference, params IData[] inputs);

    public static class MethodRefExtensions
    {

        public static MethodRef Declare(int typeIndex, string name, Options options)
        {
            MethodRef method = new MethodRef((reference, args) => { return Memory.Declare(null, typeIndex.AsValueData(), name.AsValueData(), options.AsValueData()); });
            return method;
        }
        public static MethodRef Assign(string name)
        {
            MethodRef method = new MethodRef((reference, args) => { Memory.Declare(name, reference, true); return reference; });
            return method;
        }
        public static MethodRef Get(string name)
        {
            MethodRef method = new MethodRef((reference, args) => { return Memory.Get(name); });
            return method;
        }

        public static IData Invoke(this MethodRef method)
        {
            return method?.Invoke(null, null);
        }
    }
}

