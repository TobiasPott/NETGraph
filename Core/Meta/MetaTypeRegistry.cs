using System;
using System.Collections.Generic;
using System.Linq;
using NETGraph.Core.BuiltIn;

namespace NETGraph.Core.Meta
{

    public class MetaTypeRegistry
    {
        private static Dictionary<int, MetaType> blueprints = new Dictionary<int, MetaType>();
        private static Dictionary<DataTypes, Type> builtInTypesMap = new Dictionary<DataTypes, Type>();

        public static bool Register(MetaType blueprint)
        {
            if (!blueprints.ContainsKey(blueprint.typeIndex))
            {
                // query for typeIndex of generate new from blueprint running index
                blueprints.Add(blueprint.typeIndex, blueprint);

                if (!builtInTypesMap.ContainsKey(blueprint.dataType))
                    builtInTypesMap.Add(blueprint.dataType, blueprint.type);
                else if (!builtInTypesMap[blueprint.dataType].Equals(blueprint.type))
                    Console.WriteLine($"{blueprint.dataType} is already registered with {builtInTypesMap[blueprint.dataType]} and cannot be registeered again. Try using a different data type for {blueprint.type} or {typeof(Custom)}.");
            }
            return false;
        }


        public static int GetTypeIndex(Type type) => (int)builtInTypesMap.First(x => x.Value.Equals(type)).Key;
        public static Type GetType(int typeIndex) => builtInTypesMap.First(x => x.Key == (DataTypes)typeIndex).Value;


        public static void LoadBuiltInLibraries()
        {
            Console.WriteLine("Loaded: " + LibCore.Instance);
            Console.WriteLine("Loaded: " + LibMath.Instance);
            Console.WriteLine("Loaded: " + LibString.Instance);
        }
    }
}

