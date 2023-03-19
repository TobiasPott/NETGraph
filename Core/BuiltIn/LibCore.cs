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
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Any, typeof(Any)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Void, typeof(void)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Object, typeof(object)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Bool, typeof(bool)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Byte, typeof(byte)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.SByte, typeof(sbyte)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Short, typeof(short)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.UShort, typeof(ushort)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Char, typeof(char)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Int, typeof(int)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.UInt, typeof(uint)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Long, typeof(long)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.ULong, typeof(ulong)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Float, typeof(float)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Double, typeof(double)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Decimal, typeof(decimal)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.String, typeof(string)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.IData, typeof(IData)));

            // ToDo: Implement new, initScalar, initList, initDict methods
            //      May update the generator cache, as this may make it obsolete to have specific scalar, list and dict methods in there
            //      as the lib implementation wraps the base data instance creation and the required init methods
            //      these can than be used with method definitions and calls to execute and
            //      
            //      This may also require to reintroduce additional 'invoke' signature to return new DataResolver as 'new' is impossible without
        }

        // ToDo: Add implementation for init methods of data
        //      Add set value methods of data
        //      Add modifying dict and list of data

        //      Add method to store data on a stack (making it a named variable which can be adressed)
    }

}

