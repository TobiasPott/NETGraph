using System;
using NETGraph.Core.Meta;
using System.Linq;
using System.Collections.Generic;

namespace NETGraph.Core.BuiltIn.Methods
{

    using Type = System.Int32;
    using DataInterface = IData;
    public static class Int_Static
    {
        public static MethodList Methods
        {
            get
            {
                MethodList methods = new MethodList("LibMath", null);
                methods.Set(new MethodHandle($"{nameof(Add)}", MethodBindings.Static), Add);
                methods.Set(new MethodHandle($"{nameof(Subtract)}", MethodBindings.Static), Subtract);
                methods.Set(new MethodHandle($"{nameof(Multiply)}", MethodBindings.Static), Multiply);
                methods.Set(new MethodHandle($"{nameof(Divide)}", MethodBindings.Static), Divide);
                return methods;
            }
        }

        public static DataInterface Add(DataInterface reference, params DataInterface[] args)
        {
            // static implementation ignores reference argument
            Type sum = args.Sum(q => q.resolve<Type>());
            return new ValueData<Type>(sum);
        }
        public static DataInterface Subtract(DataInterface reference, IEnumerable<DataInterface> inputs)
        {
            Type miuend = inputs.First().resolve<Type>();
            Type subtrahends = inputs.Skip(1).Sum(q => q.resolve<Type>());
            return new ValueData<Type>(miuend - subtrahends);
        }
        public static DataInterface Multiply(DataInterface reference, IEnumerable<DataInterface> inputs)
        {
            Type product = 1;
            foreach (DataInterface input in inputs)
                product *= input.resolve<Type>();
            return new ValueData<Type>(product);
        }
        public static DataInterface Divide(DataInterface reference, IEnumerable<DataInterface> inputs)
        {
            Type dividend = inputs.First().resolve<Type>();
            foreach (DataInterface input in inputs.Skip(1))
                dividend /= input.resolve<Type>();
            return new ValueData<Type>(dividend);
        }
    }

}

