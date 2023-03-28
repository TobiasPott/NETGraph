using System;
using NETGraph.Core.Meta;
using System.Collections.Generic;
using System.IO;

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
        public static bool IsFirstPathSegment(this IMethodList methodList, string path, out string remainingPath)
        {
            if (path.StartsWith(methodList.Name + "::"))
            {
                remainingPath = path.Remove(0, methodList.Name.Length + 2);
                return true;
            }
            remainingPath = string.Empty;
            return false;
        }
    }

    public class MethodList : IMethodList
    {
        private string name = string.Empty;
        private Dictionary<string, MethodRef> methods = new Dictionary<string, MethodRef>();
        private List<MethodList> nestedMethods = new List<MethodList>();

        public string Name { get => name; }

        public MethodList(string name, params MethodList[] nested)
        {
            this.name = name;
            if (nested != null)
                nestedMethods.AddRange(nested);
        }

        public bool Contains(string path, bool traverse = false)
        {
            // ToDo: Check if Contains is performing same logic as TryGet(.., ..)
            if (traverse)
            {
                if (this.IsFirstPathSegment(path, out string remaininngPath))
                {
                    foreach (MethodList list in nestedMethods)
                        if (list.Contains(remaininngPath, traverse))
                            return true;
                }
            }
            return methods.ContainsKey(path);
        }
        public bool TryGet(string path, out MethodRef method)
        {
            foreach (MethodList list in nestedMethods)
                if (list.IsFirstPathSegment(path, out string remainingPath))
                    if (list.TryGet(remainingPath, out method))
                        return true;
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

