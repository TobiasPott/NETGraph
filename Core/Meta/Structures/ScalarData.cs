
using System;

namespace NETGraph.Core.Meta
{

    public class ScalarData<T> : IData
    {
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
            Type vType = typeof(V);
            Type dType = typeof(T);
            if (vType.IsAssignableFrom(dType))
                return (V)(object)this.scalar;
            throw new InvalidOperationException($"Cannot resolve {dType} to {vType}");
        }
        public V resolve<V>(DataAccessor accessor)
        {
            if (accessor.accessType == DataAccessor.AccessTypes.Scalar)
                return (V)this.getValueScalar();
            else
                throw new InvalidOperationException($"Cannot acceess {this.GetType()} as {accessor.accessType}.");
        }

        public void assign<V>(V value)
        {
            // ToDo: add type check for IResolver and resolve them to target type
            Type vType = typeof(V);
            Type dType = typeof(T);
            if (vType.IsAssignableFrom(dType))
            {
                if (value.TryCast<T>(out T scalar))
                {
                    this.scalar = scalar;
                    return;
                }
            }
            throw new InvalidOperationException($"Cannot assign {dType} to {vType}");
        }
        // ToDo: Convert below assign methods with accessor arg to use a shared base method like assign<V>(V) to include automatic casting and assignment check
        // ToDo: Transfer results to IndexedData and NamedData types
        public void assign<V>(DataAccessor accessor, V value) => assign(accessor, (object)value);
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
            if (this.options.HasFlag(Options.Scalar))
                toString = $"scalar = {scalar}";
            else
                toString = $"INVALID {this.GetType()}";

            return $"ScalaData<{typeof(T).Name}> " + "{ " + string.Join(", ", toString, options) + " }";
        }
    }
}

