using System;
using System.Collections.Generic;
using System.Linq;

namespace NETGraph.Core
{
    //  interface for accessor processing/handling
    //      data provider allows access to specific data
    //      method provider allows invoking a method with given name and data accessors as input
    //
    //  a type implementing both is able to self-invoke methods
    //  a type implementing data provision only can be queried for data from other types
    //  a type implementing method provision only can be used independent from own data access

    public struct MethodQuery
    {
        IMethodProvider provider;
        MethodAccessor accessor;
        DataQuery resultQuery;
        DataQuery[] inputQueries;

        public MethodQuery(IMethodProvider provider, MethodAccessor accessor, DataQuery resultQuery, params DataQuery[] inputQueries)
        {
            this.provider = provider;
            this.accessor = accessor;
            this.resultQuery = resultQuery;
            this.inputQueries = inputQueries;
        }
        public MethodQuery(IMethodProvider provider, string accessPath, DataQuery resultQuery, params DataQuery[] inputQueries) : this(provider, new MethodAccessor(accessPath), resultQuery, inputQueries)
        { }

        public bool Evaluate() => provider.Invoke(accessor, resultQuery.Evaluate(), inputQueries.Select(q => q.Evaluate()));
    }

    public interface IMethodProvider
    {
        bool Invoke(MethodAccessor accessor, Data result, params Data[] inputs);
        bool Invoke(MethodAccessor accessor, Data result, IEnumerable<Data> inputs);
    }
    public struct MethodAccessor
    {
        private const string SplitMark_Arguments = "<<";
        private static readonly string[] SplitMarks_Arguments = new string[] { ",", " " };

        DataAccessor result;
        public string method { get; private set; }
        DataAccessor[] inputs;

        public MethodAccessor(string methodPath)
        {
            inputs = null;
            string[] argumentSplit = methodPath.Split(SplitMark_Arguments, StringSplitOptions.RemoveEmptyEntries);
            if (argumentSplit.Length > 0)
            {
                // split result accessor and method name
                string[] nameSplit = argumentSplit[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                result = new DataAccessor(nameSplit[0].Trim());
                method = nameSplit[1].Trim();

                // split the inputs part if it exists
                if (argumentSplit.Length > 1)
                {
                    string[] inputsSplit = argumentSplit[1].Split(SplitMarks_Arguments, StringSplitOptions.RemoveEmptyEntries);
                    if (inputsSplit.Length > 0)
                    {
                        inputs = new DataAccessor[inputsSplit.Length];
                        for (int i = 0; i < inputsSplit.Length; i++)
                            inputs[i] = new DataAccessor(inputsSplit[i]);
                    }
                }
            }
            else
                throw new ArgumentException($"No valid method pass found in {methodPath}. Please check your input.");
        }


    }

}

