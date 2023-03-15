using System;
using static NETGraph.Core.Meta.IData;

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
        public IData Generate() => this.withOptions.Invoke(IData.Options.Scalar);


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

        public static DataGenerator Generator<T>()
        {
            return new DataGenerator((o) =>
            {
                int typeIndex = MetaTypeRegistry.GetTypeIndex(typeof(T));
                if (o.HasFlag(IData.Options.List))
                    return new ListData<T>(typeIndex, o);
                else if (o.HasFlag(IData.Options.Named))
                    return new DictData<T>(typeIndex, o);
                else
                    return new ScalarData<T>(typeIndex, o);
            });
        }

    }
}

