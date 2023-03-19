using System;
using System.Collections.Generic;

namespace NETGraph.Core.Meta
{
    // ToDo: Implement base class for MutableData with generic type
    //      class based IData implementations (builtin) allow for data persistence and mutation, struct based can only carry local changes
    public class NamedData<T> : IData
    {
        public Options options { get; private set; }
        public int typeIndex { get; private set; }

        private Dictionary<string, T> dict;


        internal NamedData(Type type, Options options) : this(MetaTypeRegistry.GetTypeIndex(type), options)
        { }
        internal NamedData(int typeIndex, Options options)
        {
            this.typeIndex = typeIndex;
            this.options = Options.Named | options;
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

        public V resolve<V>()
        {
            Type vType = typeof(V);
            Type dType = typeof(Dictionary<string, T>);
            if (vType.IsAssignableFrom(dType))
                return (V)(object)this.dict;
            throw new InvalidOperationException($"Cannot resolve {dType} to {vType}");
        }
        public virtual V resolve<V>(DataAccessor accessor)
        {
            if (accessor.accessType == DataAccessor.AccessTypes.Key)
                return (V)this.getValueAt(accessor.key);
            else
                throw new InvalidOperationException($"Cannot acceess {this.GetType()} as {accessor.accessType}.");

        }

        public void assign<V>(V value)
        {
            Type vType = typeof(V);
            Type dType = typeof(Dictionary<string, T>);
            if (dType.IsAssignableFrom(vType))
            {
                this.dict = (Dictionary<string, T>)(object)value;
                return;
            }
            throw new InvalidOperationException($"Cannot assign {dType} to {vType}");
        }
        public void assign<V>(DataAccessor accessor, V value) => assign(accessor, (object)value);
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
            if (this.options.HasFlag(Options.Named))
                toString = $"dict = ({string.Join(", ", dict)})";
            else
                toString = $"INVALID {this.GetType()}";

            return $"DictData<{typeof(T).Name}> " + "{ " + string.Join(", ", toString, options) + " }";
        }

    }

}

