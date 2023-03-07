using System;
using NETGraph.Core.Meta;
using System.Collections.Generic;
using System.Linq;

namespace NETGraph.Core.BuiltIn
{


    public class LibMath : IMethodProvider
    {
        public static LibMath Instance { get; private set; } = new LibMath();

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

