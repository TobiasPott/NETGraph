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
    // ToDo: implement a library base type to allow inheriting the registration progress with the meta type system
    public abstract class LibBase : IMethodProvider, IComparable<LibBase>
    {
        protected bool loaded = false;
        public string name { get; protected set; }
        public string path { get; protected set; }
        protected Dictionary<string, Invokation> methods = new Dictionary<string, Invokation>();


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
            if (methods.TryGetValue(accessor, out Invokation invokation))
                return invokation.Invoke(reference, inputs);
            else
                throw new InvalidOperationException($"Method call for {accessor} was not found in {this.GetType()} .");
        }

        public virtual bool Load()
        {
            if (!loaded)
            {
                Console.WriteLine("Library loaded: " + this.GetType());

                loaded = true;
            }
            return loaded;
        }

    }

}

