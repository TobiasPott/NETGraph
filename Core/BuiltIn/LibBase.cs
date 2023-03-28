using System;
using NETGraph.Core.Meta;
using System.Collections.Generic;
using System.Linq;

namespace NETGraph.Core.BuiltIn
{

    public class Library
    {
        private static HashSet<LibBase> libraries = new HashSet<LibBase>();
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
            //.Get.FirstOrDefault<T>(  x => x.name.Equals(name) && x.path.Equals(path));
        }

        public static void LoadBuiltInLibraries()
        {
            Library.Load<LibCore>();
            Library.Load<LibMath>();
            Library.Load<LibString>();
        }

    }
    public abstract class LibBase : IMethodProvider, IComparable<LibBase>
    {
        protected bool loaded = false;
        public string name { get; protected set; }
        public string path { get; protected set; }

        protected MethodList methods;

        protected LibBase(string name, string path)
        {
            this.name = name;
            this.path = path;
        }
        public int CompareTo(LibBase other)
        {
            int nameCompare = this.name.CompareTo(other.name);
            if (nameCompare == 0)
                return this.path.CompareTo(other.path);
            return nameCompare;
        }

        public IResolver invoke(string accessor, IResolver reference, params IResolver[] inputs)
        {
            if (methods != null && methods.TryGet(accessor, out MethodRef method))
                return method.Invoke(reference, inputs);
            else
                throw new InvalidOperationException($"Method call for {accessor} was not found in {this.GetType()} .");
        }

        protected abstract bool LoadInternal();
        public bool Load()
        {
            if (!loaded)
            {
                loaded = LoadInternal();
                Console.WriteLine("Library loaded: " + this.GetType() + $"({(loaded ? "yes" : "no")})");
            }
            return loaded;
        }
        public void Add(string name, MethodRef method, bool overwrite = false)
        {
            if (methods == null)
                methods = new MethodList(this.GetType().Name);

            name = name.Replace(this.name, string.Empty).Replace(this.path, string.Empty).Trim('.');
            if (methods.Contains(name) || overwrite)
                methods.Set(name, method);
        }

    }
    // ToDo: Implement MethodList which can nest instances of MethodList for nested lookup
    //      Lookup by path segment separated by . compared case sensitive (like .NET)
    public class MethodList
    {
        private string name = string.Empty;
        private Dictionary<string, MethodRef> methods = new Dictionary<string, MethodRef>();
        private List<MethodList> nestedMethods = new List<MethodList>();

        public MethodList(string name, params MethodList[] nested)
        {
            this.name = name;
            if (nested != null)
                nestedMethods.AddRange(nested);
        }

        public bool Contains(string name) => methods.ContainsKey(name);
        public bool TryGet(string path, out MethodRef method)
        {
            int sepIndex = path.IndexOf('.');
            if (sepIndex != -1)
            {
                string nameSeg = path.Substring(0, sepIndex);
                if (this.name.Equals(nameSeg))
                {
                    foreach (MethodList list in nestedMethods)
                        if (list.TryGet(path.Substring(sepIndex + 1), out method))
                            return true;
                }
            }
            return methods.TryGetValue(path, out method);
        }
        public void Set(string methodName, MethodRef method)
        {
            if (!methods.ContainsKey(methodName))
                methods.Add(methodName, method);
            else
                methods[methodName] = method;
        }
    }
}

