using System;
using System.Collections.Generic;

namespace NETGraph.Core
{


    public struct AccessorDefinition
    {
        public const string AccessorScalar = "[scalar]";

        public string name;
        public string dataName; // name of data mapped by the accessor
        public string dataAccessor; // accessor (subname) to query data for, [scalar], [#index] or ['key']

        public AccessorDefinition(string name, string dataName, string dataAccessor)
        {
            this.name = name;
            this.dataName = dataName;
            this.dataAccessor = dataAccessor;
        }

        public bool isScalar => dataAccessor.Equals(AccessorScalar);
        public int Index => int.Parse(dataAccessor);
        public string Key => dataAccessor;

    }


}

