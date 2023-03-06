using System;
using NETGraph.Data;

namespace NETGraph.Core.Meta
{

    public struct DataSignature
    {
        public enum AccessTypes
        {
            Scalar,
            Index,
            Key,
            Discard
        }

        private const string SplitMark_Key = ".";
        private const string SplitMark_Index = "#";
        private const string Mark_Discard = "_";
        private const string Mark_Index = "[";
        private const string Mark_Named = ".";


        public static readonly DataSignature Discard = new DataSignature(Mark_Discard);
        public static readonly DataSignature Scalar = new DataSignature(string.Empty);
        public static DataSignature Named(string key) => new DataSignature($".{key}");
        public static DataSignature Index(int index) => new DataSignature($"[{index}]");


        public AccessTypes accessType { get; private set; }
        public int index { get; private set; }
        public string key { get; private set; }

        public DataSignature(string signature)
        {
            index = -1;
            key = string.Empty;
            if (string.IsNullOrEmpty(signature))
            {
                accessType = AccessTypes.Scalar;
                index = -1;
                key = string.Empty;
            }
            else if (signature.Equals(Mark_Discard))
            {
                accessType = AccessTypes.Discard;
            }
            else if (signature.StartsWith(Mark_Index))
            {
                accessType = AccessTypes.Index;
                string indexString = signature.Substring(signature.IndexOf(Mark_Index) + 1).TrimEnd(']');
                if (int.TryParse(indexString, out int value))
                    index = value;
                else
                    throw new ArgumentException($"You need to include a valid index to access data by index. Remember to use the {SplitMark_Index} notation for {AccessTypes.Index} access.");
            }
            else
            {
                accessType = AccessTypes.Key;

                if (signature.StartsWith(Mark_Named))
                    key = signature.Substring(1);
                else
                    key = signature;
                if (string.IsNullOrEmpty(key))
                    throw new ArgumentException($"You need to include a key to access data by key. Consider leaving out the {SplitMark_Key} notation for {AccessTypes.Scalar} access.");
            }
        }


        public static string Signature(string dataName, int index) => string.Format("{0}[{1}]", dataName, index);
        public static string Signature(string dataName, string key) => string.Format("{0}.{1}", dataName, key);
        public static string Signature(string dataName) => dataName;

        public override string ToString()
        {
            if (accessType == AccessTypes.Index)
                return "indexed value: " + index;
            if (accessType == AccessTypes.Key)
                return "named value: " + key;
            if (accessType == AccessTypes.Scalar)
                return "direct value";
            return "discard";
        }
    }

}

