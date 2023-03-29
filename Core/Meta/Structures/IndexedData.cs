using System;
using System.Collections.Generic;

namespace NETGraph.Core.Meta
{
    public class IndexedData<T> : IData
    {
        public Options options { get; private set; }
        public int typeIndex { get; private set; }

        private List<T> list;


        internal IndexedData(Type type, Options options) : this(type.GetTypeIndex(), options)
        { }
        internal IndexedData(int typeIndex, Options options)
        {
            this.typeIndex = typeIndex;
            this.options = Options.Index | options;
            this.list = new List<T>();
        }

        internal T this[int index]
        {
            get => list[index];
            set => list[index] = value;
        }

        public void initializeWith(IEnumerable<T> values)
        {
            if (this.list.Count == 0)
            {
                this.list.AddRange(values);
            }
            else
                throw new InvalidOperationException("Cannot initialise data again. Consider to clear the data or create a new instance.");
        }


        internal object getValueAt(int index) => this[index];

        public V resolve<V>() => resolve<V>(DataAccessor.Scalar);
        public virtual V resolve<V>(DataAccessor accessor)
        {
            if (accessor.accessType == DataAccessor.AccessTypes.Index)
            {
                if (this.getValueAt(accessor.index).TryAssignableCast<V>(out V casted))
                    return casted;
                throw new InvalidOperationException($"Cannot resolve [{accessor.index}] of {typeof(T)} to {typeof(V)}");
            }
            else if (accessor.accessType == DataAccessor.AccessTypes.Scalar)
            {
                if (this.list.TryAssignableCast<V>(out V casted))
                    return casted;
                throw new InvalidOperationException($"Cannot resolve {typeof(List<T>)} to {typeof(V)}");
            }
            throw new InvalidOperationException($"Cannot resolve {accessor.accessType} on {this.GetType()}");
        }

        public void assign<V>(V value) => this.assign<V>(DataAccessor.Scalar, value);
        public void assign<V>(DataAccessor accessor, V value)
        {
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
            string toString = $"list = ({string.Join(", ", list)})";
            return $"ListData<{typeof(T)}> " + "{ " + string.Join(", ", toString, options) + " }";
        }

    }
}

