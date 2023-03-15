using System;

namespace NETGraph.Core.Meta
{

    public struct DataAccessor
    {
        public enum AccessTypes
        {
            Scalar,
            Index,
            Key,
        }

        private const string Mark_Index = "[";
        private const string Mark_Named = ".";


        public static readonly DataAccessor Scalar = new DataAccessor(string.Empty);
        public static DataAccessor Named(string key) => new DataAccessor($".{key}");
        public static DataAccessor Index(int index) => new DataAccessor($"[{index}]");


        public AccessTypes accessType { get; private set; }
        public int index { get; private set; }
        public string key { get; private set; }

        public DataAccessor(string accessor)
        {
            index = -1;
            key = string.Empty;
            if (string.IsNullOrEmpty(accessor))
            {
                accessType = AccessTypes.Scalar;
                index = -1;
                key = string.Empty;
            }
            else if (accessor.StartsWith(Mark_Index))
            {
                accessType = AccessTypes.Index;
                string indexString = accessor.Substring(accessor.IndexOf(Mark_Index) + 1).TrimEnd(']');
                if (int.TryParse(indexString, out int value))
                    index = value;
                else
                    throw new ArgumentException($"You need to include a valid index to access data by index. Remember to use the [] notation for {AccessTypes.Index} access.");
            }
            else
            {
                accessType = AccessTypes.Key;
                if (accessor.StartsWith(Mark_Named))
                    key = accessor.Substring(1);
                else
                    key = accessor;
            }
        }


        public static string ToAccessorString(string dataName, int index) => string.Format("{0}[{1}]", dataName, index);
        public static string ToAccessorString(string dataName, string key) => string.Format("{0}.{1}", dataName, key);
        public static string ToAccessorString(string dataName) => dataName;

        public override string ToString()
        {
            if (accessType == AccessTypes.Index)
                return "indexed value: " + index;
            if (accessType == AccessTypes.Key)
                return "named value: " + key;
            if (accessType == AccessTypes.Scalar)
                return "direct value";
            return "INVALID SIGNATURE";
        }




        public static implicit operator DataAccessor(string accessor)
        {
            return new DataAccessor(accessor);
        }

    }

}

