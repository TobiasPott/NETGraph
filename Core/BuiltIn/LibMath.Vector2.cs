using System;
using NETGraph.Core.Meta;
using System.Linq;
using System.Collections.Generic;

namespace NETGraph.Core.BuiltIn
{

    public class Vector2<T> : NamedData<T>
    {
        private static string[] keys = new[] { "x", "y" };

        public Vector2(T x, T y) : base(typeof(T), Options.Named)
        {
            this.initializeNames(keys);
            this.assign(keys[0], x);
            this.assign(keys[1], y);
        }

        public override string ToString()
        {
            return $"{keys[0]}:{this[keys[0]]}, {keys[1]}:{this[keys[1]]}";
        }
    }


    public class Vector2b : Vector2<byte>
    {
        public Vector2b(byte x, byte y) : base(x, y) { }
    }
    public class Vector2d : Vector2<double>
    {
        public Vector2d(double x, double y) : base(x, y) { }
    }
    public class Vector2i : Vector2<int>
    {
        public Vector2i(int x, int y) : base(x, y) { }
    }
    public class Vector2f : Vector2<float>
    {
        public Vector2f(float x, float y) : base(x, y) { }
    }

}

