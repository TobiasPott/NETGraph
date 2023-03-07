using System;
using NETGraph.Core.Meta;
using System.Linq;
using System.Collections.Generic;

namespace NETGraph.Core.BuiltIn
{

    public abstract class Vector2DataBase<T> : Data<T>
    {
        private static string[] keys = new[] { "x", "y" };

        public DataResolver x { get; private set; }
        public DataResolver y { get; private set; }

        public Vector2DataBase(T x, T y) : base(MetaTypeRegistry.GetDataTypeFor(typeof(T).Name), DataOptions.Named)
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


    public class Vector2ByteData : Vector2DataBase<byte>
    {
        public Vector2ByteData(byte x, byte y) : base(x, y) { }
    }
    public class Vector2DoubleData : Vector2DataBase<double>
    {
        public Vector2DoubleData(double x, double y) : base(x, y) { }
    }
    public class Vector2IntData : Vector2DataBase<int>
    {
        public Vector2IntData(int x, int y) : base(x, y) { }
    }
    public class Vector2Data : Vector2DataBase<float>
    {
        public Vector2Data(float x, float y) : base(x, y) { }
    }

}

