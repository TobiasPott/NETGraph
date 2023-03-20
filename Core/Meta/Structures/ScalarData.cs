
using System;

namespace NETGraph.Core.Meta
{

    public class ScalarData<T> : IData
    {
        private Type TGeneric = typeof(T);

        public Options options { get; private set; }
        public int typeIndex { get; private set; }

        internal T scalar { get; set; }


        internal ScalarData(Type type, Options options) : this(MetaTypeRegistry.GetTypeIndex(type), options)
        { }
        internal ScalarData(int typeIndex, Options options)
        {
            this.typeIndex = typeIndex;
            this.options = Options.Scalar | options;
            this.scalar = default(T);
        }

        internal object getValueScalar() => this.scalar;


        public V resolve<V>()
        {
            // ToDo: Add TryCast to IndexedData and NamedData
            if (CoreExtensions.IsAssignableFrom<V, T>() && this.scalar.TryCast<V>(out V casted))
                return casted;
            throw new InvalidOperationException($"Cannot resolve {typeof(T)} to {typeof(V)}");
        }
        public V resolve<V>(DataAccessor accessor)
        {
            if (accessor.accessType == DataAccessor.AccessTypes.Scalar)
                return (V)this.getValueScalar();
            else
                throw new InvalidOperationException($"Cannot acceess {this.GetType()} as {accessor.accessType}.");
        }

        // ToDo: Convert below assign methods with accessor arg to use a shared base method like assign<V>(V) to include automatic casting and assignment check
        //          base assign implementation can check if V is IResolver and try to assign resolved value instead (this would allow assign int to index accessed list data
        // ToDo: Transfer results to IndexedData and NamedData types
        public void assign<V>(V value) => this.assign<V>(DataAccessor.Scalar, value);
        public void assign<V>(DataAccessor accessor, V value)
        {
            if (accessor.accessType == DataAccessor.AccessTypes.Scalar)
            {
                if(CoreExtensions.TryCastOrResolve<T, V>(out T casted, value))
                {
                    this.scalar = casted;
                    return;
                }
            }
            throw new InvalidOperationException($"Cannot assign {this.GetType()} by {accessor.accessType}.");
        }


        public override string ToString()
        {
            string toString = string.Empty;
            if (this.options.HasFlag(Options.Scalar))
                toString = $"scalar = {scalar}";
            else
                toString = $"INVALID {this.GetType()}";

            return $"ScalaData<{typeof(T).Name}> " + "{ " + string.Join(", ", toString, options) + " }";
        }
    }
}

