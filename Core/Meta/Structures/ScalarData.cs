
using System;

namespace NETGraph.Core.Meta
{

    public class ScalarData<T> : IData
    {
        public IData.Options options { get; private set; }
        public int typeIndex { get; private set; }

        internal T scalar { get; set; }


        internal ScalarData(Type type, IData.Options options) : this(MetaTypeRegistry.GetTypeIndex(type), options)
        { }
        internal ScalarData(int typeIndex, IData.Options options)
        {
            this.typeIndex = typeIndex;
            this.options = IData.Options.Scalar | options;
            this.scalar = default(T);
        }

        internal object getValueScalar() => this.scalar;

        public V resolve<V>(DataAccessor accessor)
        {
            if (accessor.accessType == DataAccessor.AccessTypes.Scalar)
                return (V)this.getValueScalar();
            else
                throw new InvalidOperationException($"Cannot acceess {this.GetType()} as {accessor.accessType}.");
        }
        public void assign(DataAccessor accessor, object value)
        {
            if (accessor.accessType == DataAccessor.AccessTypes.Scalar)
                this.scalar = (T)value;
            else
                throw new InvalidOperationException($"Cannot acceess {this.GetType()} as {accessor.accessType}.");
        }

        public override string ToString()
        {
            string toString = string.Empty;
            if (this.options.HasFlag(IData.Options.Scalar))
                toString = $"scalar = {scalar}";
            else
                toString = $"INVALID {this.GetType()}";

            return $"ScalaData<{typeof(T).Name}> " + "{ " + string.Join(", ", toString, options) + " }";
        }

    }
}

