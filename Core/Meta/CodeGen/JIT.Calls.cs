using System;
using System.Collections.Generic;
using System.Linq;

namespace NETGraph.Core.Meta.CodeGen
{

    public enum CallInfoType
    {
        Unknown,
        Value,
        Ref,
        Method,

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
            this.arg = arg;
        }

        public override string ToString()
        {
            return $"[{type}]".PadRight(10) + $"{depth}".PadRight(3) + $"{index};".PadRight(4) + $"{new string(' ', depth * 2)} [{this.arg}]";
        }
    }
    //public struct MethodHandle
    //{
    //    // ToDo: Implement type which can resolve the method name from a code line
    //    //      It should also resolve static method name paths and reference name paths (resolving some sort of TypeIndexMethodProvider or add a nested dictionary storing method references for typeIndex)
    //}

    public class JIT
    {


        public static CallInfoType GetCallInfoType(string arg)
        {
            if (arg.StartsWith('"') || arg.StartsWith('\'') || arg.StartsWith("-") || char.IsDigit(arg[0]))
            {
                return CallInfoType.Value;
            }
            if (arg.EndsWith("("))
            {
                return CallInfoType.Method;
            }
            if (char.IsLetter(arg[0]))
            {
                return CallInfoType.Ref;
            }
            return CallInfoType.Unknown;

        }
        public static void GetCallInfos(string input, List<CallInfo> callInfos, char delim = ',', char lhDel = '(', char rhDel = ')', int depth = 0)
        {
            int startIndex = input.IndexOf(lhDel) + 1;
            int endIndex = input.LastIndexOf(rhDel);

            CallInfoType argType = GetCallInfoType(input);
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

        [Flags()]
        public enum CompiledFlags
        {
            None = 0,
            Assign = 1, // includes assignent to variable
            Declare = 2, // inncludes allocation of named variable of type
            Call = 4, // includes a call which is executed
        }

        public static Action<IMemory> Compile(string code)
        {
            code = code.Trim();
            CompiledFlags flags = CompiledFlags.None;
            string assignName = string.Empty;
            string declareType = string.Empty;

            int curI = code.IndexOf("=");
            string lh = string.Empty;
            string rh = code;

            if (curI != -1)
            {
                // Add assignnment to flags
                flags |= CompiledFlags.Assign;
                lh = code.Substring(0, curI).Trim();
                rh = code.Substring(curI).Trim().Trim('=', ' ');
            }
            List<CallInfo> argInfos = new List<CallInfo>();
            bool isDeclaration = lh.Count(x => x == ' ') == 1 ? true : false;
            if (isDeclaration)
            {
                // Add declaration to flags
                flags |= CompiledFlags.Declare;
                int lhInd = lh.IndexOf(' ');
                assignName = (isDeclaration ? lh.Substring(lhInd) : lh).Trim();
                declareType = (isDeclaration ? lh.Substring(0, lhInd) : typeof(void).Name).Trim();
                //Console.WriteLine($"Declare: {isDeclaration}; Data of {type} called '{name}'; {lh} {rh}");

                //int depth = 0;
                //if (!type.Equals(typeof(void).Name))
                //{
                //    // ToDo: Covnert type to typeIndex to handle as integer value onward
                //    // Add alloc method call to create new data variable with given name of given type
                //    argInfos.Add(new CallInfo(argInfos.Count, depth, $"LibCore::Alloc("));
                //    argInfos.Add(new CallInfo(argInfos.Count, depth + 1, $"\"{name}\""));
                //    argInfos.Add(new CallInfo(argInfos.Count, depth + 1, $"\"{type}\""));
                //    depth += 1;
                //}
                //// Add assign method call to store the rh call to the data variable with the given name
                //argInfos.Add(new CallInfo(argInfos.Count, depth, $"LibCore::Assign("));
                //argInfos.Add(new CallInfo(argInfos.Count, depth + 1, $"\"{name}\""));
                //depth += 1;
            }
            JIT.GetCallInfos(rh, argInfos, ',', '(', ')', 0);
            if (argInfos.Count > 0)
                flags |= CompiledFlags.Call;
            Console.WriteLine($"{flags}{Environment.NewLine}{assignName} as new {declareType}{Environment.NewLine}" +
                $"{string.Join(Environment.NewLine + "", argInfos)}");

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

