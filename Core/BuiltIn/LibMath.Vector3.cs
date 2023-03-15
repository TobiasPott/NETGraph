using System;
using NETGraph.Core.Meta;
using System.Linq;
using System.Collections.Generic;

namespace NETGraph.Core.BuiltIn
{

    public abstract class Vector3Base<T> : Data<T>
    {
        private static string[] keys = new[] { "x", "y", "z" };

        public DataResolver x { get; private set; }
        public DataResolver y { get; private set; }
        public DataResolver z { get; private set; }

        public Vector3Base(T x, T y, T z) : base(typeof(T), IData.Options.Named)
        {
            initNamed(keys.Select(k => new KeyValuePair<string, T>(k, default)));
            this.assign(keys[0], x);
            this.assign(keys[1], y);

            this.x = new DataResolver(this, keys[0]);
            this.y = new DataResolver(this, keys[1]);
            this.z = new DataResolver(this, keys[2]);
        }

        public override string ToString()
        {
            return $"{this[keys[0]]} + {this[keys[1]]} = {this[keys[2]]}";
        }
    }


    public class Vector3Byte : Vector3Base<byte>
    {
        public Vector3Byte(byte x, byte y, byte z) : base(x, y, z) { }
    }
    public class Vector3Double : Vector3Base<double>
    {
        public Vector3Double(double x, double y, double z) : base(x, y, z) { }
    }
    public class Vector3Int : Vector3Base<int>
    {
        public Vector3Int(int x, int y, int z) : base(x, y, z) { }
    }
    public class Vector3 : Vector3Base<float>
    {
        public Vector3(float x, float y, float z) : base(x, y, z) { }
    }

}

