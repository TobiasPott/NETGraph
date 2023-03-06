using System;
namespace NETGraph.Core.Meta
{

    public struct MetaTypeBlueprint
    {
        private static int _runningIndex = 1024;    // reserved 1024 indices for builtin/internal types
                                                    // may add additional reserved ranges for other purposes (e.g. Unity range)

        public DataTypes dataType => (DataTypes)typeIndex;
        public int typeIndex { get; private set; }
        public string typeName { get; private set; }
        public Type type { get; private set; }
        public IDataGenerator generator { get; private set; }

        public MetaTypeBlueprint(DataTypes dataType, IDataGenerator generator)
        {
            this.typeName = dataType.ToString().ToLowerInvariant();
            this.typeIndex = (int)dataType;
            this.type = TypeMapping.instance.BuiltInTypeFor(dataType);
            this.generator = generator;
        }
        public MetaTypeBlueprint(Type type, IDataGenerator generator)
        {
            this.typeIndex = _runningIndex++;
            this.typeName = type.Name;
            this.type = type;
            this.generator = generator;
        }

    }


}

