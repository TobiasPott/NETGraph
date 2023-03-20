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
            if (CoreExtensions.IsAssignableFrom<V, Dictionary<string, T>>() && this.dict.TryCast<V>(out V casted))
                return casted;
            throw new InvalidOperationException($"Cannot resolve {typeof(Dictionary<string, T>)} to {typeof(V)}");
        }
        public virtual V resolve<V>(DataAccessor accessor)
        {
            if (accessor.accessType == DataAccessor.AccessTypes.Key)
                return (V)this.getValueAt(accessor.key);
            else
                throw new InvalidOperationException($"Cannot acceess {this.GetType()} as {accessor.accessType}.");

        }

        public void assign<V>(V value) => this.assign<V>(DataAccessor.Scalar, value);
        public void assign<V>(DataAccessor accessor, V value)
        {
            if (accessor.accessType == DataAccessor.AccessTypes.Index)
            {
                if (CoreExtensions.TryCastOrResolve<T, V>(out T casted, value))
                {
                    this[accessor.key] = casted;
                    return;
                }
            }
            else if (accessor.accessType == DataAccessor.AccessTypes.Scalar)
            {
                if (CoreExtensions.TryCastOrResolve<Dictionary<string, T>, V>(out Dictionary<string, T> casted, value))
                {
                    this.dict = casted;
                    return;
                }
            }
            throw new InvalidOperationException($"Cannot assign {accessor.accessType} on {this.GetType().Name}");
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

