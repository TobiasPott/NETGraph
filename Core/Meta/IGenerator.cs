using System;

namespace NETGraph.Core.Meta
{

    public interface IGenerator<T, O>
    {
        T Generate(O options);
    }

}

