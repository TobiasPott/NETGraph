using System;
using NETGraph.Core.Meta;
using System.Collections.Generic;
using System.Text;

namespace NETGraph.Core.BuiltIn
{
    // ToDo: implement a library base type to allow inheriting the registration progress with the meta type system
    //      base class should provide singleton instance
    //      base class should identify itself as registered or not
    //      base class should contain a collection if MetaTypeBlueprint which are processed by the base constructor
    //      base class constructor should register all blueprints of a library
    //      base class should hold a field for a namespace definition (which can be used for global access or unique identification)
    //      base class should hold a field for a library name (which can be used for global access or unique identification)
    public class LibString : IMethodProvider
    {
        public static LibString Instance { get; private set; } = new LibString();

        public enum DataTypes : int
        {
            // LibMath data types start at index 64
            StringBuilder = LibMath.DataTypes.Vector2 + 32
        }


        private LibString()
        {
            // register LibMath types e.g. Vector3Data as additional types
            // 2 component vector type
            MetaTypeRegistry.RegisterDataType(new MetaTypeBlueprint((int)DataTypes.StringBuilder, typeof(System.Text.StringBuilder), StringBuilderData.Generator()));
        }


        public static MethodSignature add = new MethodSignature("void StringBuilder::append(any, any)");



        public bool invoke(MethodSignature signature, DataResolver result, params DataResolver[] inputs) => invoke(signature, result, inputs as IEnumerable<DataResolver>);
        public bool invoke(MethodSignature signature, DataResolver result, IEnumerable<DataResolver> inputs)
        {
            if (signature.method.Equals("append"))
            {
                Append(signature, result, inputs);
                return true;
            }
            return false;
        }

        private void Append(MethodSignature signature, DataResolver result, IEnumerable<DataResolver> inputs)
        {
            //int sum = inputs.Sum(q => q.resolve<int>());
            //result.assign<int>(sum);
        }

    }

}