﻿using System;
using NETGraph.Core.Meta;
using System.Collections.Generic;
using System.Linq;

namespace NETGraph.Core.BuiltIn
{
    public class LibCore : LibBase
    {
        public static LibCore Instance { get; private set; } = new LibCore();


        public enum DataTypes : int
        {
            Any = -1,
            Void = 0,
            Object,
            Bool,
            Byte, SByte,
            Short, UShort,
            Int, UInt, Char,
            Long, ULong,
            Float, Double, Decimal,
            String,
            // internal library types (e.g. node, data, method
            IData,
        }


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
        }

        public static void assign(IResolver lh, IResolver rh) => lh.assign(rh);

    }

}

