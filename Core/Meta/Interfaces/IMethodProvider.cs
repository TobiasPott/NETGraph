using System;
using System.Collections.Generic;
using System.Linq;

namespace NETGraph.Core.Meta
{
    public delegate IResolver MethodRef(IResolver reference, params IResolver[] inputs);


    // ToDo: Complete a list of methods of the string type to extract code
    //      include method name
    //      binding (public + instance/static)
    //      param type array (list of input types of method)

    // ToDo: Build map of possible method calls and signatures (e.g. static, instance, operator, nested, assignment, result
    //  Base rules:
    //      if contains '=' call is assignment to left hand side
    //      if '=' is preceeded by two tokens, the call includes allocation of new data of result type which the call result is assigned to
    //      if '=' is preceeded by one token, the call assigns to left hand
    //
    //  Samples: () enclose optional parts, numeric values represent const values, string/chars represent named data
    //  int x;
    //  (int) x = 0;
    //  (int) x = 1 + 1;
    //  (int) x = LibMath::add(1, 1)
    //  (int) x = LibMath::add(y, z);
    //  (int) x = v.magnitude();
    //  (int) x = y.offset(z);

}

