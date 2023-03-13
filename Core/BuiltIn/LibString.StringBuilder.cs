using System;
using System.Text;
using NETGraph.Core.Meta;

namespace NETGraph.Core.BuiltIn
{

    public class StringBuilderData : Data<StringBuilder>
    {

        public StringBuilderData(string content = "") : base(MetaTypeRegistry.GetDataTypeFor(typeof(StringBuilder).Name), DataOptions.Scalar)
        {
            initScalar(new StringBuilder(content));
        }

        public override string ToString()
        {
            return $"{this.scalar.ToString()}";
        }
    }


}

