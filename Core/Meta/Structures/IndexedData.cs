using System;
using System.Collections.Generic;

namespace NETGraph.Core.Meta
{
    public class IndexedData<T> : IData
    {
        public Options options { get; private set; }
        public int typeIndex { get; private set; }

        private List<T> list;


        internal IndexedData(Type type, Options options) : this(MetaTypeRegistry.GetTypeIndex(type), options)
        { }
        internal IndexedData(int typeIndex, Options options)
        {
            this.typeIndex = typeIndex;
            this.options = Options.List | options;
            this.list = new List<T>();
        }


        internal T this[int index]
        {
            get => list[index];
            set => list[index] = value;
        }



        internal object getValueAt(int index) => this[index];

        public V resolve<V>() => resolve<V>(DataAccessor.Scalar);
        public virtual V resolve<V>(DataAccessor accessor)
        {
            if (accessor.accessType == DataAccessor.AccessTypes.Index)
            {
                if (CoreExtensions.IsAssignableFrom<V, T>() && this.getValueAt(accessor.index).TryCast<V>(out V casted))
                    return casted;
                throw new InvalidOperationException($"Cannot resolve [{accessor.index}] of {typeof(T)} to {typeof(V)}");
            }
            else if (accessor.accessType == DataAccessor.AccessTypes.Scalar)
            {
                if (CoreExtensions.IsAssignableFrom<V, List<T>>() && this.list.TryCast<V>(out V casted))
                    return casted;
            }
            throw new InvalidOperationException($"Cannot resolve {accessor.accessType} on {this.GetType().Name}");
        }



        public void assign<V>(V value) => this.assign<V>(DataAccessor.Scalar, value);
        public void assign<V>(DataAccessor accessor, V value)
        {
            // ToDo: Ponder about a way to reduce IsAssignableFrom, TryCast and necessary IResolver.resolve<T> to single method
            //      This should reduce code clutter and noise
            if (accessor.accessType == DataAccessor.AccessTypes.Index)
            {
                if (CoreExtensions.TryCastOrResolve<T, V>(out T casted, value))
                {
                    this[accessor.index] = casted;
                    return;
                }
            }
            else if (accessor.accessType == DataAccessor.AccessTypes.Scalar)
            {
                if (CoreExtensions.TryCastOrResolve<List<T>, V>(out List<T> casted, value))
                {
                    this.list = casted;
                    return;
                }
            }
            throw new InvalidOperationException($"Cannot assign {accessor.accessType} on {this.GetType().Name}");
        }



        public override string ToString()
        {
            string toString = string.Empty;
            if (this.options.HasFlag(Options.List))
                toString = $"list = ({string.Join(", ", list)})";
            else
                toString = $"INVALID {this.GetType()}";

            return $"ListData<{typeof(T).Name}> " + "{ " + string.Join(", ", toString, options) + " }";
        }

    }
}

