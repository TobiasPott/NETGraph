using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NETGraph.Core.Meta.CodeGen
{
    public class MethodExtraction
    {

        public static string ExtractMethod<T>(string name, BindingFlags binding, params Type[] paramTypes) => ExtractMethod(typeof(T), name, binding, paramTypes);
        public static string ExtractMethod(Type type, string name, BindingFlags binding, params Type[] paramTypes)
        {
            const string TypeName = "IData";
            MethodInfo mi;
            if (paramTypes.Length == 0)
                mi = type.GetMethod(name, binding);
            else
                mi = type.GetMethod(name, binding, null, paramTypes, null);
            if (mi != null)
            {
                ParameterInfo returnPi = mi.ReturnParameter;
                ParameterInfo[] parameters = mi.GetParameters();

                StringBuilder sb = new StringBuilder();

                string referencePart = binding.HasFlag(BindingFlags.Instance) ? $"reference.resolve<{returnPi.ParameterType}>()" : $"{type.FullName}";

                int argsIndex = 0;
                sb.AppendLine($"private static {TypeName} {mi.Name}({TypeName} reference, params {TypeName}[] args)");
                sb.AppendLine("{");
                sb.AppendLine("\t// resolve arguments to individual unnderlying types");
                if (parameters.Length == 0)
                    sb.AppendLine("\t// -> method has no parameters");
                foreach (ParameterInfo pi in parameters)
                {
                    sb.AppendLine($"\t{pi.ParameterType} {pi.Name} = args[{argsIndex}].resolve<{pi.ParameterType}>();");
                    argsIndex++;
                }

                sb.AppendLine("\t// call method and optionally wrap result into ValueType<T> ");
                if (!returnPi.ParameterType.Equals(typeof(void)))
                    sb.Append($"\t{returnPi.ParameterType} result = ");
                sb.AppendLine($"{referencePart}.{mi.Name}({string.Join(", ", parameters.Select(x => x.Name))});");
                if (!returnPi.ParameterType.Equals(typeof(void)))
                    sb.AppendLine($"\treturn result.AsValueData<{returnPi.ParameterType}>();");
                // ToDo: Add internal static reference to Void data which will be returned for void methods
                else
                    sb.AppendLine($"\treturn Memory.Void;");

                sb.AppendLine("}");

                return sb.ToString();
            }
            else
                Console.WriteLine($"// Method {name} on {type} not found.");
            return string.Empty;
        }


    }
}

