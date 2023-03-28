using NETGraph.Core.Meta;
using System;
using System.Text;

namespace NETGraph.Core.BuiltIn
{
    public class LibString : LibBase
    {
        public enum DataTypes : int
        {
            // LibMath data types start at index 96
            StringBuilder = LibMath.DataTypes.Vector2f + 32
        }

        public LibString(): base(nameof(LibString), "System")
        {
            // register LibText types e.g. StringBuilder as additional types
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.StringBuilder, typeof(StringBuilder)));
        }

        protected override bool LoadInternal()
        {
            return true;
        }

    }

}