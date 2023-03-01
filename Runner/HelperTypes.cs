using System;
using System.Collections.Generic;
using System.Linq;
using NETGraph.Core;

namespace NETGraph.Runner
{

    public class AddDataProvider : IDataProvider
    {
        Dictionary<string, Data> datas;

        public AddDataProvider(int lh, int rh)
        {
            datas = new Dictionary<string, Data>()
            {
                { "lh", new IntData(lh) },
                { "rh", new IntData(rh) },
                { "sum", new IntData(0) },
            };
        }

        public Data Access(DataAccessor accessor)
        {
            // ToDo: a base implementation for the 'Access' method may include type check by index and other validations to avoid runtime errors
            return datas[accessor.dataName];
        }

        public override string ToString()
        {
            return $"{((IntData)datas["lh"]).GetScalar()} + {((IntData)datas["rh"]).GetScalar()} = {((IntData)datas["sum"]).GetScalar()}";
        }

    }



    public class MathProvider : IMethodProvider
    {
        public static MathProvider Instance { get; private set; } = new MathProvider();


        public bool Invoke(MethodAccessor accessor, Data result, params Data[] inputs)
        {
            return Invoke(accessor, result, inputs as IEnumerable<Data>);
        }

        public bool Invoke(MethodAccessor accessor, Data result, IEnumerable<Data> inputs)
        {
            if (accessor.method.Equals("add"))
            {
                Add(result, inputs);
                return true;
            }
            return false;
        }

        // ToDo: add a unpacker to try to recieve Data and Data<T> objects
        //      The unpacker recieves DataAcessors and IDataProvider (paired in tuples or new type)
        //      The unpacker returns a result Data object from provider using the accessor (use IDataProvider.Access)
        //      The unpacker also returns an array of Data objects passed to actual methods

        private void Add(Data rawResult, IEnumerable<Data> inputs)
        {
            Data<int> result = rawResult as Data<int>;
            int sum = inputs.Where(x => x.TypeIndex == TypeIndices.Int).Sum(x => (x as Data<int>).GetScalar());
            result.SetScalar(sum);
        }

    }


}

