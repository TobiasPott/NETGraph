using System;
using NETGraph.Core.Meta;
using System.Collections.Generic;
using System.Text;

namespace NETGraph.Core.BuiltIn
{

    // ToDo: implement a library base type to allow inheriting the registration progress with the meta type system
    //      base class should provide singleton instance
    //      base class should identify itself as registered or not
    //      base class should contain a collection if MetaTypeBlueprint which are processed by the base constructor
    //      base class constructor should register all blueprints of a library
    //      base class should hold a field for a namespace definition (which can be used for global access or unique identification)
    //      base class should hold a field for a library name (which can be used for global access or unique identification)


    public class LibString : LibBase
    {
        public static LibString Instance { get; private set; } = new LibString();

        public enum DataTypes : int
        {
            // LibMath data types start at index 64
            StringBuilder = LibMath.DataTypes.Vector2 + 32
        }


        private LibString()
        {
            // register LibMath types e.g. Vector3Data as additional types
            // 2 component vector type
            MetaTypeRegistry.RegisterDataType(new MetaTypeBlueprint((int)DataTypes.StringBuilder, typeof(System.Text.StringBuilder), StringBuilderData.Generator()));


            methods.Add("append", new Call(Append, null));
            methods.Add("appendLine", new Call(AppendLine, null));
        }

        private void Append(IDataResolver reference, IDataResolver assignTo, IEnumerable<IDataResolver> inputs)
        {
            StringBuilder sb = assignTo.resolve<StringBuilder>();
            foreach (DataResolver input in inputs)
                sb.Append(input.resolve<string>());
        }
        private void AppendLine(IDataResolver reference, IDataResolver assignTo, IEnumerable<IDataResolver> inputs)
        {
            StringBuilder sb = assignTo.resolve<StringBuilder>();
            foreach (DataResolver input in inputs)
                sb.AppendLine(input.resolve<string>());
        }


    }

}