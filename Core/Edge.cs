using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{

    public interface IEdge {
        int u { get; set; }
        int v { get; set; }
        bool directed { get; set; }

        IEdge reversed();
    }

}
