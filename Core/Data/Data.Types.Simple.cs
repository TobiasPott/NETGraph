using System;
using System.Collections.Generic;
using NETGraph.Core;

namespace NETGraph.Data.Simple
{

    //public class ObjectData : DataBase<object>
    //{
    //    public ObjectData(object scalar) : base(DataTypes.Object, scalar) { }
    //    public ObjectData(IEnumerable<object> values, bool isRezisable) : base(DataTypes.Object, values, isRezisable) { }
    //    public ObjectData(IEnumerable<KeyValuePair<string, object>> namedValues, bool isRezisable) : base(DataTypes.Object, namedValues, isRezisable) { }

    //    public static DataDefinition Definition(string name, DataStructures structure = DataStructures.Scalar, bool isResizable = false, params string[] keys)
    //        => new DataDefinition(name, DataTypes.Object, structure, isResizable, keys);
    //    public static GeneratorDefinition Generator()
    //    {
    //        GeneratorDefinition generator = new GeneratorDefinition((s) => new ObjectData(s), (s, r) => new ObjectData(), dict);
    //    }
    //}

    //public class BoolData : DataBase<bool>
    //{
    //    public BoolData(bool scalar) : base(DataTypes.Bool, scalar) { }
    //    public BoolData(IEnumerable<bool> values, bool isRezisable) : base(DataTypes.Bool, values, isRezisable) { }
    //    public BoolData(IEnumerable<KeyValuePair<string, bool>> namedValues, bool isRezisable) : base(DataTypes.Bool, namedValues, isRezisable) { }

    //    public static DataDefinition Definition(string name, DataStructures structure = DataStructures.Scalar, bool isResizable = false, params string[] keys)
    //        => new DataDefinition(name, DataTypes.Bool, structure, isResizable, keys);
    //}

    //public class ShortData : DataBase<short>
    //{
    //    public ShortData(short scalar) : base(DataTypes.Short, scalar) { }
    //    public ShortData(IEnumerable<short> values, bool isRezisable) : base(DataTypes.Short, values, isRezisable) { }
    //    public ShortData(IEnumerable<KeyValuePair<string, short>> namedValues, bool isRezisable) : base(DataTypes.Short, namedValues, isRezisable) { }

    //    public static DataDefinition Definition(string name, DataStructures structure = DataStructures.Scalar, bool isResizable = false, params string[] keys)
    //        => new DataDefinition(name, DataTypes.Short, structure, isResizable, keys);
    //}
    public class IntData : DataBase<int>
    {
        public IntData(int typeIndex, DataStructure structure, bool isResizable) : base(typeIndex, structure, isResizable)
        { }

        public static GeneratorDefinition Generator { get; } = new GeneratorDefinition(
            (s) => (IntData)new IntData((int)DataTypes.Int, DataStructure.Scalar, false).initScalar((int)s),
            (s, r) => (IntData)new IntData((int)DataTypes.Int, DataStructure.List, r),
            (r) => (IntData)new IntData((int)DataTypes.Int, DataStructure.Named, r)
        );
        //public static DataDefinition Definition(string name, DataStructures structure = DataStructures.Scalar, bool isResizable = false, params string[] keys)
        //    => new DataDefinition(name, DataTypes.Int, structure, isResizable, keys);
    }
    //public class LongData : DataBase<long>
    //{
    //    public LongData(long scalar) : base(DataTypes.Long, scalar) { }
    //    public LongData(IEnumerable<long> values, bool isRezisable) : base(DataTypes.Long, values, isRezisable) { }
    //    public LongData(IEnumerable<KeyValuePair<string, long>> namedValues, bool isRezisable) : base(DataTypes.Long, namedValues, isRezisable) { }

    //    public static DataDefinition Definition(string name, DataStructures structure = DataStructures.Scalar, bool isResizable = false, params string[] keys)
    //        => new DataDefinition(name, DataTypes.Long, structure, isResizable, keys);
    //}

    //public class FloatData : DataBase<float>
    //{
    //    public FloatData(float scalar) : base(DataTypes.Float, scalar) { }
    //    public FloatData(IEnumerable<float> values, bool isRezisable) : base(DataTypes.Float, values, isRezisable) { }
    //    public FloatData(IEnumerable<KeyValuePair<string, float>> namedValues, bool isRezisable) : base(DataTypes.Float, namedValues, isRezisable) { }

    //    public static DataDefinition Definition(string name, DataStructures structure = DataStructures.Scalar, bool isResizable = false, params string[] keys)
    //        => new DataDefinition(name, DataTypes.Float, structure, isResizable, keys);
    //}
    //public class DoubleData : DataBase<double>
    //{
    //    public DoubleData(double scalar) : base(DataTypes.Double, scalar) { }
    //    public DoubleData(IEnumerable<double> values, bool isRezisable) : base(DataTypes.Double, values, isRezisable) { }
    //    public DoubleData(IEnumerable<KeyValuePair<string, double>> namedValues, bool isRezisable) : base(DataTypes.Double, namedValues, isRezisable) { }

    //    public static DataDefinition Definition(string name, DataStructures structure = DataStructures.Scalar, bool isResizable = false, params string[] keys)
    //        => new DataDefinition(name, DataTypes.Double, structure, isResizable, keys);
    //}

}

