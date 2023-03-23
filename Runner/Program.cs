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


        Memory.Assign("x", Memory.Alloc(typeof(int), Options.Scalar));
        ScalarData<int> xInt = (ScalarData<int>)Memory.Get("x");
        xInt.assign(1337);

        Memory.Assign("y", Memory.Alloc(typeof(int), Options.Scalar));
        ScalarData<int> yInt = (ScalarData<int>)Memory.Get("y");
        yInt.assign(7331);

        Console.WriteLine(xInt);

        // ToDo: resolve wacky new LibMath() to create instance by looking up library by name/type
        IResolver addResult = Library.Find<LibMath>().invoke("add", null, xInt, yInt); // execute method
        Console.WriteLine("add => " + addResult.resolve<int>());

        Console.WriteLine("x => " + xInt);
        Console.WriteLine("y => " + yInt);


        Memory.Assign("z", Memory.Alloc(typeof(int), Options.Scalar));
        ScalarData<int> zInt = (ScalarData<int>)Memory.Get("z");
        LibCore.assign(zInt, xInt);
        Console.WriteLine("z => " + zInt);


        Memory.Assign("w", Memory.Alloc(typeof(int), Options.Scalar));
        ScalarData<int> wInt = (ScalarData<int>)Memory.Get("w");
        wInt.assign(xInt as IResolver);
        Console.WriteLine("w => " + wInt);

        Memory.Assign("ints", Memory.Alloc(typeof(int), Options.Index));
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
    }

    public static void TimeStamp(Stopwatch sw, string additional)
    {
        Console.WriteLine(sw.Elapsed.Ticks.ToString("0000000") + ": " + additional);

    }

}