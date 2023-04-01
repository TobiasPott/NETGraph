using System;
using System.Collections.Generic;
using System.Linq;
using NETGraph.Core.Meta;

namespace NETGraph.Core.BuiltIn
{
    public class Library
    {
        public const string METHODS_SYSTEMROOT = "_SysRoot";
        public const string METHODS_LIBRARYROOT = "_LibRoot";


        private static HashSet<LibBase> libraries = new HashSet<LibBase>();
        private static MethodList methods = null;

        public static void Load<T>() where T : LibBase
        {
            LibBase lib = Activator.CreateInstance<T>();
            if (!(lib != null
                && lib.Load()
                && libraries.Add(lib)))
                Console.Error.WriteLine($"Failed to load library '{typeof(T)}'");
        }

        public static T Find<T>() where T : LibBase
        {
            return (T)libraries.FirstOrDefault(x => x.GetType().Equals(typeof(T)));
        }

        public static void LoadBuiltInLibraries()
        {
            Library.Load<LibCore>();
            Library.Load<LibMath>();
            Library.Load<LibString>();
        }
        public static void LoadLibraryMethods(MethodList libMethods)
        {
            if (methods == null)
                methods = new MethodList(METHODS_SYSTEMROOT);
            methods.Join(libMethods);
        }

        public static bool Contains(string path, MethodBindings bindings = MethodBindings.Default, bool traverse = false) => methods.Contains(path, bindings, traverse);
        public static bool TryGet(string path, out MethodRef method, MethodBindings bindings = MethodBindings.Default) => methods.TryGet(path, bindings, out method);

    }
}

