using System;
using NETGraph.Core.Meta;
using System.Linq;

namespace NETGraph.Core.BuiltIn.Methods
{

    using Type = System.Int32;
    public static class Int
    {
        public static IResolver Add(IResolver reference, params IResolver[] args)
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
        public static IResolver Subtract(IResolver reference, params IResolver[] args)
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
        public static IResolver Multiply(IResolver reference, params IResolver[] args)
        {
            // check if reference is given and add it's value to sum;
            if (reference != null)
            {
                Type product = reference.resolve<int>();
                foreach (IResolver arg in args)
                    product *= arg.resolve<Type>();
                reference.assign(product);
                return reference;
            }
            throw new ArgumentNullException(nameof(reference));
        }
        public static IResolver Divide(IResolver reference, params IResolver[] args)
        {
            // check if reference is given and add it's value to sum;
            if (reference != null)
            {
                Type result = reference.resolve<int>();
                foreach (IResolver arg in args)
                    result /= arg.resolve<Type>();
                reference.assign(result);
                return reference;
            }
            throw new ArgumentNullException(nameof(reference));
        }

    }

}

