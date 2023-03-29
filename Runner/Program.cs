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


        Memory.Declare("x", Memory.Alloc(typeof(int), Options.Scalar));
        ScalarData<int> xInt = (ScalarData<int>)Memory.Get("x");
        xInt.assign(1337);

        Memory.Declare("y", Memory.Alloc(typeof(int), Options.Scalar));
        ScalarData<int> yInt = (ScalarData<int>)Memory.Get("y");
        yInt.assign(7331);

        Console.WriteLine(xInt);

        LibMath math = Library.Find<LibMath>();
        MethodRef addMethod = null;
        Library.TryGet("LibMath::Add", out addMethod);
        IResolver addResult = addMethod.Invoke(null, xInt, yInt); // execute method
        //Console.WriteLine("add => " + addResult.resolve<int>());

        // ToDo: Check why this returs null when trying to resolve nested method lists
        Library.TryGet("int::Add", out addMethod);
        addMethod.Invoke(xInt, yInt); // execute method

        Console.WriteLine("x => " + xInt);
        Console.WriteLine("y => " + yInt);


        Memory.Declare("z", Memory.Alloc(typeof(int), Options.Scalar));
        ScalarData<int> zInt = (ScalarData<int>)Memory.Get("z");
        LibCore.assign(zInt, xInt);
        Console.WriteLine("z => " + zInt);


        Memory.Declare("w", Memory.Alloc(typeof(int), Options.Scalar));
        ScalarData<int> wInt = (ScalarData<int>)Memory.Get("w");
        wInt.assign(xInt as IResolver);
        Console.WriteLine("w => " + wInt);

        Memory.Declare("ints", Memory.Alloc(typeof(int), Options.Index));
        IndexedData<int> intsList = (IndexedData<int>)Memory.Get("ints");
        intsList.initializeWith(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        //Memory.Assign("a", Memory.Alloc(typeof(float), Options.Scalar));
        //ScalarData<float> aFloat = (ScalarData<float>)Memory.Get("a");
        //LibCore.assign(aFloat, xInt);
        //Console.WriteLine(aFloat);
        intsList.assign(new DataAccessor("[0]"), wInt);

        //MethodExtraction.ExtractMethod<string>("Concat", BindingFlags.Static | BindingFlags.Public, typeof(string), typeof(string));
        //MethodExtraction.ExtractMethod<string>("Replace", BindingFlags.Instance | BindingFlags.Public, typeof(string), typeof(string));
        //MethodExtraction.ExtractMethod<string>("ToLowerInvariant", BindingFlags.Instance | BindingFlags.Public);

        //ParseAllocAndAssign("int32 x = 0;");
        //ParseAllocAndAssign("x = 0;");

        //ParseAllocAndAssign("int x = LibMath::add(y, 'a', \"blubb\", 1, 1.45);"); //, myInt.add(2, 2), 0)");
        //ParseAllocAndAssign("int x = myInt.add(y, 'a', \"blubb\", 1, 1.45);");
        //ParseAllocAndAssign("int x = myInt.add(y, myInt.add(y, 3), 10);");

        List<CallInfo> argInfos = new List<CallInfo>();
        string code = "int x = myInt.add(\"Hello\", \"World!\");";
        code = "int x = myInt.add(\"sads\", y, myInt2.add(z, 3, anotherInt.add(4, 5)), \"(in, parenthesis)\", 'c', 10, anotherInt.add(6, 7));";
        code = "int x = myInt.add('t', \"(in, parenthesis\", 'c', 10, y, myInt2.add(z, 3, anotherInt.add(4, 5), w), v);";
        code = "int x = myInt.add(y, z);";
        //code = "myInt.add(y, z);";

        JIT.Compile(code);
        Console.WriteLine($"\t\t{string.Join(Environment.NewLine + "\t\t", argInfos)}");


        Console.WriteLine();
        Console.WriteLine();
    }


}