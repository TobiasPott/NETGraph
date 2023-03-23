using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        // ToDo: resolve wacky new LibMath() to create instance by looking up library by name/type
        IResolver addResult = Library.Find<LibMath>().invoke("add", null, xInt, yInt); // execute method
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
        Console.WriteLine("list => " + intsList);

        // ToDo: Complete a list of methods of the string type to extract code
        //      include method name
        //      binding (public + instance/static)
        //      param type array (list of input types of method)
        MethodExtraction.ExtractMethod<string>("Concat", BindingFlags.Static | BindingFlags.Public, typeof(string), typeof(string));
        MethodExtraction.ExtractMethod<string>("Replace", BindingFlags.Instance | BindingFlags.Public, typeof(string), typeof(string));
        MethodExtraction.ExtractMethod<string>("ToLowerInvariant", BindingFlags.Instance | BindingFlags.Public);

        Console.WriteLine();
        ParseAllocAndAssign("int32 x = 0;");
        ParseAllocAndAssign("x = 0;");

        ParseAllocAndAssign("int x = LibMath::add(y, z)");
        //      (int) x = LibMath::add(y, z);

        Console.WriteLine();
        Console.WriteLine();
    }

    // ToDo: Build map of possible method calls and signatures (e.g. static, instance, operator, nested, assignment, result
    //  Base rules:
    //      if contains '=' call is assignment to left hand side
    //      if '=' is preceeded by two tokens, the call includes allocation of new data of result type which the call result is assigned to
    //      if '=' is preceeded by one token, the call assigns to left hand
    //
    //  Samples: () enclose optional parts, numeric values represent const values, string/chars represent named data
    //      int x; // ToDo; is special cas without = annd should be possible to prepend when other cased are implemented
    //      (int) x = 0;
    //      (int) x = LibMath::add(1, 1)
    //      (int) x = LibMath::add(y, z);
    //      (int) x = v.magnitude();
    //      (int) x = y.offset(z);
    //      (int) x = 1 + 1; // ToDo: operators are special case to parse and map (magic notation to cover up "arg = method(arg, arg)" underlying calls

    public static Action ParseAllocAndAssign(string code)
    {
        code = code.Trim();
        int curI = code.IndexOf("=");
        string lh = code.Substring(0, curI).Trim();
        string rh = code.Substring(curI).Trim();
        bool isDeclaration = lh.Count(x => x == ' ') == 1 ? true : false;

        int lhInd = lh.IndexOf(' ');
        string name = isDeclaration ? lh.Substring(lhInd) : lh;
        string type = isDeclaration ? lh.Substring(0, lhInd) : typeof(void).Name;

        Console.WriteLine($"Declare: {isDeclaration}; Data of {type} called '{name}'; {lh} {rh}");

        // ToDo: check for static method case ::
        int classDelimIndex = rh.IndexOf("::");
        if (classDelimIndex != -1)
        {
            string libName = rh.Substring(0, classDelimIndex);
            string methodCall = rh.Substring(classDelimIndex + 2);
            int leftPInd = methodCall.IndexOf("(");
            int rightPInd = methodCall.LastIndexOf(")");

            string methodName = methodCall.Substring(0, leftPInd);
            Console.WriteLine($"\tCalls: '{methodName}' from {libName} with");

            string args = methodCall.Substring(leftPInd + 1, rightPInd - leftPInd - 1).Trim();
            if (args.Length > 0)
            {
                string[] argsSplit = args.Split(",", StringSplitOptions.RemoveEmptyEntries);
                Console.WriteLine($"\t\t{string.Join(Environment.NewLine + "\t\t", argsSplit)}");
                // ToDo: check args for direct data/value or variable name
                //      check for initial numerical character, always direct value, may include . for floating values
                //      check for "" or ''
                //      IGNORE: special static value cases e.g. LibMath::PI (not existing but will definitely come later on
            }
        }

        return null;
    }

    public static void TimeStamp(Stopwatch sw, string additional)
    {
        Console.WriteLine(sw.Elapsed.Ticks.ToString("0000000") + ": " + additional);

    }

}