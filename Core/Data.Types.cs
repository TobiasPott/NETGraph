using System;
using System.Collections.Generic;

namespace NETGraph.Core
{

    public class ObjectData : Data<object>
    {
        public ObjectData(string name, object scalar) : base(name, TypeIndices.Object, scalar)
        { }
        public ObjectData(string name, IEnumerable<object> values, bool isRezisable) : base(name, TypeIndices.Object, values, isRezisable)
        { }
        public ObjectData(string name, IEnumerable<KeyValuePair<string, object>> namedValues, bool isRezisable) : base(name, TypeIndices.Object, namedValues, isRezisable)
        { }
    }

    public class BoolData : Data<bool>
    {
        public BoolData(string name, bool scalar) : base(name, TypeIndices.Bool, scalar)
        { }
        public BoolData(string name, IEnumerable<bool> values, bool isRezisable) : base(name, TypeIndices.Bool, values, isRezisable)
        { }
        public BoolData(string name, IEnumerable<KeyValuePair<string, bool>> namedValues, bool isRezisable) : base(name, TypeIndices.Bool, namedValues, isRezisable)
        { }
    }


    public class ShortData : Data<short>
    {
        public ShortData(string name, short scalar) : base(name, TypeIndices.Short, scalar)
        { }
        public ShortData(string name, IEnumerable<short> values, bool isRezisable) : base(name, TypeIndices.Short, values, isRezisable)
        { }
        public ShortData(string name, IEnumerable<KeyValuePair<string, short>> namedValues, bool isRezisable) : base(name, TypeIndices.Short, namedValues, isRezisable)
        { }
    }
    public class IntData : Data<int>
    {
        public IntData(string name, int scalar) : base(name, TypeIndices.Int, scalar)
        { }
        public IntData(string name, IEnumerable<int> values, bool isRezisable) : base(name, TypeIndices.Int, values, isRezisable)
        { }
        public IntData(string name, IEnumerable<KeyValuePair<string, int>> namedValues, bool isRezisable) : base(name, TypeIndices.Int, namedValues, isRezisable)
        { }
    }
    public class LongData : Data<long>
    {
        public LongData(string name, long scalar) : base(name, TypeIndices.Long, scalar)
        { }
        public LongData(string name, IEnumerable<long> values, bool isRezisable) : base(name, TypeIndices.Long, values, isRezisable)
        { }
        public LongData(string name, IEnumerable<KeyValuePair<string, long>> namedValues, bool isRezisable) : base(name, TypeIndices.Long, namedValues, isRezisable)
        { }
    }


    public class FloatData : Data<float>
    {
        public FloatData(string name, float scalar) : base(name, TypeIndices.Float, scalar)
        { }
        public FloatData(string name, IEnumerable<float> values, bool isRezisable) : base(name, TypeIndices.Float, values, isRezisable)
        { }
        public FloatData(string name, IEnumerable<KeyValuePair<string, float>> namedValues, bool isRezisable) : base(name, TypeIndices.Float, namedValues, isRezisable)
        { }
    }
    public class DoubleData : Data<double>
    {
        public DoubleData(string name, double scalar) : base(name, TypeIndices.Double, scalar)
        { }
        public DoubleData(string name, IEnumerable<double> values, bool isRezisable) : base(name, TypeIndices.Double, values, isRezisable)
        { }
        public DoubleData(string name, IEnumerable<KeyValuePair<string, double>> namedValues, bool isRezisable) : base(name, TypeIndices.Double, namedValues, isRezisable)
        { }
    }

}

