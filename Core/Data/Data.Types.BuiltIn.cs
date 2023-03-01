using System;
using System.Collections.Generic;
using NETGraph.Core;
using NETGraph.Data;

namespace NETGraph.Types.BuiltIn
{

    public class MethodQueryData : DataBase<MethodQuery>
    {
        public MethodQueryData(MethodQuery scalar) : base(DataTypes.MethodQuery, scalar) { }
        public MethodQueryData(IEnumerable<MethodQuery> values, bool isRezisable) : base(DataTypes.MethodQuery, values, isRezisable) { }
        public MethodQueryData(IEnumerable<KeyValuePair<string, MethodQuery>> namedValues, bool isRezisable) : base(DataTypes.MethodQuery, namedValues, isRezisable) { }

        public static DataDefinition Definition(string name, DataStructures structure = DataStructures.Scalar, bool isResizable = false, params string[] keys)
            => new DataDefinition(name, DataTypes.Object, structure, isResizable, keys);
    }
    public class IMethodProviderData : DataBase<IMethodProvider>
    {
        public IMethodProviderData(IMethodProvider scalar) : base(DataTypes.IMethodProvider, scalar) { }
        public IMethodProviderData(IEnumerable<IMethodProvider> values, bool isRezisable) : base(DataTypes.IMethodProvider, values, isRezisable) { }
        public IMethodProviderData(IEnumerable<KeyValuePair<string, IMethodProvider>> namedValues, bool isRezisable) : base(DataTypes.IMethodProvider, namedValues, isRezisable) { }

        public static DataDefinition Definition(string name, DataStructures structure = DataStructures.Scalar, bool isResizable = false, params string[] keys)
            => new DataDefinition(name, DataTypes.IMethodProvider, structure, isResizable, keys);
    }
    public class MethodAccessorData : DataBase<MethodAccessor>
    {
        public MethodAccessorData(MethodAccessor scalar) : base(DataTypes.MethodAccessor, scalar) { }
        public MethodAccessorData(IEnumerable<MethodAccessor> values, bool isRezisable) : base(DataTypes.MethodAccessor, values, isRezisable) { }
        public MethodAccessorData(IEnumerable<KeyValuePair<string, MethodAccessor>> namedValues, bool isRezisable) : base(DataTypes.MethodAccessor, namedValues, isRezisable) { }

        public static DataDefinition Definition(string name, DataStructures structure = DataStructures.Scalar, bool isResizable = false, params string[] keys)
            => new DataDefinition(name, DataTypes.MethodAccessor, structure, isResizable, keys);
    }

}

