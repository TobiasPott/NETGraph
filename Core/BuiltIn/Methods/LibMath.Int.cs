using System;
using NETGraph.Core.Meta;
using System.Linq;

namespace NETGraph.Core.BuiltIn.Methods
{

    using Type = System.Int32;
    using DataInterface = IData;
    public static class Int
    {
        public static MethodList Methods
        {
            get
            {
                MethodList methods = new MethodList(nameof(Int32), null);
                methods.Set(new MethodHandle($"{nameof(Add)}", MethodBindings.Instance), Add);
                methods.Set(new MethodHandle($"{nameof(Subtract)}", MethodBindings.Instance), Subtract);
                methods.Set(new MethodHandle($"{nameof(Multiply)}", MethodBindings.Instance), Multiply);
                methods.Set(new MethodHandle($"{nameof(Divide)}", MethodBindings.Instance), Divide);
                return methods;
            }
        }

        public static DataInterface Add(DataInterface reference, params DataInterface[] args)
        {
            // check if reference is given and add it's value to sum;
            if (reference != null)
            {
                Type sum = args.Sum(q => q.resolve<Type>());
                reference.assign(sum + reference.resolve<Type>());
                return reference;
            }
            throw new ArgumentNullException(nameof(reference));
        }
        public static DataInterface Subtract(DataInterface reference, params DataInterface[] args)
        {
            // check if reference is given and add it's value to sum;
            if (reference != null)
            {
                Type sum = args.Sum(q => q.resolve<Type>());
                reference.assign(reference.resolve<Type>() - sum);
                return reference;
            }
            throw new ArgumentNullException(nameof(reference));
        }
        public static DataInterface Multiply(DataInterface reference, params DataInterface[] args)
        {
            // check if reference is given and add it's value to sum;
            if (reference != null)
            {
                Type product = reference.resolve<int>();
                foreach (DataInterface arg in args)
                    product *= arg.resolve<Type>();
                reference.assign(product);
                return reference;
            }
            throw new ArgumentNullException(nameof(reference));
        }
        public static DataInterface Divide(DataInterface reference, params DataInterface[] args)
        {
            // check if reference is given and add it's value to sum;
            if (reference != null)
            {
                Type result = reference.resolve<int>();
                foreach (DataInterface arg in args)
                    result /= arg.resolve<Type>();
                reference.assign(result);
                return reference;
            }
            throw new ArgumentNullException(nameof(reference));
        }

    }

}

