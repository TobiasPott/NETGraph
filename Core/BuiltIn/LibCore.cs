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
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.Any, typeof(Any)));
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.Void, typeof(void)));
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.Object, typeof(object)));
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.Bool, typeof(bool)));
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.Byte, typeof(byte)));
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.SByte, typeof(sbyte)));
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.Short, typeof(short)));
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.UShort, typeof(ushort)));
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.Char, typeof(char)));
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.Int, typeof(int)));
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.UInt, typeof(uint)));
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.Long, typeof(long)));
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.ULong, typeof(ulong)));
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.Float, typeof(float)));
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.Double, typeof(double)));
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.Decimal, typeof(decimal)));
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.String, typeof(string)));
            MetaTypeRegistry.Register(new MetaTypeBlueprint((int)DataTypes.IData, typeof(IData)));

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
            return new DataResolver(DataGenerator.Generate(MetaTypeRegistry.GetType(typeIndex), IData.Options.Scalar), DataAccessor.Scalar);
        }

        // ToDo: Add implementation for init methods of data
        //      Add set value methods of data
        //      Add modifying dict and list of data

        //      Add method to store data on a stack (making it a named variable which can be adressed)
    }

}

