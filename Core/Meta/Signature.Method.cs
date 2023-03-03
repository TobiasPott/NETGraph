﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NETGraph.Data;

namespace NETGraph.Core.Meta
{

    public interface IMethodProvider
    {
        bool Invoke(MethodSignature signature, DataResolver result, params DataResolver[] inputs);
        bool Invoke(MethodSignature signature, DataResolver result, IEnumerable<DataResolver> inputs);
    }
    public struct MethodSignature
    {
        // Regex for method path parssing
        private static readonly Regex regEx = new Regex("(?:(?:\\b[a-zA-Z]{1}(?:[\\w.\\[\\]]+))+,{0})", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string method { get; private set; }
        public DataTypes resultType { get; private set; }
        

        public MethodSignature(string signature)
        {
            //inputs = null;
            MatchCollection matches = regEx.Matches(signature);
            if (matches.Count >= 2)
            {
                // proces matches for result and name parts
                method = matches[1].Value;
                string resultTypeStr = matches[0].Value;
                // ToDo: implement type index lookup by typename through the TypeRegistry
                //      May require auto-init of TypeRegistry for built in type info

                resultType = Enum.Parse<DataTypes>(resultTypeStr);
                
                // ToDo: all matches beyond index = 1 are signatures of input types
                //      these can be parsed and stored as meta info for a method signature
            }
            else
                throw new ArgumentException($"No valid method pass found in {signature}. Please check your input.");
        }


    }

}

