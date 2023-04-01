using System;
using System.Collections.Generic;
using System.Linq;
using NETGraph.Core.BuiltIn;

namespace NETGraph.Core.Meta
{

    public class MetaTypeRegistry
    {
        internal static Dictionary<int, MetaType> typeRegistry = new Dictionary<int, MetaType>();
        internal static Dictionary<int, Type> underlyingTypesMap = new Dictionary<int, Type>();

        public static bool Register(MetaType blueprint)
        {
            if (!typeRegistry.ContainsKey(blueprint.typeIndex))
            {
                // query for typeIndex of generate new from blueprint running index
                typeRegistry.Add(blueprint.typeIndex, blueprint);

                if (!underlyingTypesMap.ContainsKey(blueprint.typeIndex))
                    underlyingTypesMap.Add(blueprint.typeIndex, blueprint.type);
                else if (!underlyingTypesMap[blueprint.typeIndex].Equals(blueprint.type))
                    Console.WriteLine($"{blueprint.typeIndex} is already registered with {underlyingTypesMap[blueprint.typeIndex]} and cannot be registeered again. Try using a different data type for {blueprint.type} or {typeof(Custom)}.");
            }
            return false;
        }


        public static Type GetType(int typeIndex) => underlyingTypesMap.First(x => x.Key == typeIndex).Value;

    }

    public static class MetaTypeExtension
    {
        public static bool GetDataInfo(this string arg, out int typeIndex, out Options options)
        {
            options = Options.Scalar;
            if (arg.EndsWith("[]"))
            {
                options = Options.Resizable | Options.Index;
                arg = arg.TrimEnd('[', ']');
            }
            if (arg.EndsWith("<>"))
            {
                options = Options.Resizable | Options.Named;
                arg = arg.TrimEnd('<', '>');
            }
            MetaType metaType = MetaTypeRegistry.typeRegistry.Values.FirstOrDefault(t => t.typeName.Equals(arg));
            if (!metaType.Equals(MetaType.Invalid))
            {
                typeIndex = metaType.typeIndex;
                return true;
            }
            throw new KeyNotFoundException($"Type '{arg}' was not found and does not exist in the registry.");
        }
        public static int GetTypeIndex(this string typeName)
        {
            MetaType type = MetaTypeRegistry.typeRegistry.Values.FirstOrDefault(t => t.typeName.Equals(typeName));
            if (!type.Equals(MetaType.Invalid))
                return type.typeIndex;
            throw new KeyNotFoundException($"Type '{typeName}' was not found and does not exist in the registry.");
        }
        public static string GetTypeName(this int typeIndex) => MetaTypeRegistry.typeRegistry[typeIndex].typeName;
        public static string GetTypeName(IData data) => MetaTypeRegistry.typeRegistry[data.typeIndex].typeName;
        // ToDo: Add overloaded extension method for string to lookup typeIndex by typename
        //      This will most-likely be extended to handle aliases or provide additional overload which does do (named: GetAliasedTypeIndex()
        public static int GetTypeIndex(this Type type) => (int)MetaTypeRegistry.underlyingTypesMap.First(x => x.Value.Equals(type)).Key;
    }

}

