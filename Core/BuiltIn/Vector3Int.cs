using System;
using NETGraph.Core.Meta;
using System.Linq;
using System.Collections.Generic;

namespace NETGraph.Core.BuiltIn
{

    public abstract class Vector3DataBase<T> : Data<T>
    {
        private static string[] keys = new[] { "x", "y", "z" };

        public DataResolver x { get; private set; }
        public DataResolver y { get; private set; }
        public DataResolver z { get; private set; }

        public Vector3DataBase(T x, T y, T z) : base(MetaTypeRegistry.GetDataTypeFor(typeof(T).Name), DataOptions.Named)
        {
            initNamed(keys.Select(k => new KeyValuePair<string, T>(k, default)));
            this.assign(keys[0], x);
            this.assign(keys[1], y);
            this.assign(keys[2], z);

            this.x = new DataResolver(this, "x");
            this.y = new DataResolver(this, "y");
            this.z = new DataResolver(this, "z");
        }

        public override string ToString()
        {
            return $"{this[keys[0]]} + {this[keys[1]]} = {this[keys[2]]}";
        }
    }


    public class Vector3IntData : Vector3DataBase<int>
    {
        public Vector3IntData(int x, int y, int z) : base(x, y, z)
        { }
    }
    public class Vector3Data : Vector3DataBase<float>
    {
        public Vector3Data(float x, float y, float z) : base(x, y, z)
        { }
    }

}

