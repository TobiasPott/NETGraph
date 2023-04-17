using System;
using System.Collections.Generic;
using System.Linq;
using NETGraph.Core.BuiltIn;

namespace NETGraph.Core.Meta.CodeGen
{

    public class JIT
    {
        private static bool IsValueInfo(string arg) => (arg.StartsWith('"') || arg.StartsWith('\'') || arg.StartsWith("-") || char.IsDigit(arg[0]));
        private static bool IsRefInfo(string arg) => (char.IsLetter(arg[0]));
        private static bool IsMethodInfo(string arg) => (arg.EndsWith("("));
        private static bool IsAssignInfo(string arg) => (arg.Contains("="));

        // ToDo: Ponder about implementation to determine all existing options in arg, to reduce repeating checks
        public static CallInfoType GetCallInfoType(string arg)
        {
            if (IsValueInfo(arg))
            {
                return CallInfoType.Value;
            }
            if (IsMethodInfo(arg))
            {
                return CallInfoType.Method;
            }
            if (IsRefInfo(arg))
            {
                return CallInfoType.Ref;
            }
            if (IsAssignInfo(arg))
            {
                return CallInfoType.Assign;
            }
            return CallInfoType.Unknown;

        }
        public static void GetDeclareOrAssign(string input, List<CallInfo> callInfos, ref int depth)
        {
            bool isAssignment = input.EndsWith('=');
            input = input.TrimEnd('=', ' ');
            int isDeclaration = input.LastIndexOf(' ');

            if (isDeclaration != -1)
                callInfos.Add(CallInfo.Declare(callInfos.Count, depth, $"{input}"));
            if (isAssignment)
            {
                // assignment exists and needs to lookup a reference to assign to
                callInfos.Add(CallInfo.Assign(callInfos.Count, depth, $"{input.Substring(isDeclaration + 1)}"));
                depth++;
            }
        }
        public static void GetCallInfos(string input, List<CallInfo> callInfos, char delim = ',', char lhDel = '(', char rhDel = ')', int depth = 0)
        {
            int startIndex = input.IndexOf(lhDel) + 1;
            int endIndex = input.LastIndexOf(rhDel);

            CallInfoType argType = GetCallInfoType(input);
            // ToDo: Ponder about including checking for 'Assign' typee within this method and make it non-internal
            if (argType == CallInfoType.Value)
            {
                callInfos.Add(new CallInfo(callInfos.Count, depth, input));
                return;
            }
            if (startIndex > 0)
            {
                callInfos.Add(new CallInfo(callInfos.Count, depth, input.Substring(0, startIndex)));
                depth += 1;
            }
            if (startIndex != -1 && endIndex != -1)
            {
                string argList = input.Substring(startIndex, endIndex - startIndex);
                int splitStart = 0;
                int splitIndex = -1;

                do
                {
                    int strSplit = argList.IndexOf('"', '(', ')', splitStart);
                    int charSplit = argList.IndexOf('\'', '(', ')', splitStart);
                    splitIndex = argList.IndexOf(',', '(', ')', splitStart);

                    if (strSplit != -1 && (splitIndex == -1 || splitIndex > strSplit)
                                        && (charSplit == -1 || charSplit > strSplit))
                    {
                        // string is indicated before next arg
                        splitIndex = argList.IndexOf('"', strSplit + 1) + 1;
                        string subArg = argList.Substring(strSplit, splitIndex - strSplit).Trim();
                        callInfos.Add(new CallInfo(callInfos.Count, depth, subArg));
                        splitStart = splitIndex + 1;
                    }
                    else if (charSplit != -1 && (splitIndex == -1 || splitIndex > charSplit)
                                            && (strSplit == -1 || strSplit > charSplit))
                    {
                        // char is indicated before next arg
                        splitIndex = argList.IndexOf('\'', charSplit + 1) + 1;
                        string subArg = argList.Substring(charSplit, splitIndex - charSplit).Trim();
                        callInfos.Add(new CallInfo(callInfos.Count, depth, subArg));
                        splitStart = splitIndex + 1;
                    }
                    else
                    {
                        if (splitIndex != -1)
                        {
                            string subArg = argList.Substring(splitStart, splitIndex - splitStart).Trim();
                            if (!subArg.Contains(lhDel) && !subArg.Contains(rhDel))
                                callInfos.Add(new CallInfo(callInfos.Count, depth, subArg));
                            else
                                GetCallInfos(subArg, callInfos, delim, lhDel, rhDel, depth + 1);
                            splitStart = splitIndex + 1;
                        }
                        else
                        {
                            string subArg = argList.Substring(splitStart).Trim();
                            if (!subArg.Contains(lhDel) && !subArg.Contains(rhDel))
                                callInfos.Add(new CallInfo(callInfos.Count, depth, subArg));
                            else
                                GetCallInfos(subArg, callInfos, delim, lhDel, rhDel, depth + 1);
                        }
                    }

                }
                while (splitIndex != -1);
            }
        }

        public static Func<IData> Compile(string code)
        {
            code = code.Trim();
            string assignName = string.Empty;
            string declareType = string.Empty;

            int curI = code.IndexOf("=");
            string lh = string.Empty;
            string rh = code;

            if (curI != -1)
            {
                // Add assignnment to flags
                lh = code.Substring(0, curI + 1).Trim();
                rh = code.Substring(curI).Trim().Trim('=', ' ');
            }

            int depth = 0;
            List<CallInfo> argInfos = new List<CallInfo>();
            JIT.GetDeclareOrAssign(lh, argInfos, ref depth);
            JIT.GetCallInfos(rh, argInfos, ',', '(', ')', depth);


            for (int i = 0; i < argInfos.Count; i++)
            {
                string resolved = string.Empty;
                if (argInfos[i].resolve(out IData data))
                    resolved = data.ToString();
                else if (argInfos[i].resolve(out MethodRef method, out IData reference))
                    resolved = method.ToString();
                Console.WriteLine(argInfos[i] + "\t => " + resolved);
            }

            // ToDo: Implement Declare and Assign call info
            //      Consider make it similar to the BuildMethod call
            if (FindNextMethod(argInfos, -1, out int nextIndex))
            {
                Func<IData> call = BuildMethod(argInfos, nextIndex, FindArgsEnd(argInfos, nextIndex), 0);
                return call;
            }

            return null;
        }

        public static int FindArgsEnd(List<CallInfo> callInfos, int methodIndex)
        {
            CallInfo info = callInfos[methodIndex];
            int depth = info.depth;
            int endIndex = methodIndex;
            for (int i = methodIndex + 1; i < callInfos.Count; i++)
            {
                CallInfo subInfo = callInfos[i];
                if (subInfo.depth <= info.depth)
                    return i - 1;

            }
            return callInfos.Count - 1;
        }
        public static bool FindNextMethod(List<CallInfo> callInfos, int startIndex, out int nextIndex)
        {
            for (int i = startIndex + 1; i < callInfos.Count; i++)
            {
                CallInfo subInfo = callInfos[i];
                if (subInfo.type == CallInfoType.Method)
                {
                    nextIndex = i;
                    return true;
                }
            }
            nextIndex = -1;
            return false;
        }

        public static Func<IData> BuildMethod(List<CallInfo> callInfos, int methodIndex, int endIndex, int depth)
        {
            if (callInfos[methodIndex].resolve(out MethodRef handle, out IData reference))
            {
                int argCount = endIndex - methodIndex;
                if (argCount > 0)
                {
                    Func<IData>[] argsCall = new Func<IData>[argCount];
                    for (int i = 0; i < argCount; i++)
                    {
                        int infoIndex = methodIndex + 1 + i;
                        CallInfo argInfo = callInfos[infoIndex];
                        if (argInfo.type == CallInfoType.Method)
                        {
                            int subEndIndex = FindArgsEnd(callInfos, infoIndex);
                            argsCall[i] = BuildMethod(callInfos, infoIndex, subEndIndex, argInfo.depth);
                        }
                        if (argInfo.type == CallInfoType.Ref || argInfo.type == CallInfoType.Value)
                        {
                            if (argInfo.resolve(out IData value))
                                argsCall[i] = () => value;
                        }
                    }
                    Func<IData> call = () =>
                    {
                        IData[] args = new IData[argsCall.Length];
                        for (int i = 0; i < args.Length; i++)
                        {
                            args[i] = argsCall[i].Invoke();
                            Console.WriteLine("\tArg: " + i);
                        }
                        return handle.Invoke(reference, args);
                    };
                    return call;
                }
                else
                {
                    Func<IData> call = () =>
                    {
                        return handle.Invoke(reference);
                    };
                    return call;
                }
            }
            return null;
        }


    }


    public static class JITExtensions
    {
        public static int IndexOf(this string input, char value, char lhDelim, char rhDelim, int startIndex = 0)
        {
            int depth = 0;
            for (int i = startIndex; i < input.Length; i++)
            {
                char c = input[i];
                if (c == value && depth == 0)
                    return i;
                else if (c == lhDelim)
                    depth++;
                else if (c == rhDelim)
                    depth--;
            }
            return -1;
        }
        public static int LastIndexOf(this string input, char value, char lhDelim, char rhDelim, int startIndex = 0)
        {
            int depth = 0;
            for (int i = input.Length - 1 - startIndex; i >= 0; i--)
            {
                char c = input[i];
                if (c == value && depth == 0)
                    return i;
                else if (c == lhDelim)
                    depth++;
                else if (c == rhDelim)
                    depth--;
            }
            return -1;
        }

    }

}

