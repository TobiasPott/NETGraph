using System;
using NETGraph.Core.Meta;
using System.Collections.Generic;
using System.Linq;
using NETGraph.Core.BuiltIn.Methods;

namespace NETGraph.Core.BuiltIn
{
    public partial class LibMath : LibBase
    {
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


        public LibMath() : base(nameof(LibMath), "System")
        {
            // register LibMath types e.g. Vector3Data as additional types
            // 2 component vector type
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Vector2f, typeof(Vector2f)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Vector2d, typeof(Vector2d)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Vector2b, typeof(Vector2b)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Vector2i, typeof(Vector2i)));
            // 3 component vector type
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Vector3f, typeof(Vector3f)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Vector3d, typeof(Vector3d)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Vector3b, typeof(Vector3b)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Vector3i, typeof(Vector3i)));
            // 4 component vector typee
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Vector4f, typeof(Vector4f)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Vector4d, typeof(Vector4d)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Vector4b, typeof(Vector4b)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Vector4i, typeof(Vector4i)));

            // ToDo: Implement global lookup function to lookup method by name and context
            //      context inludes IData reference or string name
            // add methods to method list
            MethodList intMethods = new MethodList("int");
            intMethods.Set($"{nameof(Int32)}.{nameof(Int.Add)}", Int.Add);
            intMethods.Set($"{nameof(Int32)}.{nameof(Int.Subtract)}", Int.Subtract);
            intMethods.Set($"{nameof(Int32)}.{nameof(Int.Multiply)}", Int.Multiply);
            intMethods.Set($"{nameof(Int32)}.{nameof(Int.Divide)}", Int.Divide);

            this.methods = new MethodList(nameof(LibMath), intMethods);
            this.methods.Set(nameof(Add), Add);
        }

        protected override bool LoadInternal()
        {
            // ToDo: ponder about doing self-check on registered libraries to avoid re-registering stuff
            return true;
        }
        private static IResolver Add(IResolver reference, params IResolver[] args)
        {
            int sum = args.Sum(q => q.resolve<int>());
            // check if reference is given and add it's value to sum;
            if (reference != null)
            {
                reference.assign(sum + reference.resolve<int>());
                return reference;
            }
            else
            {
                IData result = new ValueData<int>(sum);
                return result;
            }
        }
        //private IResolver Subtract(IResolver reference, IEnumerable<IResolver> inputs)
        //{
        //    int subtrahends = inputs.Sum(q => q.resolve<int>());
        //    assignTo.assign<int>(assignTo.resolve<int>() - subtrahends);
        //}
        //private IResolver Multiply(IResolver reference, IEnumerable<IResolver> inputs)
        //{
        //    int product = 1;
        //    foreach (IResolver input in inputs)
        //        product *= input.resolve<int>();
        //    assignTo.assign<int>(product);
        //}
        //private IResolver Divide(IResolver reference, IEnumerable<IResolver> inputs)
        //{
        //    int dividend = inputs.First().resolve<int>();
        //    foreach (IResolver input in inputs.Skip(1))
        //        dividend /= input.resolve<int>();
        //    assignTo.assign<int>(dividend);
        //}

    }

}

