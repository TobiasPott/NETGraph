using System;
using System.Collections.Generic;

namespace NETGraph.Core.Meta
{
    public class DictData<T> : IData
    {
        public IData.Options options { get; private set; }
        public int typeIndex { get; private set; }

        private Dictionary<string, T> dict;


        internal DictData(Type type, IData.Options options) : this(MetaTypeRegistry.GetTypeIndex(type), options)
        { }
        internal DictData(int typeIndex, IData.Options options)
        {
            this.typeIndex = typeIndex;
            this.options = IData.Options.Named | options;
            this.dict = new Dictionary<string, T>();
        }

        protected void initializeNames(string[] names)
        {
            if (this.dict.Count == 0 && names.Length > 0)
                foreach (string name in names)
                    this.dict.Add(name, default);

        }


        internal T this[string name]
        {
            get => dict[name];
            set
            {
                if (!dict.ContainsKey(name))
                    dict.Add(name, value);
                else
                    dict[name] = value;
            }
        }


        internal object getValueAt(string name) => this[name];

        public virtual V resolve<V>(DataAccessor accessor)
        {
            if (accessor.accessType == DataAccessor.AccessTypes.Key)
                return (V)this.getValueAt(accessor.key);
            else
                throw new InvalidOperationException($"Cannot acceess {this.GetType()} as {accessor.accessType}.");

        }
        public virtual void assign(DataAccessor accessor, object value)
        {
            if (accessor.accessType == DataAccessor.AccessTypes.Key)
                this[accessor.key] = (T)value;
            else
                throw new InvalidOperationException($"Cannot acceess {this.GetType()} as {accessor.accessType}.");

        }

        public override string ToString()
        {
            string toString = string.Empty;
            if (this.options.HasFlag(IData.Options.Named))
                toString = $"dict = ({string.Join(", ", dict)})";
            else
                toString = $"INVALID {this.GetType()}";

            return $"DictData<{typeof(T).Name}> " + "{ " + string.Join(", ", toString, options) + " }";
        }

    }

}

