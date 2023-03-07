using System;
using NETGraph.Core.Meta;
using System.Linq;
using System.Collections.Generic;

namespace NETGraph.Core.BuiltIn
{

    public abstract class Vector4DataBase<T> : Data<T>
    {
        private static string[] keys = new[] { "x", "y", "z", "w" };

        public DataResolver x { get; private set; }
        public DataResolver y { get; private set; }
        public DataResolver z { get; private set; }
        public DataResolver w { get; private set; }

        public Vector4DataBase(T x, T y, T z, T w) : base(MetaTypeRegistry.GetDataTypeFor(typeof(T).Name), DataOptions.Named)
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


    public class Vector4ByteData : Vector4DataBase<byte>
    {
        public Vector4ByteData(byte x, byte y, byte z, byte w) : base(x, y, z, w) { }
    }
    public class Vector4DoubleData : Vector4DataBase<double>
    {
        public Vector4DoubleData(double x, double y, double z, double w) : base(x, y, z, w) { }
    }
    public class Vector4IntData : Vector4DataBase<int>
    {
        public Vector4IntData(int x, int y, int z, int w) : base(x, y, z, w) { }
    }
    public class Vector4Data : Vector4DataBase<float>
    {
        public Vector4Data(float x, float y, float z, float w) : base(x, y, z, w) { }
    }

}

