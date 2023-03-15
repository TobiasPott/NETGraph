using System;

namespace NETGraph.Core.Meta
{
    public struct DataGenerator : IGenerator<IData, IData.Options>
    {
        Func<IData.Options, IData> withOptions;

        public DataGenerator(Func<IData.Options, IData> withOptions)
        {
            this.withOptions = withOptions;
        }

        public IData Generate(IData.Options options) => this.withOptions.Invoke(options);


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

