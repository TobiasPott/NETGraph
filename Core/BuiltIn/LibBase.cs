using System;
using NETGraph.Core.Meta;
using System.Collections.Generic;
using System.Linq;

namespace NETGraph.Core.BuiltIn
{

    public abstract class LibBase : IComparable<LibBase>, IMethodList
    {
        protected bool loaded = false;
        public string Name { get; protected set; }
        public string path { get; protected set; }

        protected MethodList methods;

        protected LibBase(string name, string path)
        {
            this.Name = name;
            this.path = path;
        }
        public int CompareTo(LibBase other)
        {
            int nameCompare = this.Name.CompareTo(other.Name);
            if (nameCompare == 0)
                return this.path.CompareTo(other.path);
            return nameCompare;
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

        public void AddMethod(string name, MethodRef method, bool overwrite = false)
        {
            if (methods == null)
                methods = new MethodList(this.GetType().Name);

            name = name.Replace(this.Name, string.Empty).Replace(this.path, string.Empty).Trim('.');
            if (methods.Contains(name) || overwrite)
                methods.Set(name, method);
        }

        public bool Contains(string path, bool traverse = false)
        {
            return methods.Contains(path, traverse);
        }

        public bool TryGet(string path, out MethodRef method)
        {
            method = null;
            return methods != null && methods.TryGet(path, out method);
        }

    }

}

