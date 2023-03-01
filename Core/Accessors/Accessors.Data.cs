using System;
using NETGraph.Data;

namespace NETGraph.Core
{
    //  interface for accessor processing/handling
    //      data provider allows access to specific data
    //      method provider allows invoking a method with given name and data accessors as input
    //
    //  a type implementing both is able to self-invoke methods
    //  a type implementing data provision only can be queried for data from other types
    //  a type implementing method provision only can be used independent from own data access


    // ToDo: add constructor overload to create accessor from string accessPath passinng it into the Accessor ctor
    public struct DataQuery
    {
        DataAccessor accessor;
        IDataProvider provider;

        public DataQuery(IDataProvider provider, DataAccessor accessor)
        {
            this.accessor = accessor;
            this.provider = provider;
        }
        public DataQuery(IDataProvider provider, string accessPath)
        {
            this.accessor = new DataAccessor(accessPath);
            this.provider = provider;
        }

        public DataBase Evaluate() => provider.Access(accessor);
    }

    public interface IDataProvider
    {
        DataBase Access(DataAccessor accessor);
    }
    public struct DataAccessor
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

        public static readonly DataAccessor Void = new DataAccessor(string.Empty);


        public string dataName { get; private set; }
        public AccessTypes accessType { get; private set; }
        public int index { get; private set; }
        public string key { get; private set; }

        public DataAccessor(string accessPath)
        {
            if (string.IsNullOrEmpty(accessPath))
            {
                accessType = AccessTypes.Void;
                dataName = string.Empty;
                index = -1;
                key = string.Empty;
            }
            else if (accessPath.Contains("."))
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
            if (accessType == AccessTypes.Scalar)
                return dataName;
            return "void";
        }
    }

}

