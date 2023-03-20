﻿using NETGraph.Core.Meta;
using System;
using System.Text;

namespace NETGraph.Core.BuiltIn
{
    public class LibString : LibBase
    {
        public static LibString Instance { get; private set; } = new LibString();

        public enum DataTypes : int
        {
            // LibMath data types start at index 96
            StringBuilder = LibMath.DataTypes.Vector2f + 32
        }

        private LibString()
        {
            // register LibText types e.g. StringBuilder as additional types
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.StringBuilder, typeof(StringBuilder)));
        }

    }

}