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
        // ToDo: add alias for type names
        public Type type { get; private set; }
        public IDataGenerator generator { get; private set; }

        public MetaTypeBlueprint(int dataType, Type type, IDataGenerator generator)
        {
            this.typeName = type.Name.ToLowerInvariant();
            this.typeIndex = (int)dataType;
            this.type = type;
            this.generator = generator;
        }
        public MetaTypeBlueprint(Type type, IDataGenerator generator)
        {
            this.typeIndex = _runningIndex++;
            this.typeName = type.Name.ToLowerInvariant();
            this.type = type;
            this.generator = generator;
        }

    }


}

