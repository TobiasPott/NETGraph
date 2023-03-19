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

        public V resolve<V>()
        {
            Type vType = typeof(V);
            Type dType = typeof(List<T>);
            if (vType.IsAssignableFrom(dType))
                return (V)(object)this.list;
            throw new InvalidOperationException($"Cannot resolve {dType} to {vType}");
        }
        public virtual V resolve<V>(DataAccessor accessor)
        {
            if (accessor.accessType == DataAccessor.AccessTypes.Index)
                return (V)this.getValueAt(accessor.index);
            else
            {
                if (typeof(V).Equals(list.GetType()))
                    return (V)(object)this.list; // ToDo: ponder about weird object cast solution (e.g. object typed backing field
                else
                    throw new InvalidOperationException($"Cannot acceess {this.GetType()} as {accessor.accessType}.");
            }
        }

        public void assign<V>(V value)
        {
            Type vType = typeof(V);
            Type dType = typeof(List<T>);
            if (dType.IsAssignableFrom(vType))
            {
                this.list = (List<T>)(object)value;
                return;
            }
            throw new InvalidOperationException($"Cannot assign {dType} to {vType}");
        }
        public void assign<V>(DataAccessor accessor, V value) => assign(accessor, (object)value);
        public virtual void assign(DataAccessor accessor, object value)
        {
            if (accessor.accessType == DataAccessor.AccessTypes.Index)
                this[accessor.index] = (T)value;
            else
                throw new InvalidOperationException($"Cannot acceess {this.GetType()} as {accessor.accessType}.");
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

