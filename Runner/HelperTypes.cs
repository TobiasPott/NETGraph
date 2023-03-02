using System;
using System.Collections.Generic;
using System.Linq;
using NETGraph.Core;
using NETGraph.Data;
using NETGraph.Data.Simple;

namespace NETGraph.Runner
{

    public class AddDataProvider : IDataProvider
    {
        Dictionary<string, DataBase> datas;

        public AddDataProvider(int lh, int rh)
        {
            GeneratorDefinition intGen = IntData.Generator;
            
            datas = new Dictionary<string, DataBase>()
            {
                { "lh", IntData.Generator.Scalar(lh) },
                { "rh", IntData.Generator.Scalar(rh) },
                { "sum", IntData.Generator.Scalar(0) },
            };
        }

        public DataBase Access(DataAccessor accessor)
        {
            // ToDo: a base implementation for the 'Access' method may include type check by index and other validations to avoid runtime errors
            return datas[accessor.dataName];
        }

        public override string ToString()
        {
            return $"{((IntData)datas["lh"]).GetScalar()} + {((IntData)datas["rh"]).GetScalar()} = {((IntData)datas["sum"]).GetScalar()}";
        }

    }

    // regex: ^(?:(.*)?[$(]+)               // match return and name part (no ())
    //          ([$(]{1}(?<args>.*)?[$)]{1})        // match argument list (no ())
    //          (?:[$(]{1}(?<args>(((?:[\w.]+)+),?).*)[$)]{1})      // match down to first argument name (not repeated)

    //      (?:[$(]{1}(?<args>(?:[\w.]+).*)( >> insert , split here << )[$)]{1}) // outer

    //      (?:(?:\b[a-zA-Z]{1}(?:[\w.\[\]]+))+,{0})    // final regex to separate everything into

    public class MathProvider : IMethodProvider
    {
        public static MathProvider Instance { get; private set; } = new MathProvider();


        public bool Invoke(MethodAccessor accessor, DataBase result, params DataBase[] inputs)
        {
            return Invoke(accessor, result, inputs as IEnumerable<DataBase>);
        }

        public bool Invoke(MethodAccessor accessor, DataBase result, IEnumerable<DataBase> inputs)
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

        private void Add(DataBase rawResult, IEnumerable<DataBase> inputs)
        {
            DataBase<int> result = rawResult as DataBase<int>;
            int sum = inputs.Where(x => x.TypeIndex == (int)DataTypes.Int).Sum(x => (x as DataBase<int>).GetScalar());
            result.SetScalar(sum);
        }

    }


}

