using System;
using NETGraph.Core.Meta;
using System.Collections.Generic;
using System.Linq;

namespace NETGraph.Core.BuiltIn
{
    public class LibMath : LibBase
    {
        public static LibMath Instance { get; private set; } = new LibMath();

        public enum DataTypes : int
        {
            // LibMath data types start at index 64
            Vector2f = 64, // vector2 float
            Vector2d,
            Vector2b,
            Vector2i,
            Vector3f, // vector3 float
            Vector3d,
            Vector3b,
            Vector3i,
            Vector4f, // vector4 float
            Vector4d,
            Vector4b,
            Vector4i,
        }


        private LibMath()
        {
            // register LibMath types e.g. Vector3Data as additional types
            // 2 component vector type
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.Vector2f, typeof(Vector2f)));
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.Vector2d, typeof(Vector2d)));
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.Vector2b, typeof(Vector2b)));
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.Vector2i, typeof(Vector2i)));
            // 3 component vector type
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.Vector3f, typeof(Vector3f)));
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.Vector3d, typeof(Vector3d)));
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.Vector3b, typeof(Vector3b)));
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.Vector3i, typeof(Vector3i)));
            // 4 component vector typee
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.Vector4f, typeof(Vector4f)));
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.Vector4d, typeof(Vector4d)));
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.Vector4b, typeof(Vector4b)));
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.Vector4i, typeof(Vector4i)));


            methods.Add("add", new Call(Add, null));
            methods.Add("subtract", new Call(Subtract, null));
            methods.Add("multiply", new Call(Multiply, null));
            methods.Add("divide", new Call(Divide, null));
        }


        private void Add(IResolver reference, IResolver assignTo, IEnumerable<IResolver> inputs)
        {
            int sum = inputs.Sum(q => q.resolve<int>());
            // check if reference is given and add it's value to sum;
            if (reference != null)
                sum += reference.resolve<int>();
            assignTo.assign<int>(sum);
        }
        private void Subtract(IResolver reference, IResolver assignTo, IEnumerable<IResolver> inputs)
        {
            int subtrahends = inputs.Sum(q => q.resolve<int>());
            assignTo.assign<int>(assignTo.resolve<int>() - subtrahends);
        }
        private void Multiply(IResolver reference, IResolver assignTo, IEnumerable<IResolver> inputs)
        {
            int product = 1;
            foreach (DataResolver input in inputs)
                product *= input.resolve<int>();
            assignTo.assign<int>(product);
        }
        private void Divide(IResolver reference, IResolver assignTo, IEnumerable<IResolver> inputs)
        {
            int dividend = inputs.First().resolve<int>();
            foreach (DataResolver input in inputs.Skip(1))
                dividend /= input.resolve<int>();
            assignTo.assign<int>(dividend);
        }

    }

}

