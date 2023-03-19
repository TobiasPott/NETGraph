using System;
using System.Collections.Generic;

namespace NETGraph.Core.Meta
{
    public class ListData<T> : IData
    {
        public IData.Options options { get; private set; }
        public int typeIndex { get; private set; }

        private List<T> list;


        internal ListData(Type type, IData.Options options) : this(MetaTypeRegistry.GetTypeIndex(type), options)
        { }
        internal ListData(int typeIndex, IData.Options options)
        {
            this.typeIndex = typeIndex;
            this.options = IData.Options.List | options;
            this.list = new List<T>();
        }


        internal T this[int index]
        {
            get => list[index];
            set => list[index] = value;
        }



        internal object getValueAt(int index) => this[index];

        public virtual V resolve<V>(DataAccessor accessor)
        {
            if (accessor.accessType == DataAccessor.AccessTypes.Index)
                return (V)this.getValueAt(accessor.index);
            else
                throw new InvalidOperationException($"Cannot acceess {this.GetType()} as {accessor.accessType}.");
        }
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
            if (this.options.HasFlag(IData.Options.List))
                toString = $"list = ({string.Join(", ", list)})";
            else
                toString = $"INVALID {this.GetType()}";

            return $"ListData<{typeof(T).Name}> " + "{ " + string.Join(", ", toString, options) + " }";
        }

    }
}

