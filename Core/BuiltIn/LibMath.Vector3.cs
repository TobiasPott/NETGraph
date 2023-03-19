﻿using System;
using NETGraph.Core.Meta;
using System.Linq;
using System.Collections.Generic;

namespace NETGraph.Core.BuiltIn
{

    public class Vector3<T> : NamedData<T>
    {
        private static string[] keys = new[] { "x", "y", "z" };

        public Vector3(T x, T y, T z) : base(typeof(T), Options.Named)
        {
            this.initializeNames(keys);
            this.assign(keys[0], x);
            this.assign(keys[1], y);
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

