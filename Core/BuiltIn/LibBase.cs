using System;
using NETGraph.Core.Meta;
using System.Collections.Generic;

namespace NETGraph.Core.BuiltIn
{
    // ToDo: implement a library base type to allow inheriting the registration progress with the meta type system
    //      base class should provide singleton instance
    //      base class should identify itself as registered or not
    //      base class should contain a collection if MetaTypeBlueprint which are processed by the base constructor
    //      base class constructor should register all blueprints of a library
    //      base class should hold a field for a namespace definition (which can be used for global access or unique identification)
    //      base class should hold a field for a library name (which can be used for global access or unique identification)
    public abstract class LibBase : IMethodProvider
    {
        protected Dictionary<string, Call> methods = new Dictionary<string, Call>();


        public void assign(string accessor, IResolver reference, IResolver assignTo, params IResolver[] inputs)
        {
            Call call = methods[accessor];
            if (call.assign != null)
                call.assign.Invoke(reference, assignTo, inputs);
            else
                throw new InvalidOperationException($"Assignment call for {accessor} is not supported.");
        }

        public IResolver invoke(string accessor, IResolver reference, params IResolver[] inputs)
        {
            Call call = methods[accessor];
            if (call.invoke != null)
                return call.invoke.Invoke(reference, inputs);
            else
                throw new InvalidOperationException($"Method call for {accessor} is not supported.");
        }
    }

}

