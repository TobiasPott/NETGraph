using System;
using System.Collections.Generic;
using System.Linq;

namespace NETGraph.Core.Meta
{
    public delegate IResolver MethodRef(IResolver reference, params IResolver[] inputs);

}

