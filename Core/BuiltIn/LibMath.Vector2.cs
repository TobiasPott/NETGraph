using System;
using NETGraph.Core.Meta;
using System.Linq;
using System.Collections.Generic;

namespace NETGraph.Core.BuiltIn
{

    public abstract class Vector2Base<T> : Data<T>
    {
        private static string[] keys = new[] { "x", "y" };

        public DataResolver x { get; private set; }
        public DataResolver y { get; private set; }

        public Vector2Base(T x, T y) : base(typeof(T), IData.Options.Named)
        {
            initNamed(keys.Select(k => new KeyValuePair<string, T>(k, default)));
            this.assign(keys[0], x);
            this.assign(keys[1], y);

            this.x = new DataResolver(this, keys[0]);
            this.y = new DataResolver(this, keys[1]);
        }

        public override string ToString()
        {
            return $"{keys[0]}:{this[keys[0]]}, {keys[1]}:{this[keys[1]]}";
        }
    }


    public class Vector2Byte : Vector2Base<byte>
    {
        public Vector2Byte(byte x, byte y) : base(x, y) { }
    }
    public class Vector2Double : Vector2Base<double>
    {
        public Vector2Double(double x, double y) : base(x, y) { }
    }
    public class Vector2Int : Vector2Base<int>
    {
        public Vector2Int(int x, int y) : base(x, y) { }
    }
    public class Vector2 : Vector2Base<float>
    {
        public Vector2(float x, float y) : base(x, y) { }
    }

}

