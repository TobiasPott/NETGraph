using System;

namespace NETGraph.Core
{
    //  interface for accessor processing/handling
    //      data provider allows access to specific data
    //      method provider allows invoking a method with given name and data accessors as input
    //
    //  a type implementing both is able to self-invoke methods
    //  a type implementing data provision only can be queried for data from other types
    //  a type implementing method provision only can be used independent from own data access


    public interface IDataProvider
    {
        Data Access(DataAccessor accessor);
        Data<T> Access<T>(DataAccessor accessor);
    }
    public struct DataAccessor
    {
        public enum AccessTypes
        {
            Scalar,
            Index,
            Key
        }

        private const string SplitMark_Key = ".";
        private const string SplitMark_Index = "#";


        public string dataName { get; private set; }
        public AccessTypes accessType { get; private set; }
        public int index { get; private set; }
        public string key { get; private set; }

        public DataAccessor(string accessPath)
        {
            if (accessPath.Contains("."))
            {
                accessType = AccessTypes.Key;
                dataName = accessPath.Substring(0, accessPath.IndexOf('.'));
                index = -1;
                key = accessPath.Substring(dataName.Length + 1);
                if (string.IsNullOrEmpty(key))
                    throw new ArgumentException($"You need to include a key to access data by key. Consider leaving out the {SplitMark_Key} notation for {AccessTypes.Scalar} access.");
                //Console.WriteLine($"Accessor: {dataName} by key '{key}'");
            }
            else if (accessPath.Contains("["))
            {
                accessType = AccessTypes.Index;
                dataName = accessPath.Substring(0, accessPath.IndexOf('['));
                key = string.Empty;
                string indexString = accessPath.Substring(dataName.Length + 1).TrimEnd(']');
                if (int.TryParse(indexString, out int value))
                    index = value;
                else
                    throw new ArgumentException($"You need to include a valid index to access data by index. Remember to use the {SplitMark_Index} notation for {AccessTypes.Index} access.");
                //Console.WriteLine($"Accessor: {dataName} by index '{index}'");
            }
            else
            {
                accessType = AccessTypes.Scalar;
                dataName = accessPath;
                index = -1;
                key = string.Empty;
                //Console.WriteLine($"Accessor: {dataName} by scalar");
            }
        }

        public static string AccessPath(string dataName, int index) => string.Format("{0}[{1}]", dataName, index);
        public static string AccessPath(string dataName, string key) => string.Format("{0}.{1}", dataName, key);
        public static string AccessPath(string dataName) => dataName;

        public override string ToString()
        {
            if (accessType == AccessTypes.Index)
                return AccessPath(dataName, index);
            if (accessType == AccessTypes.Key)
                return AccessPath(dataName, key);
            return dataName;
        }
    }


    public interface IMethodProvider
    {
        void Invoke(MethodAccessor accessor, params Data[] input);
        T Invoke<T>(MethodAccessor accessor, params Data[] input);
    }
    public struct MethodAccessor
    {
        private const string SplitMark_Arguments = "<<";
        private static readonly string[] SplitMarks_Arguments = new string[] { ",", " " };

        int typeIndex;
        string method;
        DataAccessor[] accessors;

        public MethodAccessor(string methodPath)
        {
            typeIndex = -1;
            method = string.Empty;
            accessors = null;
            string[] argumentSplit = methodPath.Split(SplitMark_Arguments, StringSplitOptions.RemoveEmptyEntries);
            if (argumentSplit.Length > 0)
            {
                string[] nameSplit = argumentSplit[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                typeIndex = parseTypeIndex(nameSplit[0]);
                method = parseMethod(nameSplit[1]);
                Console.WriteLine($"\t{typeIndex}{Environment.NewLine}\t{method}");

                if (argumentSplit.Length > 1)
                {
                    string[] accessorsSplit = argumentSplit[1].Split(SplitMarks_Arguments, StringSplitOptions.RemoveEmptyEntries);
                    if (accessorsSplit.Length > 0)
                    {
                        accessors = new DataAccessor[accessorsSplit.Length];
                        for (int i = 0; i < accessorsSplit.Length; i++)
                            accessors[i] = new DataAccessor(accessorsSplit[i]);
                    }
                    else
                        accessors = null;
                }

            }

        }

        private int parseTypeIndex(string typeIndexString)
        {
            if (int.TryParse(typeIndexString.Trim(), out int index))
                return index;
            return -1;
        }
        private string parseMethod(string methodString)
        {
            return methodString.Trim();
        }

    }

}

