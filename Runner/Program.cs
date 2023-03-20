using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NETGraph;
using NETGraph.Core;
using NETGraph.Core.BuiltIn;
using NETGraph.Core.Meta;
using NETGraph.Graphs;


public class Node : IEquatable<Node>
{
    string name = string.Empty;
    List<object> data = new List<object>();

    public Node(string name)
    {
        this.name = name;
    }

    bool IEquatable<Node>.Equals(Node other)
    {
        if (other != null)
            return other.name.Equals(this.name);
        return false;
    }

    public override string ToString()
    {
        return $"{name} ({data.Count})";
    }
}

public static class IEnumerableExt
{
    public static IEnumerable<T> AsEnumerable<T>(this T item)
    {
        yield return item;
    }
}
public class Program
{

    public static object GuidStringNode { get; private set; }

    public static void Main(string[] args)
    {
        MetaTypeRegistry.LoadBuiltInLibraries();

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

        LibMath libMath = LibMath.Instance;
        libMath.assign("add", null, xInt, xInt, yInt); // execute method

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

        Memory.Assign("ints", Memory.Alloc(typeof(int), Options.List));
        IndexedData<int> intsList = (IndexedData<int>)Memory.Get("ints");
        //Memory.Assign("a", Memory.Alloc(typeof(float), Options.Scalar));
        //ScalarData<float> aFloat = (ScalarData<float>)Memory.Get("a");
        //LibCore.assign(aFloat, xInt);
        //Console.WriteLine(aFloat);

        Console.WriteLine();
    }

    public static void TimeStamp(Stopwatch sw, string additional)
    {
        Console.WriteLine(sw.Elapsed.TotalMicroseconds.ToString("0000000") + ": " + additional);

    }

}