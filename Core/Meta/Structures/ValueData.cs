using System;

namespace NETGraph.Core.Meta
{
    public struct ValueData<T> : IData
    {

        public Options options { get; private set; }
        public int typeIndex { get; private set; }

        private T scalar { get; set; }


        internal ValueData(T value)
        {
            this.typeIndex = typeof(T).GetTypeIndex();
            this.options = Options.Scalar;
            this.scalar = value;
        }

        internal object getValueScalar() => this.scalar;


        public V resolve<V>() => resolve<V>(DataAccessor.Scalar);
        public V resolve<V>(DataAccessor accessor)
        {
            if (accessor.accessType == DataAccessor.AccessTypes.Scalar)
            {
                if (this.scalar.TryAssignableCast<V>(out V casted))
                    return casted;
                throw new InvalidOperationException($"Cannot resolve {typeof(T)} to {typeof(V)}");
            }
            throw new InvalidOperationException($"Cannot resolve {accessor.accessType} on {this.GetType()}");
        }
        public void assign<V>(V value) => this.assign<V>(DataAccessor.Scalar, value);
        public void assign<V>(DataAccessor accessor, V value)
        {
            if (accessor.accessType == DataAccessor.AccessTypes.Scalar)
            {
                if (CoreExtensions.TryCastOrResolve<T, V>(out T casted, value))
                {
                    this.scalar = casted;
                    return;
                }
            }
            throw new InvalidOperationException($"Cannot assign {this.GetType()} by {accessor.accessType}.");
        }


        public override string ToString()
        {
            string toString = $"scalar = {scalar}";
            return $"ScalaData<{typeof(T)}> " + "{ " + string.Join(", ", toString, options) + " }";
        }
    }
}
