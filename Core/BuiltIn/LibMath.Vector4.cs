using System;
using NETGraph.Core.Meta;
using System.Linq;
using System.Collections.Generic;

namespace NETGraph.Core.BuiltIn
{

    public abstract class Vector4Base<T> : Data<T>
    {
        private static string[] keys = new[] { "x", "y", "z", "w" };

        public DataResolver x { get; private set; }
        public DataResolver y { get; private set; }
        public DataResolver z { get; private set; }
        public DataResolver w { get; private set; }

        public Vector4Base(T x, T y, T z, T w) : base(MetaTypeRegistry.GetDataTypeFor(typeof(T).Name), IData.Options.Named)
        {
            initNamed(keys.Select(k => new KeyValuePair<string, T>(k, default)));
            this.assign(keys[0], x);
            this.assign(keys[1], y);
            this.assign(keys[2], z);
            this.assign(keys[3], w);

            this.x = new DataResolver(this, keys[0]);
            this.y = new DataResolver(this, keys[1]);
            this.z = new DataResolver(this, keys[2]);
            this.w = new DataResolver(this, keys[3]);
        }

        public override string ToString()
        {
            return $"{keys[0]}:{this[keys[0]]}, {keys[1]}:{this[keys[1]]}, {keys[2]}:{this[keys[2]]}, {keys[3]}:{this[keys[3]]}";
        }
    }


    public class Vector4Byte : Vector4Base<byte>
    {
        public Vector4Byte(byte x, byte y, byte z, byte w) : base(x, y, z, w) { }
    }
    public class Vector4Double : Vector4Base<double>
    {
        public Vector4Double(double x, double y, double z, double w) : base(x, y, z, w) { }
    }
    public class Vector4Int : Vector4Base<int>
    {
        public Vector4Int(int x, int y, int z, int w) : base(x, y, z, w) { }
    }
    public class Vector4 : Vector4Base<float>
    {
        public Vector4(float x, float y, float z, float w) : base(x, y, z, w) { }
    }

}

