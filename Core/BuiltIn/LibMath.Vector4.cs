using System;
using NETGraph.Core.Meta;
using System.Linq;
using System.Collections.Generic;

namespace NETGraph.Core.BuiltIn
{

    public class Vector4<T> : NamedData<T>
    {
        private static string[] keys = new[] { "x", "y", "z", "w" };

        public Vector4(T x, T y, T z, T w) : base(typeof(T), Options.Named)
        {
            this.initializeWith(keys);
            this.assign(keys[0], x);
            this.assign(keys[1], y);
            this.assign(keys[2], z);
            this.assign(keys[3], w);
        }

        public override string ToString()
        {
            return $"{keys[0]}:{this[keys[0]]}, {keys[1]}:{this[keys[1]]}, {keys[2]}:{this[keys[2]]}, {keys[3]}:{this[keys[3]]}";
        }
    }


    public class Vector4b : Vector4<byte>
    {
        public Vector4b(byte x, byte y, byte z, byte w) : base(x, y, z, w) { }
    }
    public class Vector4d : Vector4<double>
    {
        public Vector4d(double x, double y, double z, double w) : base(x, y, z, w) { }
    }
    public class Vector4i : Vector4<int>
    {
        public Vector4i(int x, int y, int z, int w) : base(x, y, z, w) { }
    }
    public class Vector4f : Vector4<float>
    {
        public Vector4f(float x, float y, float z, float w) : base(x, y, z, w) { }
    }

}

