using System;
using NETGraph.Core.Meta;
using System.Collections.Generic;
using System.Linq;

namespace NETGraph.Core.BuiltIn
{


    // ToDo: implement a library base type to allow inheriting the registration progress with the meta type system
    //      base class should provide singleton instance
    //      base class should identify itself as registered or not
    //      base class should contain a collection if MetaTypeBlueprint which are processed by the base constructor
    //      base class constructor should register all blueprints of a library
    //      base class should hold a field for a namespace definition (which can be used for global access or unique identification)
    //      base class should hold a field for a library name (which can be used for global access or unique identification)
    public class LibMath : IMethodProvider
    {
        public static LibMath Instance { get; private set; } = new LibMath();

        public enum DataTypes : int
        {
            // LibMath data types start at index 64
            Vector2 = 64,
            Vector2Double,
            Vector2Byte,
            Vector2Int,
            Vector3,
            Vector3Double,
            Vector3Byte,
            Vector3Int,
            Vector4,
            Vector4Double,
            Vector4Byte,
            Vector4Int,
        }


        private LibMath()
        {
            // register LibMath types e.g. Vector3Data as additional types
            // 2 component vector type
            MetaTypeRegistry.RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Vector2, typeof(Vector2Data), Vector2Data.Generator()));
            MetaTypeRegistry.RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Vector2Double, typeof(Vector2DoubleData), Vector2DoubleData.Generator()));
            MetaTypeRegistry.RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Vector2Byte, typeof(Vector2ByteData), Vector2ByteData.Generator()));
            MetaTypeRegistry.RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Vector2Int, typeof(Vector2IntData), Vector2IntData.Generator()));
            // 3 component vector type
            MetaTypeRegistry.RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Vector3, typeof(Vector3Data), Vector3Data.Generator()));
            MetaTypeRegistry.RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Vector3Double, typeof(Vector3DoubleData), Vector3DoubleData.Generator()));
            MetaTypeRegistry.RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Vector3Byte, typeof(Vector3ByteData), Vector3ByteData.Generator()));
            MetaTypeRegistry.RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Vector3Int, typeof(Vector3IntData), Vector3IntData.Generator()));
            // 4 component vector typee
            MetaTypeRegistry.RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Vector4, typeof(Vector4Data), Vector4Data.Generator()));
            MetaTypeRegistry.RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Vector4Double, typeof(Vector4DoubleData), Vector4DoubleData.Generator()));
            MetaTypeRegistry.RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Vector4Byte, typeof(Vector4ByteData), Vector4ByteData.Generator()));
            MetaTypeRegistry.RegisterDataType(new MetaTypeBlueprint((int)DataTypes.Vector4Int, typeof(Vector4IntData), Vector4IntData.Generator()));
        }


        public static MethodSignature add = new MethodSignature("any add(any, any)");
        public static MethodSignature subtract = new MethodSignature("any subtract(any, any)");
        public static MethodSignature multiply = new MethodSignature("any multiply(any, any)");
        public static MethodSignature divide = new MethodSignature("any divide(any, any)");



        public bool invoke(MethodSignature signature, DataResolver result, params DataResolver[] inputs) => invoke(signature, result, inputs as IEnumerable<DataResolver>);
        public bool invoke(MethodSignature signature, DataResolver result, IEnumerable<DataResolver> inputs)
        {
            if (signature.method.Equals("add"))
            {
                Add(signature, result, inputs);
                return true;
            }
            else if (signature.method.Equals("subtract"))
            {
                Subtract(signature, result, inputs);
                return true;
            }
            else if (signature.method.Equals("multiply"))
            {
                Multiply(signature, result, inputs);
                return true;
            }
            else if (signature.method.Equals("divide"))
            {
                Divide(signature, result, inputs);
                return true;
            }
            return false;
        }

        private void Add(MethodSignature signature, DataResolver result, IEnumerable<DataResolver> inputs)
        {
            int sum = inputs.Sum(q => q.resolve<int>());
            result.assign<int>(sum);
        }
        private void Subtract(MethodSignature signature, DataResolver result, IEnumerable<DataResolver> inputs)
        {
            int subtrahends = inputs.Sum(q => q.resolve<int>());
            result.assign<int>(result.resolve<int>() - subtrahends);
        }
        private void Multiply(MethodSignature signature, DataResolver result, IEnumerable<DataResolver> inputs)
        {
            int product = 1;
            foreach (DataResolver input in inputs)
                product *= input.resolve<int>();
            result.assign<int>(product);
        }
        private void Divide(MethodSignature signature, DataResolver result, IEnumerable<DataResolver> inputs)
        {
            int dividend = inputs.First().resolve<int>();
            foreach (DataResolver input in inputs.Skip(1))
                dividend /= input.resolve<int>();
            result.assign<int>(dividend);
        }

    }

}

