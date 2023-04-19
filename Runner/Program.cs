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
        /*
        Memory.Declare("x", Memory.Alloc(typeof(int), Options.Scalar));
        ScalarData<int> xInt = (ScalarData<int>)Memory.Get("x");
        xInt.assign(1337);

        Memory.Declare("y", Memory.Alloc(typeof(int), Options.Scalar));
        ScalarData<int> yInt = (ScalarData<int>)Memory.Get("y");
        yInt.assign(7331);

        Console.WriteLine(xInt);

        LibMath math = Library.Find<LibMath>();
        MethodRef addMethod = null;
        Library.TryGet("LibMath::Add", out addMethod, MethodBindings.Static);
        IData addResult = addMethod.Invoke(null, xInt, yInt); // execute method

        Library.TryGet("Int32::Add", out addMethod);
        addMethod.Invoke(xInt, yInt); // execute method

        Console.WriteLine("x => " + xInt);
        Console.WriteLine("y => " + yInt);


        Memory.Declare("z", Memory.Alloc(typeof(int), Options.Scalar));
        ScalarData<int> zInt = (ScalarData<int>)Memory.Get("z");
        Memory.Assign(zInt, xInt);
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
        */


        //MethodExtraction.ExtractMethod<string>("Concat", BindingFlags.Static | BindingFlags.Public, typeof(string), typeof(string));
        //MethodExtraction.ExtractMethod<string>("Replace", BindingFlags.Instance | BindingFlags.Public, typeof(string), typeof(string));
        //MethodExtraction.ExtractMethod<string>("ToLowerInvariant", BindingFlags.Instance | BindingFlags.Public);

        string code = "Int32 x = myInt.Add(\"Hello\", \"World!\");";
        code = "Int32 x = myInt.Add(\"sads\", y, myInt2.add(z, 3, anotherInt.add(4, 5)), \"(in, parenthesis)\", 'c', 10, anotherInt.add(6, 7));";
        code = "Int32 x = myInt.Add('t', \"(in, parenthesis\", 'c', 10, y, myInt2.add(z, 3, anotherInt.add(4, 5), w), v);";
        code = "Int32 tmp = myInt.Add(1, 1);";
        code = "Int32 tmp = LibMath::Add(1, -10);";
        code = "Int32 tmp = tmp.Add(1, 3);"
                    + Environment.NewLine +
            "Int32 tmp2 = LibMath::Add(tmp, 12);";

        List<Action> codeCall = JIT.CompileML(code);

        foreach (Action a in codeCall)
            a.Invoke();
        Console.WriteLine();

        Console.WriteLine(Memory.Get("tmp"));
        Console.WriteLine(Memory.Get("tmp2"));

        Console.WriteLine();    
        Console.WriteLine();
    }
    

}