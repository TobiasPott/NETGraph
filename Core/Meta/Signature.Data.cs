using System;
using NETGraph.Data;

namespace NETGraph.Core
{

    public struct DataSignature
    {
        public enum AccessTypes
        {
            Scalar,
            Index,
            Key,
            Void
        }

        private const string SplitMark_Key = ".";
        private const string SplitMark_Index = "#";

        public static readonly DataSignature Void = new DataSignature(string.Empty);
        public static readonly DataSignature Scalar = new DataSignature("-");
        public static DataSignature Named(string key) => new DataSignature($".{key}");
        public static DataSignature Index(int index) => new DataSignature($"{index}");


        public string dataName { get; private set; }
        public AccessTypes accessType { get; private set; }
        public int index { get; private set; }
        public string key { get; private set; }

        public DataSignature(string signature)
        {
            if (string.IsNullOrEmpty(signature))
            {
                accessType = AccessTypes.Void;
                dataName = string.Empty;
                index = -1;
                key = string.Empty;
            }
            else if (signature.Contains("."))
            {
                accessType = AccessTypes.Key;
                dataName = signature.Substring(0, signature.IndexOf('.'));
                index = -1;
                key = signature.Substring(dataName.Length + 1);
                if (string.IsNullOrEmpty(key))
                    throw new ArgumentException($"You need to include a key to access data by key. Consider leaving out the {SplitMark_Key} notation for {AccessTypes.Scalar} access.");
            }
            else if (signature.Contains("["))
            {
                accessType = AccessTypes.Index;
                dataName = signature.Substring(0, signature.IndexOf('['));
                key = string.Empty;
                string indexString = signature.Substring(dataName.Length + 1).TrimEnd(']');
                if (int.TryParse(indexString, out int value))
                    index = value;
                else
                    throw new ArgumentException($"You need to include a valid index to access data by index. Remember to use the {SplitMark_Index} notation for {AccessTypes.Index} access.");
            }
            else
            {
                accessType = AccessTypes.Scalar;
                dataName = signature;
                index = -1;
                key = string.Empty;
            }
        }


        public static string Signature(string dataName, int index) => string.Format("{0}[{1}]", dataName, index);
        public static string Signature(string dataName, string key) => string.Format("{0}.{1}", dataName, key);
        public static string Signature(string dataName) => dataName;

        public override string ToString()
        {
            if (accessType == AccessTypes.Index)
                return Signature(dataName, index);
            if (accessType == AccessTypes.Key)
                return Signature(dataName, key);
            if (accessType == AccessTypes.Scalar)
                return dataName;
            return "void";
        }
    }

}

