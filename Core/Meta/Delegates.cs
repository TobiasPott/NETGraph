using System;
using System.Collections.Generic;

namespace NETGraph.Core.Meta
{
    public delegate IData MethodRef(IData reference, params IData[] inputs);

    public static class MethodRefExtensions
    {
        public static IData Invoke(this MethodRef method)
        {
            return method?.Invoke(null, null);
        }
    }
}

