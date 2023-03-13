using System;
using NETGraph.Core.Meta;
using System.Collections.Generic;

namespace NETGraph.Core.BuiltIn
{
    public class LibCore : LibBase
    {
        public static LibCore Instance { get; private set; } = new LibCore();


        private LibCore()
        {
            // ToDo: Implement new, initScalar, initList, initDict methods
            //      May update the generator cache, as this may make it obsolete to have specific scalar, list and dict methods in there
            //      as the lib implementation wraps the base data instance creation and the required init methods
            //      these can than be used with method definitions and calls to execute and
            //      
            //      This may also require to reintroduce additional 'invoke' signature to return new DataResolver as 'new' is impossible without
            //methods.Add("new", Add);
            //methods.Add("initScalar", Add);
            //methods.Add("initList", Add);
            //methods.Add("initDict", Add);
        }


        private void Add(string signature, IDataResolver assignTo, IEnumerable<IDataResolver> inputs)
        {
            //int sum = inputs.Sum(q => q.resolve<int>());
            //assignTo.assign<int>(sum);
        }

    }

}

