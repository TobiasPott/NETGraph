using System;
using System.Collections.Generic;
using System.Linq;
using NETGraph.Core;
using NETGraph.Core.Meta;
using NETGraph.Data;

namespace NETGraph.Runner
{

    public class MathOpDataProvider : DataBase<int>
    {
        private static string[] keys = new[] { "lh", "rh", "sum" };

        public MathOpDataProvider(int lh, int rh) : base(DataTypes.Int, DataStructure.Named, false)
        {
            initNamed(keys.Select(k => new KeyValuePair<string, int>(k, 0)));
            this.setAt(keys[0], lh);
            this.setAt(keys[1], rh);
        }

        public override string ToString()
        {
            return $"{this["lh"]} + {this["rh"]} = {this["sum"]}";
        }

    }

    // regex: 
    //      (?:(?:\b[a-zA-Z]{1}(?:[\w.\[\]]+))+,{0})    // final regex to separate everything into

    public class MathProvider : IMethodProvider
    {
        public static MathProvider Instance { get; private set; } = new MathProvider();

        public bool Invoke(MethodSignature signature, DataResolver result, params DataResolver[] inputs) => Invoke(signature, result, inputs as IEnumerable<DataResolver>);
        public bool Invoke(MethodSignature signature, DataResolver result, IEnumerable<DataResolver> inputs)
        {
            if (signature.method.Equals("add"))
            {
                Add(signature, result, inputs);
                return true;
            }
            else if (signature.method.Equals("subtract"))
            {
                Subtract(signature, result, inputs);
                Console.WriteLine("Subtract");
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
            int subtrahends = inputs.Skip(1).Sum(q => q.resolve<int>());
            result.assign<int>(result.resolve<int>() - subtrahends);
        }


    }


}

