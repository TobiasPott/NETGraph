using System;
using System.Collections.Generic;

namespace NETGraph.Core
{


    public abstract class NodeDefinition
    {

        List<AccessorDefinition> accessors = new List<AccessorDefinition>();
        List<DataDefinition> datas = new List<DataDefinition>();

        protected NodeDefinition(IEnumerable<AccessorDefinition> accessors, IEnumerable<DataDefinition> datas)
        {
            this.accessors.AddRange(accessors);
            this.datas.AddRange(datas);
        }


    }
}

