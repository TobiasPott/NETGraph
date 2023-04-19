using System;
using NETGraph.Core.BuiltIn;
using NETGraph.Core.Meta;
using NETGraph.Core.Meta.CodeGen;

namespace NETGraph.Core.Meta.CodeGen
{
    public enum CallInfoType
    {
        Unknown,
        Value, // const data 
        Ref, // ref to data variable in memory
        Method, // info for method call (reference, static and name)
        Alloc, // allocation of new data type of give type
        Declare,
        Assign
        // ToDo:    Add call info like scope (as first structure element)
        //          Add call info for branch (req. scope first)
    }
    public struct CallInfo
    {

        public int index;
        public int depth;
        public string arg;
        public CallInfoType type;

        public CallInfo(int index, int depth, string arg)
        {
            this.index = index;
            this.depth = depth;
            this.type = JIT.GetCallInfoType(arg);
            this.arg = arg.TrimEnd('(');
        }
        public static CallInfo Declare(int index, int depth, string arg)
        {
            // new declaration call info with the variable's name as argument
            return new CallInfo(index, depth, arg) { type = CallInfoType.Declare };
        }
        public static CallInfo Alloc(int index, int depth, string arg)
        {
            // new allocation call info with the variable's type name as argument
            return new CallInfo(index, depth, arg) { type = CallInfoType.Alloc };
        }
        public static CallInfo Assign(int index, int depth, string arg)
        {
            // new assignment call info with target variable's name as argument
            return new CallInfo(index, depth, arg) { type = CallInfoType.Assign };
        }

        // used to resolve CallInfo to value or reference
        public bool resolve(out IData value)
        {
            if (type == CallInfoType.Value)
            {
                if (arg.StartsWith('"'))
                {
                    value = new ValueData<string>(arg.Trim('"'));
                    return true;
                }
                if (arg.StartsWith('\''))
                {
                    value = new ValueData<char>(arg.Trim('\'')[0]);
                    return true;
                }
                if (char.IsDigit(arg[0]) || arg.StartsWith("-"))
                {
                    if (arg.Contains('.'))
                        value = new ValueData<float>(float.Parse(arg));
                    else
                        value = new ValueData<int>(int.Parse(arg));
                    return true;
                }
            }
            else if (type == CallInfoType.Alloc)
            {
                if (arg.GetAllocInfo(out int typeIndex, out Options options))
                {
                    value = Memory.Alloc(typeIndex, options);
                    return true;
                }
            }

            value = null;
            return false;
        }
        // used to resolve CallInfo to optional reference and method handle
        public bool resolve(out MethodRef handle, out MethodRef referenceHandle)
        {
            if (type == CallInfoType.Method)
            {
                if (arg.Contains(IMethodListExtension.PATHSEPARATOR))
                {
                    referenceHandle = null;
                    return Library.TryGet(arg, out handle, MethodBindings.Static);
                }
                else
                {
                    int splitIndex = arg.IndexOf('.');
                    string methodName = arg.Substring(splitIndex + 1);
                    string typeName = JIT.GetTypeName(arg.Substring(0, splitIndex));
                    // ToDo: need extra resolve for MethodReef to retrieve the Reference
                    referenceHandle = MethodRefExtensions.Get(arg.Substring(0, splitIndex));
                    return Library.TryGet($"{typeName}::{methodName}", out handle, MethodBindings.Instance);
                }
            }
            throw new InvalidOperationException($"Cannot resolve method and/or reference from {type} info.");
        }
        public bool resolve(out MethodRef handle)
        {
            if (type == CallInfoType.Ref)
            {
                // try to recieve the given reference from memory
                handle = MethodRefExtensions.Get(arg);
                return true;
            }
            else if (type == CallInfoType.Declare)
            {
                if (arg.GetDeclareInfo(out int typeIndex, out string name, out Options options))
                {
                    handle = MethodRefExtensions.Declare(typeIndex, name, options);
                    return true;
                }
            }
            else if (type == CallInfoType.Assign)
            {
                handle = MethodRefExtensions.Assign(arg);
                return handle != null;
            }
            throw new InvalidOperationException($"Cannot resolve method and/or reference from {type} info.");
        }

        public override string ToString()
        {
            return $"[{type}]".PadRight(10) + $"{depth}".PadRight(3) + $"{index};".PadRight(4) + $"{new string(' ', depth * 2)} [{this.arg}]";
        }
    }

}

