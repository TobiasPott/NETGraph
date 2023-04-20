using System;
using NETGraph.Core.Meta;
using System.Linq;
using System.Collections.Generic;

namespace NETGraph.Core.BuiltIn.Methods
{

    using Type = System.Int32;
    using DataInterface = IData;
    public static class Console
    {
        public static MethodList Methods
        {
            get
            {
                MethodList methods = new MethodList("Console", null);
                methods.Set(new MethodHandle($"{nameof(WriteLine)}", MethodBindings.Static), WriteLine);
                methods.Set(new MethodHandle($"{nameof(Write)}", MethodBindings.Static), Write);
                methods.Set(new MethodHandle($"{nameof(Clear)}", MethodBindings.Static), Clear);
                return methods;
            }
        }

        private static IData Clear(IData reference, params IData[] args)
        {
            // resolve arguments to individual unnderlying types
            // -> method has no parameters
            // call method and optionally wrap result into ValueType<T>
            System.Console.Clear();
            return Memory.Void;
        }
        private static IData Write(IData reference, params IData[] args)
        {
            // resolve arguments to individual unnderlying types
            System.String value = args[0].resolve<System.String>();
            // call method and optionally wrap result into ValueType<T>
            System.Console.Write(value);
            return Memory.Void;
        }
        private static IData WriteLine(IData reference, params IData[] args)
        {
            // resolve arguments to individual unnderlying types
            System.String value = args[0].resolve<System.String>();
            // call method and optionally wrap result into ValueType<T>
            System.Console.WriteLine(value);
            return Memory.Void;
        }

    }

}

