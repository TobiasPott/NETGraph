using System;

namespace NETGraph.Core.Meta
{
    // ToDo: Implement parser for string command to create/register new data/variable
    // ToDo: Implement parser for string method to assign call (call with return type and optional reference)
    //      parser needs to distinguish between assignment to variable and returnless invokation
    //
    //      Sample:
    //          int32 i;                // creates new int variable named 'i' and store in data frame (comparable to a StackFrame?)
    //          i = x;                  // assigns RH to LH (can be made method, is part of method)
    //                                  // can also be written as i = x.assign(); 
    //          i = Math::Add(x, y);    // calls static/referenceless method and assigns to LH (assignment call)
    //          i = x.Add(y);           // calls instance method and assigns to LH (assignment call)
    //
    //
    //      base parser steps:
    //          check for ; ending
    //          check for = and split
    //              * left hand is assignment data
    //              * right hand needs further parsing and is reference data, signature and input data
    //              * right hand reference can be static named, contains :: or no .
    //              * right hand reference is instance if contains .
}

