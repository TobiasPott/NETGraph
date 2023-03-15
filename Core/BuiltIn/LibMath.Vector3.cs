using System;
using NETGraph.Core.Meta;
using System.Linq;
using System.Collections.Generic;

namespace NETGraph.Core.BuiltIn
{

    public class Vector3<T> : DictData<T>
    {
        private static string[] keys = new[] { "x", "y", "z" };

        public DataResolver x { get; private set; }
        public DataResolver y { get; private set; }
        public DataResolver z { get; private set; }

        public Vector3(T x, T y, T z) : base(typeof(T), IData.Options.Named)
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


    public class Vector3b : Vector3<byte>
    {
        public Vector3b(byte x, byte y, byte z) : base(x, y, z) { }
    }
    public class Vector3d : Vector3<double>
    {
        public Vector3d(double x, double y, double z) : base(x, y, z) { }
    }
    public class Vector3i : Vector3<int>
    {
        public Vector3i(int x, int y, int z) : base(x, y, z) { }
    }
    public class Vector3f : Vector3<float>
    {
        public Vector3f(float x, float y, float z) : base(x, y, z) { }
    }

}

