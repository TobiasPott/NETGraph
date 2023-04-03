using System;
using NETGraph.Core.Meta;
using System.Collections.Generic;
using System.Linq;
using NETGraph.Core.BuiltIn.Methods;

namespace NETGraph.Core.BuiltIn
{
    public partial class LibMath : LibBase
    {
        public enum DataTypes : int
        {
            // LibMath data types start at index 64
            Vector2f = 64, // vector2 float
            Vector2d,
            Vector2b,
            Vector2i,
            Vector3f, // vector3 float
            Vector3d,
            Vector3b,
            Vector3i,
            Vector4f, // vector4 float
            Vector4d,
            Vector4b,
            Vector4i,
        }


        public LibMath() : base(nameof(LibMath), "System")
        {
            // register LibMath types e.g. Vector3Data as additional types
            // 2 component vector type
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Vector2f, typeof(Vector2f)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Vector2d, typeof(Vector2d)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Vector2b, typeof(Vector2b)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Vector2i, typeof(Vector2i)));
            // 3 component vector type
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Vector3f, typeof(Vector3f)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Vector3d, typeof(Vector3d)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Vector3b, typeof(Vector3b)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Vector3i, typeof(Vector3i)));
            // 4 component vector typee
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Vector4f, typeof(Vector4f)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Vector4d, typeof(Vector4d)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Vector4b, typeof(Vector4b)));
            MetaTypeRegistry.Register(new MetaType((int)DataTypes.Vector4i, typeof(Vector4i)));

            // ToDo: Ponder abount a solution to register and lookup MethodRef with additional options
            //      e.g.: BindingFlags to lookup static vs reference methods
            //          This might allow solving aliases which can be enabled by flag (though might be suuuper slow as no mapping exists yet
            //          This will most-likely require MethodNamedRef type to name and flag methodRefs inside the MethodList
            //          
            this.Methods.Nest(Int.Methods);
            this.Methods.Nest(Int_Static.Methods);
        }

        protected override bool LoadInternal()
        {
            // ToDo: ponder about doing self-check on registered libraries to avoid re-registering stuff
            return true;
        }



    }

}

