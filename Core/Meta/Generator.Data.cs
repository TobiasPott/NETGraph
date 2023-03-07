using System;

namespace NETGraph.Core.Meta
{

    public interface IDataGenerator
    {
        DataBase Scalar();
        DataBase List(bool isResizable);
        DataBase Named(bool isRezisable);
    }

    public struct GeneratorDefinition : IDataGenerator
    {
        Func<DataBase> scalar;
        Func<bool, DataBase> list;
        Func<bool, DataBase> dict;

        public GeneratorDefinition(Func<DataBase> scalar, Func<bool, DataBase> list, Func<bool, DataBase> dict)
        {
            if (scalar == null || list == null || dict == null)
                throw new ArgumentNullException($"{nameof(scalar)},{nameof(list)} and {nameof(dict)} cannot be left empty. Please provide all generator methods.");
            this.scalar = scalar;
            this.list = list;
            this.dict = dict;
        }

        public DataBase Scalar() => this.scalar.Invoke();
        public DataBase List(bool isResizable) => this.list.Invoke(isResizable);
        public DataBase Named(bool isRezisable) => this.dict.Invoke(isRezisable);


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

