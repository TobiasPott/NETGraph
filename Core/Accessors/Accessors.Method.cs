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

        public bool Evaluate() => provider.Invoke(accessor, resultQuery, inputQueries);
    }

    public interface IMethodProvider
    {
        bool Invoke(MethodAccessor accessor, DataQuery result, params DataQuery[] inputs);
        bool Invoke(MethodAccessor accessor, DataQuery result, IEnumerable<DataQuery> inputs);
    }
    public struct MethodAccessor
    {
        // Regex for method path parssing
        private static readonly Regex regEx = new Regex("(?:(?:\\b[a-zA-Z]{1}(?:[\\w.\\[\\]]+))+,{0})", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string method { get; private set; }
        
        //DataAccessor result;
        //DataAccessor[] inputs;

        public MethodAccessor(string methodPath)
        {
            //inputs = null;
            MatchCollection matches = regEx.Matches(methodPath);
            if (matches.Count >= 2)
            {
                // proces matches for result and name parts
                method = matches[1].Value;
                // ToDo: all matches beyond index = 1 are signatures of input types
                //      these can be parsed and stored as meta info for a method signature
            }
            else
                throw new ArgumentException($"No valid method pass found in {methodPath}. Please check your input.");
        }


    }

}

