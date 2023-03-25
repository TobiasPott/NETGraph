using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NETGraph;
using NETGraph.Core;
using NETGraph.Core.BuiltIn;
using NETGraph.Core.Meta;
using NETGraph.Core.Meta.CodeGen;
using NETGraph.Graphs;


public class Program
{

    public static void Main(string[] args)
    {
        Library.LoadBuiltInLibraries();

        /*
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3i vec3IntData = new Vector3i(1, 10, 0);
        LibMath libMath = LibMath.Instance;


        // binding to actual data objeect => reesults in resolvable DataQuery
        TimeStamp(sw, vec3IntData.ToString());

        IEnumerable<IResolver> inputs = vec3IntData.z.AsEnumerable<IResolver>().Append(vec3IntData.x);
        libMath.assign("add", null, vec3IntData.z, inputs); // execute method
        TimeStamp(sw, vec3IntData.ToString());

        vec3IntData.x.assign(5);
        for (int i = 0; i < 10; i++)
            libMath.assign("add", null, vec3IntData.z, inputs); // execute method

        TimeStamp(sw, vec3IntData.ToString());

        vec3IntData.x.assign(12);
        for (int i = 0; i < 5; i++)
            libMath.assign("add", null, vec3IntData.z, inputs); // execute method
        TimeStamp(sw, vec3IntData.ToString());
        */

        // runner test code to create a new 'named' data of data type Int


        Memory.Store("x", Memory.Alloc(typeof(int), Options.Scalar));
        ScalarData<int> xInt = (ScalarData<int>)Memory.Get("x");
        xInt.assign(1337);

        Memory.Store("y", Memory.Alloc(typeof(int), Options.Scalar));
        ScalarData<int> yInt = (ScalarData<int>)Memory.Get("y");
        yInt.assign(7331);

        Console.WriteLine(xInt);

        LibMath math = Library.Find<LibMath>();
        IResolver addResult = math.invoke("add", null, xInt, yInt); // execute method
        Console.WriteLine("add => " + addResult.resolve<int>());

        Console.WriteLine("x => " + xInt);
        Console.WriteLine("y => " + yInt);


        Memory.Store("z", Memory.Alloc(typeof(int), Options.Scalar));
        ScalarData<int> zInt = (ScalarData<int>)Memory.Get("z");
        LibCore.assign(zInt, xInt);
        Console.WriteLine("z => " + zInt);


        Memory.Store("w", Memory.Alloc(typeof(int), Options.Scalar));
        ScalarData<int> wInt = (ScalarData<int>)Memory.Get("w");
        wInt.assign(xInt as IResolver);
        Console.WriteLine("w => " + wInt);

        Memory.Store("ints", Memory.Alloc(typeof(int), Options.Index));
        IndexedData<int> intsList = (IndexedData<int>)Memory.Get("ints");
        intsList.initializeWith(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        //Memory.Assign("a", Memory.Alloc(typeof(float), Options.Scalar));
        //ScalarData<float> aFloat = (ScalarData<float>)Memory.Get("a");
        //LibCore.assign(aFloat, xInt);
        //Console.WriteLine(aFloat);
        intsList.assign(new DataAccessor("[0]"), wInt);

        zInt.assign(math.invoke("add", null, xInt, yInt));
        Console.WriteLine("add => " + zInt + " // " + xInt + " + " + yInt);

        //MethodExtraction.ExtractMethod<string>("Concat", BindingFlags.Static | BindingFlags.Public, typeof(string), typeof(string));
        //MethodExtraction.ExtractMethod<string>("Replace", BindingFlags.Instance | BindingFlags.Public, typeof(string), typeof(string));
        //MethodExtraction.ExtractMethod<string>("ToLowerInvariant", BindingFlags.Instance | BindingFlags.Public);

        //ParseAllocAndAssign("int32 x = 0;");
        //ParseAllocAndAssign("x = 0;");

        //ParseAllocAndAssign("int x = LibMath::add(y, 'a', \"blubb\", 1, 1.45);"); //, myInt.add(2, 2), 0)");
        //ParseAllocAndAssign("int x = myInt.add(y, 'a', \"blubb\", 1, 1.45);");
        //ParseAllocAndAssign("int x = myInt.add(y, myInt.add(y, 3), 10);");

        //List<ArgInfo> argInfos = new List<ArgInfo>();
        //string code = "int x = myInt.add(\"Hello\", \"World!\");";
        //code = "int x = myInt.add(\"sads\", y, myInt2.add(z, 3, anotherInt.add(4, 5)), \"(in, parenthesis)\", 'c', 10, anotherInt.add(6, 7));";
        //code = "int x = myInt.add('t', \"(in, parenthesis\", 'c', 10, y, myInt2.add(z, 3, anotherInt.add(4, 5), w), v);";
        //ParseAllocAndAssign(code);
        //Console.WriteLine($"\t\t{string.Join(Environment.NewLine + "\t\t", argInfos)}");


        Console.WriteLine();
        Console.WriteLine();
    }

    public enum ArgInfoType
    {
        Unknown,
        Value,
        Ref,
        Method,

    }
    public struct ArgInfo
    {

        public int index;
        public int depth;
        public string arg;
        public ArgInfoType type;

        public ArgInfo(int index, int depth, string arg)
        {
            this.index = index;
            this.depth = depth;
            this.type = GetArgInfoType(arg);
            this.arg = arg;
        }

        public override string ToString()
        {
            return $"[{type}]".PadRight(10) + $"{depth}".PadRight(3) + $"{index};".PadRight(4) + $"{new string(' ', depth * 3)} [{this.arg}]";
        }
    }

    public static ArgInfoType GetArgInfoType(string arg)
    {
        if (arg.StartsWith('"') || arg.StartsWith('\'') || arg.StartsWith("-") || char.IsDigit(arg[0]))
        {
            return ArgInfoType.Value;
        }
        if (arg.EndsWith("("))
        {
            return ArgInfoType.Method;
        }
        if (char.IsAscii(arg[0]))
        {
            return ArgInfoType.Ref;
        }
        return ArgInfoType.Unknown;

    }
    public static void GetArgInfo(string input, List<ArgInfo> arguments, char delim = ',', char lhDel = '(', char rhDel = ')', int depth = 0)
    {
        int startIndex = input.IndexOf(lhDel) + 1;
        int endIndex = input.LastIndexOf(rhDel);

        ArgInfoType argType = GetArgInfoType(input);
        if (argType == ArgInfoType.Value)
        {
            arguments.Add(new ArgInfo(arguments.Count, depth, input));
            return;
        }

        if (startIndex > 0)
        {
            arguments.Add(new ArgInfo(arguments.Count, depth, input.Substring(0, startIndex)));
            depth += 1;
        }

        if (startIndex != -1 && endIndex != -1)
        {
            string argList = input.Substring(startIndex, endIndex - startIndex);
            int splitStart = 0;
            int splitIndex = -1;

            do
            {
                int strSplit = IndexOf(argList, '"', '(', ')', splitStart);
                int charSplit = IndexOf(argList, '\'', '(', ')', splitStart);
                splitIndex = IndexOf(argList, ',', '(', ')', splitStart);

                if (strSplit != -1 && (splitIndex == -1 || splitIndex > strSplit)
                                    && (charSplit == -1 || charSplit > strSplit))
                {
                    // string is indicated before next arg
                    splitIndex = argList.IndexOf('"', strSplit + 1) + 1;
                    string subArg = argList.Substring(strSplit, splitIndex - strSplit).Trim();
                    arguments.Add(new ArgInfo(arguments.Count, depth, subArg));
                    splitStart = splitIndex + 1;
                }
                else if (charSplit != -1 && (splitIndex == -1 || splitIndex > charSplit)
                                        && (strSplit == -1 || strSplit > charSplit))
                {
                    // char is indicated before next arg
                    splitIndex = argList.IndexOf('\'', charSplit + 1) + 1;
                    string subArg = argList.Substring(charSplit, splitIndex - charSplit).Trim();
                    arguments.Add(new ArgInfo(arguments.Count, depth, subArg));
                    splitStart = splitIndex + 1;
                }
                else
                {
                    if (splitIndex != -1)
                    {
                        string subArg = argList.Substring(splitStart, splitIndex - splitStart).Trim();
                        if (!subArg.Contains(lhDel) && !subArg.Contains(rhDel))
                            arguments.Add(new ArgInfo(arguments.Count, depth, subArg));
                        else
                            GetArgInfo(subArg, arguments, delim, lhDel, rhDel, depth + 1);
                        splitStart = splitIndex + 1;
                    }
                    else
                    {
                        string subArg = argList.Substring(splitStart).Trim();
                        if (!subArg.Contains(lhDel) && !subArg.Contains(rhDel))
                            arguments.Add(new ArgInfo(arguments.Count, depth, subArg));
                        else
                            GetArgInfo(subArg, arguments, delim, lhDel, rhDel, depth + 1);
                    }
                }

            }
            while (splitIndex != -1);
        }
    }
    public static int IndexOf(string input, char value, char lhDelim, char rhDelim, int startIndex = 0)
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
    public static int LastIndexOf(string input, char value, char lhDelim, char rhDelim, int startIndex = 0)
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


    public static Action ParseAllocAndAssign(string code)
    {
        code = code.Trim();
        int curI = code.IndexOf("=");
        string lh = string.Empty;
        string rh = code;

        if (curI != -1)
        {
            lh = code.Substring(0, curI).Trim();
            rh = code.Substring(curI).Trim().Trim('=', ' ');
        }
        int depth = 0;
        List<ArgInfo> argInfos = new List<ArgInfo>();
        bool isDeclaration = lh.Count(x => x == ' ') == 1 ? true : false;
        if (isDeclaration)
        {
            int lhInd = lh.IndexOf(' ');
            string name = (isDeclaration ? lh.Substring(lhInd) : lh).Trim();
            string type = (isDeclaration ? lh.Substring(0, lhInd) : typeof(void).Name).Trim();
            Console.WriteLine($"Declare: {isDeclaration}; Data of {type} called '{name}'; {lh} {rh}");

            if (!type.Equals(typeof(void).Name))
            {
                // ToDo: Covnert type to typeIndex to handle as integer value onward
                // Add alloc method call to create new data variable with given name of given type
                argInfos.Add(new ArgInfo(argInfos.Count, depth, $"LibCore::Alloc("));
                argInfos.Add(new ArgInfo(argInfos.Count, depth + 1, $"\"{name}\""));
                argInfos.Add(new ArgInfo(argInfos.Count, depth + 1, $"\"{type}\""));
                depth += 1;
            }
            // Add assign method call to store the rh call to the data variable with the given name
            argInfos.Add(new ArgInfo(argInfos.Count, depth, $"LibCore::Assign("));
            argInfos.Add(new ArgInfo(argInfos.Count, depth + 1, $"\"{name}\""));
            depth += 1;
        }
        GetArgInfo(rh, argInfos, ',', '(', ')', depth);
        Console.WriteLine($"{string.Join(Environment.NewLine + "", argInfos)}");

        return null;
    }

}