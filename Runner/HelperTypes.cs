using System;
using System.Collections.Generic;
using System.Linq;
using NETGraph.Core;
using NETGraph.Core.Meta;

namespace NETGraph.Runner
{

    public class MathOpDataProvider : Data<int>
    {
        private static string[] keys = new[] { "lh", "rh", "sum" };

        public MathOpDataProvider(int lh, int rh) : base(DataTypes.Int, DataOptions.Named)
        {
            initNamed(keys.Select(k => new KeyValuePair<string, int>(k, 0)));
            this.assign(keys[0], lh);
            this.assign(keys[1], rh);
        }

        public override string ToString()
        {
            return $"{this["lh"]} + {this["rh"]} = {this["sum"]}";
        }

    }

    // regex: 
    //      (?:(?:[a-zA-Z]{1}(?:[\w.\[\]]+))+),{0}      // split by , (for method signature)
    //      (?:(?:[a-zA-Z]{1}(?:[\w\[\]]?))+).{0}       // split by . (for data path)

    public class MathProvider : IMethodProvider
    {
        public static MathProvider Instance { get; private set; } = new MathProvider();

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

