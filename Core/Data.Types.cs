using System;
using System.Collections.Generic;

namespace NETGraph.Core
{

    public enum DataStructures
    {
        Scalar,
        Array,
        List,
        Dict,
    }

    public static class TypeIndices
    {
        public const int Object = 0;
        public const int Bool = 1;
        public const int Short = 2;
        public const int Int = 3;
        public const int Long = 4;
        public const int Float = 5;
        public const int Double = 6;

        public static Dictionary<int, Type> Map = new Dictionary<int, Type>()
        {
            { Object, typeof(object) },
            { Bool, typeof(bool) },
            { Short, typeof(short) },
            { Int, typeof(int) },
            { Long, typeof(long) },
            { Float, typeof(float) },
            { Double, typeof(double) },
        };
    }


    public class ObjectData : Data<object>
    {
        public ObjectData(object scalar) : base(TypeIndices.Object, scalar) { }
        public ObjectData(IEnumerable<object> values, bool isRezisable) : base(TypeIndices.Object, values, isRezisable) { }
        public ObjectData(IEnumerable<KeyValuePair<string, object>> namedValues, bool isRezisable) : base(TypeIndices.Object, namedValues, isRezisable) { }

        public static DataDefinition Definition(string name, DataStructures structure = DataStructures.Scalar, bool isResizable = false, params string[] keys)
            => new DataDefinition(name, TypeIndices.Object, structure, isResizable, keys);
    }

    public class BoolData : Data<bool>
    {
        public BoolData(bool scalar) : base(TypeIndices.Bool, scalar) { }
        public BoolData(IEnumerable<bool> values, bool isRezisable) : base(TypeIndices.Bool, values, isRezisable) { }
        public BoolData(IEnumerable<KeyValuePair<string, bool>> namedValues, bool isRezisable) : base(TypeIndices.Bool, namedValues, isRezisable) { }

        public static DataDefinition Definition(string name, DataStructures structure = DataStructures.Scalar, bool isResizable = false, params string[] keys)
            => new DataDefinition(name, TypeIndices.Bool, structure, isResizable, keys);
    }


    public class ShortData : Data<short>
    {
        public ShortData(short scalar) : base(TypeIndices.Short, scalar) { }
        public ShortData(IEnumerable<short> values, bool isRezisable) : base(TypeIndices.Short, values, isRezisable) { }
        public ShortData(IEnumerable<KeyValuePair<string, short>> namedValues, bool isRezisable) : base(TypeIndices.Short, namedValues, isRezisable) { }

        public static DataDefinition Definition(string name, DataStructures structure = DataStructures.Scalar, bool isResizable = false, params string[] keys)
            => new DataDefinition(name, TypeIndices.Short, structure, isResizable, keys);
    }
    public class IntData : Data<int>
    {
        public IntData(int scalar) : base(TypeIndices.Int, scalar) { }
        public IntData(IEnumerable<int> values, bool isRezisable) : base(TypeIndices.Int, values, isRezisable) { }
        public IntData(IEnumerable<KeyValuePair<string, int>> namedValues, bool isRezisable) : base(TypeIndices.Int, namedValues, isRezisable) { }

        public static DataDefinition Definition(string name, DataStructures structure = DataStructures.Scalar, bool isResizable = false, params string[] keys)
            => new DataDefinition(name, TypeIndices.Int, structure, isResizable, keys);
    }
    public class LongData : Data<long>
    {
        public LongData(long scalar) : base(TypeIndices.Long, scalar) { }
        public LongData(IEnumerable<long> values, bool isRezisable) : base(TypeIndices.Long, values, isRezisable) { }
        public LongData(IEnumerable<KeyValuePair<string, long>> namedValues, bool isRezisable) : base(TypeIndices.Long, namedValues, isRezisable) { }

        public static DataDefinition Definition(string name, DataStructures structure = DataStructures.Scalar, bool isResizable = false, params string[] keys)
            => new DataDefinition(name, TypeIndices.Long, structure, isResizable, keys);
    }


    public class FloatData : Data<float>
    {
        public FloatData(float scalar) : base(TypeIndices.Float, scalar) { }
        public FloatData(IEnumerable<float> values, bool isRezisable) : base(TypeIndices.Float, values, isRezisable) { }
        public FloatData(IEnumerable<KeyValuePair<string, float>> namedValues, bool isRezisable) : base(TypeIndices.Float, namedValues, isRezisable) { }

        public static DataDefinition Definition(string name, DataStructures structure = DataStructures.Scalar, bool isResizable = false, params string[] keys)
            => new DataDefinition(name, TypeIndices.Float, structure, isResizable, keys);
    }
    public class DoubleData : Data<double>
    {
        public DoubleData(double scalar) : base(TypeIndices.Double, scalar) { }
        public DoubleData(IEnumerable<double> values, bool isRezisable) : base(TypeIndices.Double, values, isRezisable) { }
        public DoubleData(IEnumerable<KeyValuePair<string, double>> namedValues, bool isRezisable) : base(TypeIndices.Double, namedValues, isRezisable) { }

        public static DataDefinition Definition(string name, DataStructures structure = DataStructures.Scalar, bool isResizable = false, params string[] keys)
            => new DataDefinition(name, TypeIndices.Double, structure, isResizable, keys);
    }

}

