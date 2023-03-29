using System;
using NETGraph.Core.Meta;
using System.Collections.Generic;
using System.Linq;

namespace NETGraph.Core.BuiltIn
{

    public abstract class LibBase : IComparable<LibBase>
    {
        protected bool loaded = false;
        public string Name { get; protected set; }
        public string path { get; protected set; }

        private MethodList methods;
        protected MethodList Methods
        {
            get
            {
                if (methods == null)
                    methods = new MethodList(Library.METHODS_LIBRARYROOT);
                return methods;
            }
        }


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
                bool intern = LoadInternal();
                Library.LoadLibraryMethods(this.methods);
                loaded = intern;
                Console.WriteLine("Library loaded: " + this.GetType() + $"({(loaded ? "yes" : "no")})");
            }
            return loaded;
        }


        /**
         * IMethodList and related methods
         **/
        public MethodList GetMethods() => methods;

        //public bool Contains(string path, bool traverse = false)
        //{
        //    return methods != null && methods.Contains(path, traverse);
        //}

        //public bool TryGet(string path, out MethodRef method)
        //{
        //    method = null;
        //    return methods != null && methods.TryGet(path, out method);
        //}

    }

}

