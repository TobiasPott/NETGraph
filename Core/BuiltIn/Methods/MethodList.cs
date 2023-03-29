using System;
using NETGraph.Core.Meta;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NETGraph.Core.BuiltIn
{

    public interface IMethodList
    {
        string Name { get; }
        bool Contains(string path, bool traverse = false);
        bool TryGet(string path, out MethodRef method);
    }

    public static class IMethodListExtension
    {
        public const string PATHSEPARATOR = "::";

        public static bool IsFirstPathSegment(this IMethodList methodList, string path, out string remainingPath)
        {
            if (path.StartsWith(methodList.Name + PATHSEPARATOR))
            {
                remainingPath = path.Remove(0, methodList.Name.Length + 2);
                return true;
            }
            remainingPath = string.Empty;
            return false;
        }
    }

    [Flags()]
    public enum MethodBindings
    {
        None = 0,
        Default = Instance,
        Instance = 1,
        Static = 2,
        Operator = 4, // special case for future magic methods
    }
    public struct MethodHandle
    {
        public string name { get; private set; }
        public MethodBindings binding { get; private set; }
        public MethodRef method { get; private set; }

        public MethodHandle(string name, MethodRef method, MethodBindings binding)
        {
            this.name = name;
            this.method = method;
            this.binding = binding;
        }

        public bool Match(string name, MethodBindings bindings)
        {
            if (binding == MethodBindings.None)
                return this.name.Equals(name);

            // ToDo: Reconsider this comparison, might be better to do OR or reverse OR on argument to ensure easier search
            if (this.binding == bindings)
                return this.name.Equals(name);
            return false;
        }
    }

    public class MethodList : IMethodList
    {
        private string name = string.Empty;
        private Dictionary<string, MethodRef> methods = new Dictionary<string, MethodRef>();
        private List<MethodList> nestedMethods = new List<MethodList>();
        private bool isRoot = false;
        public string Name { get => name; }

        internal MethodList(string name)
        {
            this.name = name;
            this.isRoot = true;
        }
        public MethodList(string name, params MethodList[] nested)
        {
            this.name = name;
            if (nested != null)
                nestedMethods.AddRange(nested);
        }

        public bool Contains(string path, bool traverse = false)
        {
            if (traverse)
            {
                foreach (MethodList list in nestedMethods)
                    if (list.IsFirstPathSegment(path, out string remainingPath))
                        if (list.Contains(remainingPath, traverse))
                            return true;
            }
            return methods.ContainsKey(path);
        }
        public bool TryGet(string path, out MethodRef method)
        {
            if (path.Contains(IMethodListExtension.PATHSEPARATOR))
                foreach (MethodList list in nestedMethods)
                    if (list.IsFirstPathSegment(path, out string remainingPath))
                        if (list.TryGet(remainingPath, out method))
                            return true;
            return methods.TryGetValue(path, out method);
            }
        public void Set(MethodHandle handle)
        {
            if (!isRoot)
            {
                if (!methods.ContainsKey(handle.name))
                    methods.Add(handle.name, handle.method);
                else
                    methods[handle.name] = handle.method;
            }
            else
                throw new InvalidOperationException($"Cannot add {handle.method} to root list. Add to a nested list instead.");
        }
        public void Nest(MethodList methodList)
        {
            if (!nestedMethods.Contains(methodList))
                nestedMethods.Add(methodList);
        }
        /// <summary>
        /// Joins the given list with the current instance
        /// </summary>
        /// <param name="other"></param>
        public void Join(MethodList other)
        {
            // ignore name when copying local
            if (!this.isRoot)
                foreach (KeyValuePair<string, MethodRef> kvPair in other?.methods)
                    this.methods.TryAdd(kvPair.Key, kvPair.Value);

            if (other != null && other.nestedMethods != null)
                foreach (MethodList otherSubList in other.nestedMethods)
                {
                    bool found = false;
                    foreach (MethodList subList in this.nestedMethods)
                    {
                        if (subList.Name.Equals(otherSubList.Name))
                        {
                            subList.Join(otherSubList);
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                        this.nestedMethods.Add(otherSubList);
                }
        }
    }
}

