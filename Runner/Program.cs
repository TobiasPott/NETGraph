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
    public const string NInput = "input";
    public const string NOutput = "output";
    public const string VStart = "Start";
    public const string VMid = "Mitte";
    public const string VEnd = "Ende";

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
        ScalarData<string> name = (ScalarData<string>)Memory.Alloc<string>(IData.Options.Scalar);
        name.assign(DataAccessor.Scalar, "blubb");

        ScalarData<int> typeIndex = (ScalarData<int>)Memory.Alloc<int>(IData.Options.Scalar);
        typeIndex.assign(DataAccessor.Scalar, DataTypes.Int);

        Memory.Assign("newInt", Memory.Alloc(typeof(int), IData.Options.Scalar));
        ScalarData<int> rtGenInt = (ScalarData<int>)Memory.Get("newInt");
        rtGenInt.assign(1337);
        Console.WriteLine(rtGenInt);

        Console.WriteLine();
    }

    public static void TimeStamp(Stopwatch sw, string additional)
    {
        Console.WriteLine(sw.Elapsed.TotalMicroseconds.ToString("0000000") + ": " + additional);

    }

}