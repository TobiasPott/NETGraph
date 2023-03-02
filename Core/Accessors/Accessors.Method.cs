using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        bool Invoke(MethodAccessor accessor, DataBase result, params DataBase[] inputs);
        bool Invoke(MethodAccessor accessor, DataBase result, IEnumerable<DataBase> inputs);
    }
    public struct MethodAccessor
    {
        // Regex for method path parssing
        private static readonly Regex regEx = new Regex("(?:(?:\\b[a-zA-Z]{1}(?:[\\w.\\[\\]]+))+,{0})", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        DataAccessor result;
        public string method { get; private set; }
        DataAccessor[] inputs;

        public MethodAccessor(string methodPath)
        {
            inputs = null;
            MatchCollection matches = regEx.Matches(methodPath);
            if (matches.Count >= 2)
            {
                // proces matches for result and name parts
                result = new DataAccessor(matches[0].Value);
                method = matches[1].Value;

                if (matches.Count > 2)
                {
                    inputs = new DataAccessor[matches.Count - 2];
                    // process matches for input argument parts
                    for (int i = 2; i < matches.Count; i++)
                        inputs[i - 2] = new DataAccessor(matches[i].Value);
                }
            }
            else
                throw new ArgumentException($"No valid method pass found in {methodPath}. Please check your input.");
        }


    }

}

