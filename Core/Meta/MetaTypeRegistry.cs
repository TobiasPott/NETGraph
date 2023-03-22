using System;
using System.Collections.Generic;
using System.Linq;
using NETGraph.Core.BuiltIn;

namespace NETGraph.Core.Meta
{

    public class MetaTypeRegistry
    {
        private static Dictionary<int, MetaType> blueprints = new Dictionary<int, MetaType>();
        private static Dictionary<int, Type> builtInTypesMap = new Dictionary<int, Type>();

        public static bool Register(MetaType blueprint)
        {
            if (!blueprints.ContainsKey(blueprint.typeIndex))
            {
                // query for typeIndex of generate new from blueprint running index
                blueprints.Add(blueprint.typeIndex, blueprint);

                if (!builtInTypesMap.ContainsKey(blueprint.typeIndex))
                    builtInTypesMap.Add(blueprint.typeIndex, blueprint.type);
                else if (!builtInTypesMap[blueprint.typeIndex].Equals(blueprint.type))
                    Console.WriteLine($"{blueprint.typeIndex} is already registered with {builtInTypesMap[blueprint.typeIndex]} and cannot be registeered again. Try using a different data type for {blueprint.type} or {typeof(Custom)}.");
            }
            return false;
        }


        public static int GetTypeIndex(Type type) => (int)builtInTypesMap.First(x => x.Value.Equals(type)).Key;
        public static Type GetType(int typeIndex) => builtInTypesMap.First(x => x.Key == typeIndex).Value;


    }
}

