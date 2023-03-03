using System;
using NETGraph.Core.Meta;

namespace NETGraph.Data
{
    public struct GeneratorDefinition : IDataGenerator
    {
        Func<object, DataBase> scalar;
        Func<int, bool, DataBase> list;
        Func<bool, DataBase> dict;

        public GeneratorDefinition(Func<object, DataBase> scalar, Func<int, bool, DataBase> list, Func<bool, DataBase> dict)
        {
            if (scalar == null || list == null || dict == null)
                throw new ArgumentNullException($"{nameof(scalar)},{nameof(list)} and {nameof(dict)} cannot be left empty. Please provide all generator methods.");
            this.scalar = scalar;
            this.list = list;
            this.dict = dict;
        }

        public DataBase Scalar(object scalar) => this.scalar.Invoke(scalar);
        public DataBase List(int size, bool isResizable) => this.list.Invoke(size, isResizable);
        public DataBase Dict(bool isRezisable) => this.dict.Invoke(isRezisable);


        // ToDo: Consider implementing a runtime Data Factory which builds new DataBase<T> typees by Type parameter
        //      This might be limited to reflection not available at runtime on iOS
        /*
          // Specify the type parameter of the A<> type
          Type genericType = typeof(A<>).MakeGenericType(new Type[] { o.GetType() });
          // Get the 'B' method and invoke it:
          object res = genericType.GetMethod("B").Invoke(new object[] { o });
          // Convert the result to string & return it
          return (string)res;
        */
    }

}

