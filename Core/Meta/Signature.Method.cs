using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NETGraph.Core.Meta
{


    //public class IntData : Data<int>
    //{ protected IntData(Type type, DataOptions options) : base(type, options) { } }

    //public struct MethodSignature
    //{
    //    // regex: 
    //    //      (?:(?:[a-zA-Z]{1}(?:[\w.\[\]]+))+),{0}      // split by , (for method signature)
    //    //      (?:(?:[a-zA-Z]{1}(?:[\w\[\]]?))+).{0}       // split by . (for data path)
    //    // sample:
    //    //      int32 add(int32, int32)
    //    //      int32.add(int32)
    //    // Regex for method path parsing
    //    private static readonly Regex regEx = new Regex("(?:(?:[a-zA-Z]{1}(?:[\\w.\\[\\]]+))+),{0}", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    //    public string method { get; private set; }
    //    public DataTypes resultType { get; private set; }


    //    public MethodSignature(string signature)
    //    {
    //        //inputs = null;
    //        MatchCollection matches = regEx.Matches(signature);
    //        if (matches.Count >= 2)
    //        {
    //            // proces matches for result and name parts
    //            method = matches[1].Value;
    //            string resultTypeStr = matches[0].Value;
    //            resultType = MetaTypeRegistry.GetDataTypeFor(resultTypeStr);

    //            // ToDo: all matches beyond index = 1 are signatures of input types
    //            //      these can be parsed and stored as meta info for a method signature
    //        }
    //        else
    //            throw new ArgumentException($"No valid method pass found in {signature}. Please check your input.");
    //    }


    //}

}

