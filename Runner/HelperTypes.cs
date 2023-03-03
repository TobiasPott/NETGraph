using System;
using System.Collections.Generic;
using System.Linq;
using NETGraph.Core;
using NETGraph.Data;
using NETGraph.Data.Simple;

namespace NETGraph.Runner
{

    public class MathOpDataProvider : DataBase<int>
    {
        private static string[] keys = new[] { "lh", "rh", "sum" };

        public MathOpDataProvider(int lh, int rh) : base(DataTypes.Int, DataStructures.Named, false)
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

        public bool Invoke(MethodAccessor accessor, DataQuery result, params DataQuery[] inputs) => Invoke(accessor, result, inputs as IEnumerable<DataQuery>);
        public bool Invoke(MethodAccessor accessor, DataQuery result, IEnumerable<DataQuery> inputs)
        {
            if (accessor.method.Equals("add"))
            {
                Add(accessor, result, inputs);
                return true;
            }
            else if (accessor.method.Equals("subtract"))
            {
                Subtract(accessor, result, inputs);
                Console.WriteLine("Subtract");
                return true;
            }
            return false;
        }

        private void Add(MethodAccessor accessor, DataQuery result, IEnumerable<DataQuery> inputs)
        {
            int sum = inputs.Sum(q => q.resolve<int>());
            result.assign<int>(sum);
        }
        private void Subtract(MethodAccessor accessor, DataQuery result, IEnumerable<DataQuery> inputs)
        {
            int subtrahends = inputs.Skip(1).Sum(q => q.resolve<int>());
            result.assign<int>(result.resolve<int>() - subtrahends);
        }


    }


}

