using System;
using NETGraph.Core.Meta;
using System.Collections.Generic;
using System.Linq;

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
            methods.Add("new", new Call(null, New));

        }

        // ToDo: Add processing of third parameter for naming a variable on creation
        private IResolver New(IResolver reference, IEnumerable<IResolver> inputs)
        {
            int typeIndex = inputs.First().resolve<int>();
            string name = inputs.Skip(1).First().resolve<string>();

            Console.WriteLine($"new {(DataTypes)typeIndex} named {name}");
            return new DataResolver(MetaTypeRegistry.Generator(typeIndex).Generate(IData.Options.Scalar), DataAccessor.Scalar);
        }

        // ToDo: Add implementation for init methods of data
        //      Add set value methods of data
        //      Add modifying dict and list of data

        //      Add method to store data on a stack (making it a named variable which can be adressed)
    }

}

